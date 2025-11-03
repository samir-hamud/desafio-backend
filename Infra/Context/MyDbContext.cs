using Domain.Entities;
using Infra.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infra.Context;

public partial class MyDbContext : DbContext
{
    public DbSet<Moto> Motos { get; set; }
    public DbSet<Entregador> Entregadores { get; set; }
    public DbSet<Plano> Planos { get; set; }
    public DbSet<Mensagem> Mensagens { get; set; }
    public DbSet<Locacao> Locacoes { get; set; }

    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>()
            .HaveConversion<UtcValueConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MotoConfiguration());
        modelBuilder.ApplyConfiguration(new EntregadorConfiguration());
        modelBuilder.ApplyConfiguration(new PlanoConfiguration());
        modelBuilder.ApplyConfiguration(new MensagemConfiguration());
        modelBuilder.ApplyConfiguration(new LocacaoConfiguration());

        modelBuilder.Entity<Plano>().HasData(
            new Plano(7, "7 dias", 7, 30m, 20m),
            new Plano(15, "15 dias", 15, 28m, 40), 
            new Plano(30, "30 dias", 30, 22m, 40),
            new Plano(45, "45 dias", 45, 20m, 40), 
            new Plano(50, "50 dias", 50, 18m, 40));
    }
}

public class UtcValueConverter() : ValueConverter<DateTime, DateTime>(v => v.ToUniversalTime(),
    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
