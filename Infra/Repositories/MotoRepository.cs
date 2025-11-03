using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class MotoRepository(MyDbContext db) : Repository<Moto>(db), IMotoRepository
{
    private MotoValidator _validator = new();
    
    public override async Task AddAsync(Moto entity)
    {
        var validation = await _validator.ValidateAsync(entity);
        
        if (!validation.IsValid)
            throw new MotoValidacaoException(validation.ToString());
        
        await base.AddAsync(entity);
    }

    public override async Task UpdateAsync(Moto entity)
    {
        var validation = await _validator.ValidateAsync(entity);
        
        if (!validation.IsValid)
            throw new MotoValidacaoException(validation.ToString());
        
        await base.UpdateAsync(entity);
    }

    public async Task<Moto?> GetByLicensePlateAsync(string licensePlate)
    {
        return await _db.Motos.FirstOrDefaultAsync(x => x.Placa.Equals(licensePlate));
    }

    public async Task<Moto?> GetByIdentificationAsync(string id)
    {
        return await _db.Motos.FirstOrDefaultAsync(x => x.Identificador.Equals(id));
    }
}

public class MotoValidacaoException(string msg) : Exception(msg);