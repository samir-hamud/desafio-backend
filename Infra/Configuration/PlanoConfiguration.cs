using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class PlanoConfiguration : IEntityTypeConfiguration<Plano>
{
    public void Configure(EntityTypeBuilder<Plano> builder)
    {
        builder.ToTable("planos");
        builder.HasKey(x => x.Identificador);
        builder.Property(x => x.Identificador).IsRequired();
        builder.Property(x => x.Descricao).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Dias).IsRequired();
        builder.Property(x => x.Valor).IsRequired();
        builder.Property(x => x.PercMulta).IsRequired();
    }
}