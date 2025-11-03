using App.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Mappings;

public static class EntregadorMappings
{
    public static EntregadorDTO ToDTO(this Entregador entity)
    {
        return new EntregadorDTO
        (
            entity.Identificador,
            entity.Nome,
            entity.Cnpj,
            entity.DataNascimento,
            entity.NumeroCnh,
            entity.TipoCnh,
            entity.GetImagem()
        );
    }

    public static Entregador ToEntity(this EntregadorDTO dto)
    {
        return new Entregador(dto.Identificador, dto.Nome, dto.Cnpj, dto.DataNascimento, dto.NumeroCnh,
            dto.TipoCnh, dto.ImagemCnh);
    }
}