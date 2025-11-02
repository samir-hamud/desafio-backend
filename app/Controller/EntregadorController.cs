using App.Controller.Schemas;
using App.DTO;
using App.Mappings;
using App.Utils;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace App.Controller;

[ApiController]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class EntregadorController(IEntregadorRepository repo)
    : AppController<Entregador, EntregadorDTO>(repo)
{
    [NonAction]
    public override Task<IActionResult> GetAll()
    {
        return Task.FromResult<IActionResult>(BadRequest());
    }

    [NonAction]
    public override Task<IActionResult> Get(string id)
    {
        return Task.FromResult<IActionResult>(BadRequest());
    }

    [HttpPost("/entregadores")]
    [SwaggerOperation(OperationId = "addEntregador", Tags = ["entregadores"])]
    [SwaggerResponse(StatusCodes.Status201Created, "")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos", typeof(Response))]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DadosInvalidosExample))]
    public override async Task<IActionResult> Post([FromBody] EntregadorDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(DadosInvalidos());

        try
        {
            await repo.AddAsync(dto.ToEntity());
        }
        catch (Exception)
        {
            return BadRequest(DadosInvalidos());
        }
        
        return CreatedAtAction(nameof(Post), null);
    }

    [HttpPost("/entregadores/{id}/cnh")]
    [SwaggerOperation(OperationId = "addImagemCnh", Tags = ["entregadores"])]
    [SwaggerResponse(StatusCodes.Status201Created, "")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos", typeof(Response))]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DadosInvalidosExample))]
    public async Task<IActionResult> PostImagem([FromRoute] string id,
        [FromBody] AdicionarImagemCNHRequest body)
    {
        if (string.IsNullOrEmpty(body.ImagemCnh))
            return BadRequest(DadosInvalidos());

        var ent = await repo.GetByIdAsync(id);

        if (ent is null)
            return BadRequest(DadosInvalidos());

        try
        {
            var path = await CnhUtils.SalvarImagem(id, body.ImagemCnh);
            
            if (string.IsNullOrEmpty(path))
                return BadRequest(DadosInvalidos());
            
            ent.PathImagemCnh = path;
            await repo.UpdateAsync(ent);
        }
        catch (Exception)
        {
            return BadRequest(DadosInvalidos());
        }
        
        return CreatedAtAction(nameof(PostImagem), null);   

    }
}