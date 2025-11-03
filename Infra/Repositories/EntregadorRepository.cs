using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;

namespace Infra.Repositories;

public class EntregadorRepository(MyDbContext db)
    : Repository<Entregador>(db), IEntregadorRepository;