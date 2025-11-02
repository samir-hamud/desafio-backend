using App.Controller.Schemas;
using App.DTO;
using Domain.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace App.Controller;

public abstract class AppController<TEntity, TDto>(IRepository<TEntity> repo) : ControllerBase
    where TEntity : class
    where TDto : class
{
    protected readonly IRepository<TEntity> _repo = repo;

    public abstract Task<IActionResult> GetAll();
    public abstract Task<IActionResult> Get(string id);
    public abstract Task<IActionResult> Post(TDto dto);

    [NonAction]
    protected Response DadosInvalidos() => new("Dados inválidos");
}