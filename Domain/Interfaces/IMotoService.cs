using Domain.Entities;

namespace Domain.Interfaces;

public interface IMotoService
{
    Task<IEnumerable<Moto>> GetAllAsync();
    Task<Moto?> GetByIdAsync(string id);
    Task<Moto?> GetByLicensePlateAsync(string licensePlate);
    Task<Moto?> GetByIdentificationAsync(string id);
    Task AddAsync(Moto entity);
    Task UpdateAsync(Moto entity);
    Task DeleteAsync(Moto entity);
}