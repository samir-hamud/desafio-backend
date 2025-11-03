using App.Controller.Schemas;
using App.DTO;
using Microsoft.AspNetCore.Mvc;

namespace App.Controller;

public abstract class AppController<TEntity, TDto> : ControllerBase
    where TEntity : class
    where TDto : class
{
    [NonAction]
    protected Response DadosInvalidos() => new("Dados inválidos");
}