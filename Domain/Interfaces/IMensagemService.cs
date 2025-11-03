using Domain.Entities;

namespace Domain.Interfaces;

public interface IMensagemService
{
    Task<Mensagem?> GetByIdAsync(string id);
    Task<IEnumerable<Mensagem>> GetAllAsync();
    Task AddAsync(Mensagem entity);
    Task UpdateAsync(Mensagem entity);
    Task DeleteAsync(Mensagem entity);
}