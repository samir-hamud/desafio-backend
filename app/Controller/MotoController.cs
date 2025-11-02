using System.ComponentModel.DataAnnotations;
using App.Controller.Schemas;
using App.DTO;
using App.Mappings;
using Domain.Entities;
using Domain.Interfaces;
using MassTransit.Internals;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace App.Controller;

[ApiController]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class MotoController(IMotoRepository repo, ILogger<MotoController> logger)
    : AppController<Moto, MotoDTO>(repo)
{
    private readonly ILogger _logger = logger;
    
    /// <summary>
    /// Consultar motos existentes
    /// </summary>
    /// <param name="placa">Placa para filtro opcional</param>
    [HttpGet("/motos")]
    [SwaggerOperation(OperationId = "listMotos", Tags = ["motos"])]
    [SwaggerResponse(StatusCodes.Status200OK, "Lista de motos", typeof(IEnumerable<MotoDTO>))]
    public async Task<IActionResult> GetAll([FromQuery] string? placa)
    {
        if (string.IsNullOrWhiteSpace(placa))
        {
            var all = await repo.GetAllAsync();
            return Ok(all.Select(e => e.ToDTO()));
        }

        var moto = await repo.GetByLicensePlateAsync(placa);
        IEnumerable<MotoDTO> list = moto is null ? Array.Empty<MotoDTO>() : new[] { moto.ToDTO() };
        return Ok(list);
    }

    [NonAction]
    public override async Task<IActionResult> GetAll()
    {
        var all = await repo.GetAllAsync();
        return Ok(all.Select(e => e.ToDTO()));
    }

    /// <summary>
    /// Consultar motos existentes por id
    /// </summary>
    [HttpGet("/motos/{id}")]
    [SwaggerOperation(OperationId = "getMotoById", Tags = ["motos"])]
    [SwaggerResponse(StatusCodes.Status200OK, "Detalhes da moto", typeof(MotoDTO))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MotoExample))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Request mal formada")]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(RequestMalFormadaExample))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(MotoNaoEncontradaExample))]
    public override async Task<IActionResult> Get(
        [FromRoute] [StringLength(20, MinimumLength = 1)]
        string id)
    {
        var result = await repo.GetByIdAsync(id);

        if (result == null)
            return NotFound(new Response("Moto não encontrada"));

        return Ok(result.ToDTO());
    }

    /// <summary>
    /// Cadastrar uma nova moto
    /// </summary>
    /// <response code="201"></response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost("/motos")]
    [Consumes("application/json")]
    [SwaggerOperation(OperationId = "createMoto", Tags = ["motos"])]
    [SwaggerResponse(StatusCodes.Status201Created, "")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos", typeof(Response))]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DadosInvalidosExample))]
    public override async Task<IActionResult> Post([FromBody] MotoDTO dto)
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

    /// <summary>
    /// Modificar a placa de uma moto.
    /// </summary>
    /// <response code="200">Placa modificada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPut("/motos/{id}/placa")]
    [Consumes("application/json")]
    [SwaggerOperation(Summary = "Modificar a placa de uma moto", OperationId = "updateMotoPlaca",
        Tags = ["motos"])]
    [SwaggerResponse(StatusCodes.Status200OK, "Placa modificada com sucesso", typeof(Response))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PlacaModificadaComSucessoExample))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos", typeof(Response))]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DadosInvalidosExample))]
    public async Task<IActionResult> Put([FromRoute] string id,
        [FromBody] ModificarPlacaRequest body)
    {
        var moto = await repo.GetByIdentificationAsync(id);

        if (moto is null)
            return BadRequest(DadosInvalidos());
        
        moto.Placa = body.Placa;

        try
        {
            await repo.UpdateAsync(moto);
        }
        catch (Exception)
        {
            return BadRequest(DadosInvalidos());
        }
        
        return Ok(new Response("Placa modificada com sucesso"));
    }

    /// <summary>
    /// Remover uma moto.
    /// </summary>
    [HttpDelete("/motos/{id}")]
    [SwaggerOperation(Summary = "Remover uma moto", OperationId = "deleteMoto", Tags = ["motos"])]
    [SwaggerResponse(StatusCodes.Status200OK, "")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos", typeof(Response))]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(DadosInvalidosExample))]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var moto = await repo.GetByIdentificationAsync(id);
        
        if (moto == null)
            return BadRequest(DadosInvalidos());
        
        await repo.DeleteAsync(moto);
        
        return Ok();   
    }
}