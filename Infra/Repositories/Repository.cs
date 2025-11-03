using Domain.Entities;
using Domain.Mappings;
using Infra.Context;
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
        if (entity is Entity entidade)
        {
            if (string.IsNullOrEmpty(entidade.Identificador))
            {
                entidade.Identificador = Guid.NewGuid().ToString();
            }
        }
        
        await _db.Set<T>().AddAsync(entity);
        await _db.SaveChangesAsync();   
    }

    public virtual async Task UpdateAsync(T entity)
    {
        await _db.SaveChangesAsync();
    }

    public virtual Task BeginUpdate(T entity)
    {
        _db.Set<T>().Attach(entity);
        return Task.CompletedTask;   
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _db.Set<T>().Remove(entity);
        await _db.SaveChangesAsync();   
    }
}