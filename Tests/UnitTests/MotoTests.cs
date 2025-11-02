using App.Context;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests.UnitTests;

[Collection("moto")]
public class MotoTests : IDisposable
{
    private readonly ITestOutputHelper _output;
    private readonly SqliteConnection _connection;
    private readonly MyDbContext _context;
    private readonly IMotoRepository _repo;
    private readonly MotoValidator _validator;
    
    public MotoTests(ITestOutputHelper output)
    {
        _output = output;
        _validator = new MotoValidator();
        
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        
       var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;
       
        _context = new MyDbContext(options);
        _repo = new MotoRepository(_context);
        
        _context.Database.EnsureCreated();
    }
    
    public void Dispose()
    {
        _connection.Close();
        _context.Dispose();
    }
    
    [Fact]
    public async Task adicionar_moto()
    {
        var newMoto = new Moto("moto1", 2020, "Model", "AAA-1111");
        
        await _repo.AddAsync(newMoto);

        var moto = await _repo.GetByIdAsync("moto1");

        Assert.Multiple(() => Assert.NotNull(moto), () => Assert.Equal("AAA-1111", moto?.Placa));
    }
    
    [Fact]
    public async Task adicionar_moto_duplicada()
    {
        var newMoto = new Moto("moto1", 2020, "Model", "123");
        var outraMoto = new Moto("moto1", 2020, "Model", "124");

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _repo.AddAsync(newMoto);
            await _repo.AddAsync(outraMoto);
        });
    }
    
    [Fact]
    public async Task adicionar_moto_com_placa_duplicada()
    {
        var newMoto = new Moto("moto1", 2020, "Model", "AAA-1111");
        var outraMoto = new Moto("moto2", 2024, "Model1", "AAA-1111");

        await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            await _repo.AddAsync(newMoto);
            await _repo.AddAsync(outraMoto);
        });
    }

    [Fact]
    public async Task adicionar_moto_sem_placa_request_validacao()
    {
        var newMoto = new Moto("moto3", 2020, "Model", "");
        var result = await _validator.ValidateAsync(newMoto);
        _output.WriteLine(result.ToString());
        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task atualizar_placa_moto_vazia()
    {
        var newMoto = new Moto("moto", 2020, "modelo", "placa");
        await _repo.AddAsync(newMoto);

        newMoto.Placa = string.Empty;
        
        await Assert.ThrowsAsync<MotoValidacaoException>(async () =>
        {
            await _repo.UpdateAsync(newMoto);
        });  
    }

    [Fact]
    public async Task adicionar_moto_sem_placa_repo_validacao()
    {
        var newMoto = new Moto("moto3", 2020, "Model", "");
        await Assert.ThrowsAsync<MotoValidacaoException>(async () =>
        {
            await _repo.AddAsync(newMoto);
        });   
    }
    
    [Fact]
    public async Task atualizar_placa_moto()
    {
        var newMoto = new Moto("moto1", 2020, "Model", "AAA-1111");
        await _repo.AddAsync(newMoto);
        
        newMoto.Placa = "AAA-2222";
        await _repo.UpdateAsync(newMoto);
        
        var moto = await _repo.GetByIdAsync("moto1");
        Assert.Equal("AAA-2222", moto?.Placa);
    }

    [Fact]
    public async Task atualizar_placa_moto_duplicada()
    {
        var newMoto = new Moto("moto1", 2020, "Model", "AAA-1111");
        await _repo.AddAsync(newMoto);
        
        var outraMoto = new Moto("moto2", 2024, "Model1", "AAA-2222");
        await _repo.AddAsync(outraMoto);
        
        newMoto.Placa = "AAA-2222";
        
        await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            await _repo.UpdateAsync(newMoto);
        });
    }
    
    
}