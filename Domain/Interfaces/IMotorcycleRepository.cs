using Domain.Entities;
using Domain.Mappings;

namespace Domain.Interfaces;

public interface IMotorcycleRepository : IRepository<Motorcycle>
{
    Task<Motorcycle?> GetByLicensePlateAsync(string licensePlate);
}