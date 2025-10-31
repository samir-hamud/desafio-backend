using App.Context;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class MotorcycleRepository : Repository<Motorcycle>, IMotorcycleRepository
{
    public MotorcycleRepository(MyDbContext db) : base(db)
    {
    }

    public async Task<Motorcycle?> GetByLicensePlateAsync(string licensePlate)
    {
        return await _db.Motorcycles.FirstOrDefaultAsync(x => x.LicensePlate.Equals(licensePlate));
    }
}