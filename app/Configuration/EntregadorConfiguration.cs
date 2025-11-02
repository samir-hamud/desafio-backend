using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Configuration;

public class EntregadorConfiguration : IEntityTypeConfiguration<Entregador>
{
    public void Configure(EntityTypeBuilder<Entregador> builder)
    {
        builder.ToTable("entregadores");
        
        builder.HasKey(x => x.Identificador);
        builder.Property(x => x.Identificador).HasMaxLength(50).IsRequired();
        
        
    }
}