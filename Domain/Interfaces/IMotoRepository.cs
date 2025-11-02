using Domain.Entities;
using Domain.Mappings;

namespace Domain.Interfaces;

public interface IMotoRepository : IRepository<Moto>
{
    Task<Moto?> GetByLicensePlateAsync(string licensePlate);
    Task<Moto?> GetByIdentificationAsync(string id);
}