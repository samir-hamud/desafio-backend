using App.Controller.Schemas;
using App.DTO;
using Domain.Entities;

namespace App.Mappings;

public static class LocacaoMappings
{
    public static LocacaoDTO ToDTO(this Locacao entity)
    {
        return new LocacaoDTO
        (
            entity.Identificador,
            entity.Moto.Identificador,
            entity.Entregador.Identificador,
            entity.DataInicio,
            entity.DataTermino,
            entity.DataPrevisaoTermino,
            entity.Plano.Identificador,
            entity.Plano.Valor
        );
    }
    
    public static LocacaoDTO ToDTO(this LocacaoRequest request)
    {
        return new LocacaoDTO
        (
            string.Empty,
            request.IdMoto,
            request.IdEntregador,
            request.DataInicio,
            request.DataTermino,
            request.DataPrevisaoTermino,
            request.IdPlano
        );   
    }
}