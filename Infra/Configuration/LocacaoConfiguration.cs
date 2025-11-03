using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class LocacaoConfiguration : IEntityTypeConfiguration<Locacao>
{
    public void Configure(EntityTypeBuilder<Locacao> builder)
    {
        builder.ToTable("locacoes");
        builder.HasKey(x => x.Identificador);
        builder.HasIndex(x => x.Identificador).IsUnique();
        builder.HasOne(x => x.Entregador)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Moto)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Plano)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.DataInicio).IsRequired();
        builder.Property(x => x.DataTermino).IsRequired();
        builder.Property(x => x.DataPrevisaoTermino).IsRequired();
        builder.Property(x => x.DataDevolucao);
    }
}