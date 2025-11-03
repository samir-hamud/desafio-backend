using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class LocacaoRepository(MyDbContext db) : Repository<Locacao>(db), ILocacaoRepository
{
    public override async Task<Locacao?> GetByIdAsync(string id)
    {
        return await _db.Set<Locacao>()
            .Include(x => x.Moto)
            .Include(x => x.Entregador)
            .Include(x => x.Plano)
            .FirstOrDefaultAsync(x => x.Identificador.Equals(id));
    }
}