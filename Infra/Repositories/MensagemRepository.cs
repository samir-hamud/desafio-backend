using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;

namespace Infra.Repositories;

public class MensagemRepository : Repository<Mensagem>, IMensagemRepository
{
    public MensagemRepository(MyDbContext db) : base(db)
    {
    }
}