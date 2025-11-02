using App.Configuration;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Context;

public partial class MyDbContext : DbContext
{
    public DbSet<Moto> Motos { get; set; }
    public DbSet<Entregador> Entregadores { get; set; }
    public DbSet<Plano> Planos { get; set; }
    
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MotoConfiguration());
        modelBuilder.ApplyConfiguration(new EntregadorConfiguration());

        modelBuilder.Entity<Plano>().HasData(
            new Plano(7, "7 dias", 7, 30m),
            new Plano(15, "15 dias", 15, 28m), 
            new Plano(30, "30 dias", 30, 22m),
            new Plano(45, "45 dias", 45, 20m), 
            new Plano(50, "50 dias", 50, 18m));
    }
}
