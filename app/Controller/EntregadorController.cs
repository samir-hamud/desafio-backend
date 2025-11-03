using App.Controller.Schemas;
using App.DTO;
using App.Mappings;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace App.Controller;

[ApiController]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class EntregadorController(IEntregadorService service, ILogger<EntregadorController> logger)
    : AppController<Entregador, EntregadorDTO>
{
    private readonly ILogger _logger = logger;

    [HttpPost("/entregadores")]
    [SwaggerOperation(OperationId = "addEntregador", Tags = ["entregadores"])]
    [SwaggerResponse(StatusCodes.Status201Created, "")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos", typeof(Response))]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DadosInvalidosExample))]
    public async Task<IActionResult> Post([FromBody] EntregadorDTO dto)
    {
        try
        {
            _logger.LogInformation("Adicionando entregador");
            var ent = dto.ToEntity();
            await service.AddAsync(ent);
        }
        catch (Exception)
        {
            _logger.LogError("Erro ao adicionar entregador");
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

        var ent = await service.GetByIdAsync(id);

        if (ent is null)
            return BadRequest(DadosInvalidos());

        try
        {
            _logger.LogInformation("Salvando imagem");
            var path = await service.SalvarImagemCnhAsync(id, body.ImagemCnh);
            
            if (string.IsNullOrEmpty(path))
                return BadRequest(DadosInvalidos());
            
            ent.PathImagemCnh = path;
            await service.UpdateAsync(ent);
            _logger.LogInformation("Imagem salva com sucesso");
        }
        catch (Exception)
        {
            _logger.LogError("Erro ao salvar imagem");
            return BadRequest(DadosInvalidos());
        }
        
        return CreatedAtAction(nameof(PostImagem), null);   

    }
}