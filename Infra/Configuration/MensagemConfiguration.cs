using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;

public class MensagemConfiguration : IEntityTypeConfiguration<Mensagem>
{
    public void Configure(EntityTypeBuilder<Mensagem> builder)
    {
        builder.ToTable("mensagens");
        
        builder.HasKey(x => x.Identificador);
        builder.Property(x => x.Identificador).IsRequired();
        builder.Property(x => x.JsonMoto).IsRequired();
        builder.Property(x => x.Data).IsRequired();
    }
}