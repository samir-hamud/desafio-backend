using App.Context;
using Domain.Entities;
using Domain.Interfaces;

namespace Infra.Repositories;

public class EntregadorRepository(MyDbContext db)
    : Repository<Entregador>(db), IEntregadorRepository;