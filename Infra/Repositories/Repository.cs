using App.Context;
using Domain.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly MyDbContext _db;

    protected Repository(MyDbContext db)
    {
        _db = db;
    }
    
    public virtual async Task<T?> GetByIdAsync(string id)
    {
        return await _db.FindAsync<T>(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _db.Set<T>().ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _db.Set<T>().AddAsync(entity);
        await _db.SaveChangesAsync();   
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _db.Set<T>().Update(entity);
        await _db.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _db.Set<T>().Remove(entity);
        await _db.SaveChangesAsync();   
    }
}