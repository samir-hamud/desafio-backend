using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class EntregadorConfiguration : IEntityTypeConfiguration<Entregador>
{
    public void Configure(EntityTypeBuilder<Entregador> builder)
    {
        builder.ToTable("entregadores");
        
        builder.HasKey(x => x.Identificador);
        builder.Property(x => x.Identificador).IsRequired();

        builder.HasIndex(x => x.Cnpj).IsUnique();
        builder.HasIndex(x => x.NumeroCnh).IsUnique();
    }
}