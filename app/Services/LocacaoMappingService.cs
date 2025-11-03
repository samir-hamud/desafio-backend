using App.DTO;
using App.Mappings;
using Domain.Entities;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace App.Services;

public interface ILocacaoMappingService
{
    Task<Locacao> MapFromDTOAsync(LocacaoDTO dto);
    LocacaoDTO MapToDTO(Locacao entity);
}

public class LocacaoMappingService : ILocacaoMappingService
{
    private readonly MyDbContext _context;
    
    public LocacaoMappingService(MyDbContext context)
    {
        _context = context;
    }
    
    public async Task<Locacao> MapFromDTOAsync(LocacaoDTO dto)
    {
        var moto = await _context.Motos
            .FirstOrDefaultAsync(x => x.Identificador == dto.IdMoto);
            
        var entregador = await _context.Entregadores
            .FirstOrDefaultAsync(x => x.Identificador == dto.IdEntregador);
            
        var plano = await _context.Planos
            .FirstOrDefaultAsync(x => x.Identificador == dto.IdPlano);

        if (moto == null || entregador == null || plano == null)
            throw new Exception();
            
        return new Locacao(dto.Identificador, moto, entregador, 
            dto.DataInicio, dto.DataTermino, dto.DataPrevisaoTermino, plano);
    }

    public LocacaoDTO MapToDTO(Locacao entity)
    {
        return entity.ToDTO();
    }
}