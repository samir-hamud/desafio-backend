using System.Net.Http.Json;
using App.Controller.Schemas;
using App.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Xunit.Abstractions;

namespace Tests.IntegrationTests;

[TestCaseOrderer("Tests.IntegrationTests.PriorityOrderer", "Tests")]
public class EndpointTests : IClassFixture<TestWebApplicationFactory>
{
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;
    
    public EndpointTests(TestWebApplicationFactory factory, ITestOutputHelper output)
    {
        _output = output;
        _factory = factory;
        _client = _factory.CreateClient();
    }

    private static readonly MotoDTO[] Motos = [
        new("moto1", 2020, "modelo1", "placa1"),
        new("moto2", 2021, "modelo2", "placa2"),
        new("moto3", 2022, "modelo3", "placa3"),
        new("moto4", 2023, "modelo4", "placa4"),
        new("moto5", 2024, "modelo5", "placa5")
    ];

    [Fact, TestPriority(1)]
    public async Task adicionar_moto()
    {
        var responses = new List<HttpResponseMessage>();
        
        foreach (var moto in Motos)
            responses.Add(await _client.PostAsJsonAsync("/motos", moto));

        Assert.All(responses,
            response => Assert.Equal(StatusCodes.Status201Created, (int)response.StatusCode));
    }

    [Fact, TestPriority(2)]
    public async Task consultar_todas_motos()
    {
        var response = await _client.GetAsync("/motos");
        var motos = await response.Content.ReadFromJsonAsync<MotoDTO[]>();

        if (motos is not null)
            foreach (var moto in motos)
                _output.WriteLine(moto.Identificador);
        
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(Motos.Length, motos?.Length ?? 0);
    }
    
    [Fact, TestPriority(3)]
    public async Task adicionar_moto_duplicada()
    {
        await _client.PostAsJsonAsync("/motos", Motos[0]);
        var response = await _client.PostAsJsonAsync("/motos", Motos[0]);
        
        Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
    }
    
    [Fact, TestPriority(4)]
    public async Task adicionar_moto_sem_placa()
    {
        var moto = new Moto("moto6", 2025, "modelo6", "");
        var response = await _client.PostAsJsonAsync("/motos", moto);
        
        Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
    }

    [Fact, TestPriority(5)]
    public async Task atualizar_placa_moto()
    {
        var moto = Motos[0];

        var request = new ModificarPlacaRequest
        {
            Placa = "atualizada"
        };
        
        var response = await _client.PutAsJsonAsync($"/motos/{moto.Identificador}/placa", request);
        var mensagem = await response.Content.ReadFromJsonAsync<Response>();
        
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
        Assert.Equal("Placa modificada com sucesso", mensagem?.Mensagem);
    }

    [Fact, TestPriority(6)]
    public async Task atualizar_placa_vazia()
    {
        var moto = Motos[0];

        var request = new ModificarPlacaRequest
        {
            Placa = string.Empty
        };
        
        var response = await _client.PutAsJsonAsync($"/motos/{moto.Identificador}/placa", request);
        
        Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
    }
    
    [Fact, TestPriority(7)]
    public async Task consultar_moto_por_placa()
    {
        var moto = Motos[1];
        var response = await _client.GetAsync($"/motos?placa={moto.Placa}");
        var motos = await response.Content.ReadFromJsonAsync<MotoDTO[]>();
        var motoConsultada = motos?[0];
        
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
        Assert.Equal(moto.Identificador, motoConsultada?.Identificador);
    }
    
    [Fact, TestPriority(8)]
    public async Task deletar_moto()
    {
        var moto = Motos[0];
        var response = await _client.DeleteAsync($"/motos/{moto.Identificador}");
        
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact, TestPriority(9)]
    public async Task deletar_moto_inexistente()
    {
        var response = await _client.DeleteAsync("/motos/moto-inexistente");
        var message = await response.Content.ReadFromJsonAsync<Response>();
        
        Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        Assert.Equal("Dados inválidos", message?.Mensagem);
    }
    
    [Fact, TestPriority(10)]
    public async Task consultar_moto_inexistente()
    {
        var response = await _client.GetAsync("/motos/moto-inexistente");
        var message = await response.Content.ReadFromJsonAsync<Response>();
        
        Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        Assert.Equal("Moto não encontrada", message?.Mensagem);
    }
    
    [Fact, TestPriority(11)]
    public async Task consultar_request_mal_formada()
    {
        var response = await _client.GetAsync("/motos/123457890123457890123456789012345678901234567890");
        var message = await response.Content.ReadFromJsonAsync<Response>();
        
        Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        Assert.Equal("Request mal formada", message?.Mensagem);
    }
    
    [Fact, TestPriority(12)]
    public async Task atualizar_placa_request_mal_formada()
    {
        var moto = Motos[0];

        var request = new ModificarPlacaRequest
        {
            Placa = "123456789012345678901234567890"
        };
        
        var response = await _client.PutAsJsonAsync($"/motos/{moto.Identificador}/placa", request);
        var mensagem = await response.Content.ReadFromJsonAsync<Response>();
        
        Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        Assert.Equal("Request mal formada", mensagem?.Mensagem);
    }
}