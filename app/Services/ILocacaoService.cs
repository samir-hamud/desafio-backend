using App.DTO;
using Domain.Entities;

namespace App.Services;

public interface ILocacaoService
{
    Task<Locacao?> GetByIdAsync(string id);
    Task<IEnumerable<Locacao>> GetAllAsync();
    Task AddAsync(Locacao? entity);
    Task UpdateAsync(Locacao entity);
    Task DeleteAsync(Locacao entity);

    Task InformarDevolucao(string id, DateTime dataDevolucao);
    Task AddFromDTOAsync(LocacaoDTO dto);
}
