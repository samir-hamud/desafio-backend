using App.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Tests;

public class DatabaseFixture : IDisposable
{
    public MyDbContext DbContext { get; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;
        
        DbContext = new MyDbContext(options);
        
        DbContext.Database.EnsureCreated();
        
        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}