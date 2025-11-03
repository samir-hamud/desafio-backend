using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests.UnitTests;

[Collection("entregador")]
public class EntregadorTests : IDisposable
{
    private readonly ITestOutputHelper _output;
    private readonly SqliteConnection _connection;
    private readonly MyDbContext _context;
    private readonly IEntregadorRepository _repo;
    
    public EntregadorTests(ITestOutputHelper output)
    {
        _output = output;
        
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        
       var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;
       
        _context = new MyDbContext(options);
        _repo = new EntregadorRepository(_context);
        
        _context.Database.EnsureCreated();
    }
    
    public void Dispose()
    {
        _connection.Close();
        _context.Dispose();
    }

    [Fact]
    public async Task adicionar_entregador()
    {
        var novo = new Entregador("1", "nome", "cnpj", new DateTime(1990, 1, 1), "1234567890", "A",
            "base64string");
        
        await _repo.AddAsync(novo);
        
        var ent = await _repo.GetByIdAsync("1");
        
        Assert.Equal("nome", ent?.Nome);
    }

    [Fact]
    public async Task verificar_planos()
    {
        var planos = await _context.Planos.ToListAsync();
        
        Assert.NotEmpty(planos);
    }
}