using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests.UnitTests;

[Collection("locacao")]
public class LocacaoTests : IDisposable
{
    
    private readonly ITestOutputHelper _output;
    private readonly SqliteConnection _connection;
    private readonly MyDbContext _context;
    private readonly ILocacaoRepository _repo;
    
    public LocacaoTests(ITestOutputHelper output)
    {
        _output = output;
        
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;
       
        _context = new MyDbContext(options);
        _repo = new LocacaoRepository(_context);
        
        _context.Database.EnsureCreated();
    }
    
    public void Dispose()
    {
        _connection.Close();
        _context.Dispose();
    }

    [Fact]
    public async Task update_data_devolucao()
    {
        var novaMoto = new Moto("moto1", 2020, "modelo", "placa");
        await _context.Motos.AddAsync(novaMoto);

        var novoEntregador = new Entregador("1", "nome", "cnpj", new DateTime(1990, 1, 1),
            "1234567890", "A", null);
        await _context.Entregadores.AddAsync(novoEntregador);
        
        var plano = _context.Planos.First(x => x.Identificador == 7);
        
        var loc = new Locacao
        {
            Moto = novaMoto,
            Entregador = novoEntregador,
            DataInicio = DateTime.Now.Date,
            DataTermino = DateTime.Now.AddDays(7),
            DataPrevisaoTermino = DateTime.Now.AddDays(7),
            Plano = plano,
            DataDevolucao = null
        };
        
        await _repo.AddAsync(loc);
        
        loc.DataDevolucao = DateTime.Now.Date.AddDays(2);
        await _repo.UpdateAsync(loc);
        
        var updated = await _repo.GetAllAsync();
        var enumerable = updated.ToList();
        Assert.NotNull(enumerable.FirstOrDefault());
        Assert.Equal(DateTime.Now.Date.AddDays(2), enumerable.FirstOrDefault()?.DataDevolucao);
    }
    
    [Fact]
    public async Task update_data_devolucao_com_GetByIdAsync()
    {
        var novaMoto = new Moto("moto1", 2020, "modelo", "placa");
        await _context.Motos.AddAsync(novaMoto);

        var novoEntregador = new Entregador("1", "nome", "cnpj", new DateTime(1990, 1, 1),
            "1234567890", "A", null);
        await _context.Entregadores.AddAsync(novoEntregador);
        
        var plano = _context.Planos.First(x => x.Identificador == 7);
        
        var loc = new Locacao
        {
            Moto = novaMoto,
            Entregador = novoEntregador,
            DataInicio = DateTime.Now.Date,
            DataTermino = DateTime.Now.AddDays(7),
            DataPrevisaoTermino = DateTime.Now.AddDays(7),
            Plano = plano,
            DataDevolucao = null
        };
        
        await _repo.AddAsync(loc);
        await _repo.BeginUpdate(loc);
        loc.DataDevolucao = DateTime.Now.Date.AddDays(2);
        await _repo.UpdateAsync(loc);
        
        var updated = await _repo.GetAllAsync();
        var novo = updated.First();
        
        var updated2 = await _repo.GetByIdAsync(novo.Identificador);
        Assert.NotNull(updated2);
        Assert.Equal(DateTime.Now.Date.AddDays(2), updated2?.DataDevolucao);
    }
    
    [Fact]
    public async Task verificar_valores()
    {
        var novaMoto = new Moto("moto1", 2020, "modelo", "placa");
        await _context.Motos.AddAsync(novaMoto);

        var novoEntregador = new Entregador("1", "nome", "cnpj", new DateTime(1990, 1, 1),
            "1234567890", "A", null);
        await _context.Entregadores.AddAsync(novoEntregador);
        
        var plano = _context.Planos.First(x => x.Identificador == 7);
        
        var loc = new Locacao
        {
            Moto = novaMoto,
            Entregador = novoEntregador,
            DataInicio = DateTime.Now.Date,
            DataTermino = DateTime.Now.AddDays(10),
            DataPrevisaoTermino = DateTime.Now.AddDays(10),
            Plano = plano,
            DataDevolucao = null
        };
        
        await _repo.AddAsync(loc);
        var l = await _repo.GetAllAsync();
        loc = l.First();

        loc.DataDevolucao = DateTime.Now.Date.AddDays(3);
        var valor = 3 * loc.Plano.Valor + 7 * loc.Plano.Valor * (1 + loc.Plano.PercMulta / 100);
        Assert.Equal(valor, loc.CalcularValor());
        
        loc.DataDevolucao = DateTime.Now.Date.AddDays(14);
        var valor2 = 10 * loc.Plano.Valor + 4 * (loc.Plano.Valor + 50);
        Assert.Equal(valor2, loc.CalcularValor());
    }
}