using Domain.Entities;

namespace Domain.Interfaces;

public interface IEntregadorService
{
    Task AddAsync(Entregador? entity);
    Task<Entregador?> GetByIdAsync(string id);
    Task UpdateAsync(Entregador entity);
    Task<string?> SalvarImagemCnhAsync(string entregadorId, string imagemBase64);
}