using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class MotoConfiguration : IEntityTypeConfiguration<Moto>
{
    public void Configure(EntityTypeBuilder<Moto> builder)
    {
        builder.ToTable("motos");

        builder.HasKey(x => x.Identificador);
        builder.HasIndex(x => x.Identificador).IsUnique();
        builder.HasIndex(x => x.Placa).IsUnique();
        builder.Property(x => x.Placa).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Identificador).IsRequired();
        builder.Property(x => x.Ano).IsRequired();
        builder.Property(x => x.Modelo).HasMaxLength(50).IsRequired();
    }
}