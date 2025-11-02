using App.DTO;
using Domain.Entities;

namespace App.Mappings;

public static class MotoMappings
{
    public static MotoDTO ToDTO(this Moto entity)
    {
        return new MotoDTO
        (
            entity.Identificador,
            entity.Ano,
            entity.Modelo,
            entity.Placa
        );
    }
    
    public static Moto ToEntity(this MotoDTO dto)
    {
        return new Moto(dto.Identificador, dto.Ano, dto.Modelo, dto.Placa);
    }
}