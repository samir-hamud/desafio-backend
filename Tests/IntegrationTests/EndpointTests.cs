using System.Globalization;
using System.Net.Http.Json;
using App.Controller.Schemas;
using App.DTO;
using Domain.Entities;
using Infra.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    
    [Fact, TestPriority(13)]
    public async Task adicionar_entregador() 
    {
        var novo = new EntregadorDTO("1", "nome", "cnpj", new DateTime(1990, 1, 1), "1234567890", "A",
            "base64string");
        
        var response = await _client.PostAsJsonAsync("/entregadores", novo);
        
        _output.WriteLine(await response.Content.ReadAsStringAsync());
        Assert.Equal(StatusCodes.Status201Created, (int)response.StatusCode);
    }
    
    [Fact, TestPriority(14)]
    public async Task adicionar_entregador_cnpj_duplicado() 
    {
        var novo = new EntregadorDTO("2", "nome", "cnpj", new DateTime(1990, 1, 1), "123", "A",
            "base64string");

        var response = await _client.PostAsJsonAsync("/entregadores", novo);

        var mensagem = await response.Content.ReadFromJsonAsync<Response>();

        Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
    }

    [Fact, TestPriority(15)]
    public async Task enviar_foto_cnh_entregador()
    {
        var dir = Directory.GetCurrentDirectory();
        var arquivo = await File.ReadAllBytesAsync(Path.Combine(dir, "imagem.png"));
        var request = new AdicionarImagemCNHRequest()
        {
            ImagemCnh = Convert.ToBase64String(arquivo)
        };
        var response = await _client.PostAsJsonAsync("/entregadores/1/cnh", request);
        var db = (MyDbContext?) _factory.Services.GetService(typeof(MyDbContext));
        
        Entregador? entregador = null;
        if (db is not null)
            entregador = await db.Entregadores.FirstOrDefaultAsync(x => x.Identificador == "1");
        
        Assert.NotEqual(string.Empty, entregador?.PathImagemCnh);
        Assert.Equal(StatusCodes.Status201Created, (int)response.StatusCode);
    }

    [Fact, TestPriority(16)]
    public async Task verificar_imagem_cnh_adicionada()
    {
        var db = (MyDbContext?) _factory.Services.GetService(typeof(MyDbContext));
        if (db is null)
            Assert.True(false);
        
        var entregador = await db.Entregadores.FirstOrDefaultAsync(x => x.Identificador == "1");
        
        if (entregador is null)
            Assert.True(false);
        
        if (File.Exists("imagem.png"))
            File.Delete("imagem.png");
        
        if (entregador.PathImagemCnh is not null)
            File.Copy(entregador.PathImagemCnh, "imagem.png", true);
        
        _output.WriteLine(entregador.PathImagemCnh);
        _output.WriteLine(Path.GetFullPath("imagem.png"));
        Assert.True(File.Exists("imagem.png"));
    }
    
    [Fact, TestPriority(17)]
    public async Task alugar_moto()
    {
        var request = new LocacaoRequest
        {
            IdMoto = "moto2",
            IdEntregador = "1",
            DataInicio = DateTime.Now.Date.AddDays(1),
            DataTermino = DateTime.Now.AddDays(7),
            DataPrevisaoTermino = DateTime.Now.AddDays(7),
            IdPlano = 7
        };

        var response = await _client.PostAsJsonAsync("/locacao", request);
        Assert.Equal(StatusCodes.Status201Created, (int)response.StatusCode);
        
        var db = (MyDbContext?) _factory.Services.GetService(typeof(MyDbContext));
        if (db is null)
            Assert.Fail("db é nulo");

        var loc = db.Locacoes.FirstOrDefault(x =>
            x.Entregador.Identificador == "1" && x.Moto.Identificador == "moto2");

        if (loc is null)
            Assert.Fail("locacao não encontrada no db");
        
        Assert.NotNull(loc);
    }
    
    [Fact, TestPriority(18)]
    public async Task informar_devolucao()
    {
        var db = (MyDbContext?) _factory.Services.GetService(typeof(MyDbContext));
        
        if (db is null)
            Assert.Fail("db é nulo");

        var loc = db.Locacoes.FirstOrDefault();
        
        if (loc is null)
            Assert.Fail("não há locações");
        
        var request = new DevolucaoRequest
        {
            DataDevolucao = DateTime.Now.AddDays(5)
        };
        
        var response = await _client.PutAsJsonAsync($"/locacao/{loc.Identificador}/devolucao", request);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }

    [Fact, TestPriority(19)]
    public async Task verificar_data_devolucao()
    {
        var db = (MyDbContext?) _factory.Services.GetService(typeof(MyDbContext));
        
        if (db is null)
            Assert.Fail("db é nulo");

        var loc = await db.Locacoes.Include(x => x.Entregador).Include(x => x.Moto).Include(x => x.Plano)
            .FirstAsync();
        
        db.Entry(loc).State = EntityState.Modified;
        await db.Entry(loc).ReloadAsync();
        _output.WriteLine(loc.CalcularValor().ToString(CultureInfo.InvariantCulture));
        Assert.NotNull(loc);
        Assert.NotNull(loc.DataDevolucao);
    }

    [Fact, TestPriority(20)]
    public async Task consultar_locacao_por_id()
    {
        var db = (MyDbContext?) _factory.Services.GetService(typeof(MyDbContext));
        
        if (db is null)
            Assert.Fail("db é nulo");

        var loc = await db.Locacoes.FirstAsync();
        
        var response = await _client.GetAsync($"/locacao/{loc.Identificador}");
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
        
        var locacao = await response.Content.ReadFromJsonAsync<LocacaoDTO>();
        Assert.NotNull(locacao);
        _output.WriteLine($"{locacao.Identificador} {locacao.DataInicio} {locacao.DataTermino} {locacao.DataPrevisaoTermino}");
        Assert.Equal(loc.Identificador, locacao.Identificador);
    }

    [Fact, TestPriority(21)]
    public async Task verificar_mensagem_publicada()
    {
        var db = (MyDbContext?) _factory.Services.GetService(typeof(MyDbContext));
        if (db is null)
            Assert.Fail("db é nulo");
        
        var mensagem = await db.Mensagens.FirstOrDefaultAsync();
        
        Assert.NotNull(mensagem);
    }
}