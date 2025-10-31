using App.DTO;
using Domain.Entities;

namespace App.Mappings;

public static class MotorcycleMapping
{
    public static MotorcycleDTO ToDTO(this Motorcycle entity)
    {
        return new MotorcycleDTO
        {
            Id = entity.Id,
            Identification = entity.Identification,
            Year = entity.Year,
            Model = entity.Model,
            LicensePlate = entity.LicensePlate
        };
    }
    
    public static Motorcycle ToEntity(this MotorcycleDTO dto)
    {
        return new Motorcycle(dto.Id, dto.Identification, dto.Year, dto.Model, dto.LicensePlate);
    }
}