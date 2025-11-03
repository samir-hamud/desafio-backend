using System.ComponentModel.DataAnnotations;
using App.Controller.Schemas;
using App.DTO;
using App.Mappings;
using App.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace App.Controller;

[ApiController]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class LocacaoController(ILocacaoService service, ILogger<LocacaoController> logger) :
    AppController<Locacao, LocacaoDTO>
{
    private readonly ILogger _logger = logger;

    /// <summary>
    /// Consultar locação por id
    /// </summary>
    [HttpGet("/locacao/{id}")]
    [SwaggerOperation(OperationId = "getLocacaoById", Tags = ["locação"])]
    [SwaggerResponse(StatusCodes.Status200OK, "Detalhes da locação", typeof(LocacaoDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DadosInvalidosExample))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Dados não encontrados")]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(LocacaoNaoEncontradaExample))]
    public async Task<IActionResult> Get(
        [FromRoute] string id)
    {
        var result = await service.GetByIdAsync(id);

        if (result is null)
            return NotFound(new Response("Locação não encontrada"));

        return Ok(result.ToDTO());
    }

    [HttpPost("/locacao")]
    [Consumes("application/json")]
    [SwaggerOperation(OperationId = "createLocacao", Tags = ["locação"])]
    [SwaggerResponse(StatusCodes.Status201Created, "")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DadosInvalidosExample))]
    public async Task<IActionResult> PostLocacao([FromBody] LocacaoRequest request)
    {
        try
        {
            _logger.LogInformation("Adicionando nova locação");
            var dto = request.ToDTO();
            await service.AddFromDTOAsync(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar locação");
            return BadRequest(DadosInvalidos());
        }
        
        return CreatedAtAction(nameof(PostLocacao), null);
    }

    [HttpPut("/locacao/{id}/devolucao")]
    [Consumes("application/json")]
    [SwaggerOperation(OperationId = "createDevolucao", Tags = ["locação"])]
    [SwaggerResponse(StatusCodes.Status200OK, "Data de devolução informada com sucesso")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DevolucaoInformadaExample))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DadosInvalidosExample))]
    public async Task<IActionResult> PutDevolucao([FromRoute] string id,
        [FromBody] DevolucaoRequest request)
    {
        try
        {
            _logger.LogInformation("Informando devolucão");
            await service.InformarDevolucao(id, request.DataDevolucao);
        }
        catch (Exception)
        {
            _logger.LogError("Erro ao informar devolução");
            return BadRequest(DadosInvalidos());
        }

        return Ok(new Response("Data de devolução informada com sucesso"));
    }
}