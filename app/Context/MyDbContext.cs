using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Context;

public partial class MyDbContext : DbContext
{
    public DbSet<Motorcycle> Motorcycles { get; set; }
    
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
