using System.Net;
using System.Net.Mime;
using App.Controller.Examples;
using App.DTO;
using App.Mappings;
using Azure.Core;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Examples;

namespace App.Controller;

[Route("api/[controller]")]
[ApiController]
public class MotorcycleController(IMotorcycleRepository repo)
    : AppController<Motorcycle, MotorcycleDTO>(repo)
{
    /// <summary>
    /// Consultar motos existentes
    /// </summary>
    /// <response code="200">Lista de motos</response>  
    /// <returns></returns>
    [HttpGet("/motos")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromBody] string licensePlate) 
    {
        var result = await _repo.GetAllAsync();

        return Ok(result);
    }

    public override Task<IActionResult> GetAll()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Consultar motos existentes por id
    /// </summary>
    /// <response code="200">Detalhes da moto</response> 
    /// <response code="400">Request mal formada</response> 
    /// <response code="404">Moto não encontrada</response> 
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("/motos/{id:long}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override async Task<IActionResult> Get(long id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Response("Request mal formada"));
        }

        var result = await _repo.GetByIdAsync(id);

        if (result == null)
            return NotFound("Moto não encontrada");

        return Ok(result);
    }

    /// <summary>
    /// Cadastrar uma moto nova 
    /// </summary>
    /// <response code="201"></response> 
    /// <response code="400">Dados inválidos</response> 
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("/motos")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
    [SwaggerResponseExample(HttpStatusCode.BadRequest, typeof(MotorcycleExamples))]
    public override async Task<IActionResult> Post(MotorcycleDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new Response("Dados inválidos"));

        try
        {
            await _repo.AddAsync(dto.ToEntity());
        }
        catch (Exception)
        {
            return BadRequest(new Response("Dados inválidos"));
        }
        
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    /// <summary>
    /// Modificar a placa de uma moto
    /// </summary>
    /// <response code="200">Placa modificada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <param name="id"></param>
    /// <param name="placa"></param>
    [HttpPut("/motos/{id:long}/{placa}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(long id, [FromBody] string placa)
    {
        if (!ModelState.IsValid)
            return BadRequest(new Response("Dados inválidos"));

        var moto = await _repo.GetByIdAsync(id);

        if (moto == null)
            return BadRequest(new Response("Dados inválidos"));
        
        moto.LicensePlate = placa;

        try
        {
            await _repo.UpdateAsync(moto);
        }
        catch (Exception )
        {
            return BadRequest(new Response("Dados inválidos"));
        }
        
        return Ok(new Response(""));
    }

    /// <summary>
    /// Remover uma moto
    /// </summary>
    /// <response code="200"></response>
    /// <response code="400">Dados inválidos</response>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("/motos/{id:long}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(long id)
    {
        var moto = await _repo.GetByIdAsync(id);
        
        if (moto == null)
            return BadRequest(new Response("Dados inválidos"));
        
        await _repo.DeleteAsync(moto);
        
        return Ok();   
    }
}