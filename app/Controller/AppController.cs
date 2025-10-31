using App.DTO;
using Domain.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace App.Controller;

public abstract class AppController<TEntity, TDto>(IRepository<TEntity> repo) : ControllerBase
    where TEntity : class
    where TDto : BaseDTO
{
    protected readonly IRepository<TEntity> _repo = repo;

    public abstract Task<IActionResult> GetAll();
    public abstract Task<IActionResult> Get(long id);
    public abstract Task<IActionResult> Post(TDto dto);
}