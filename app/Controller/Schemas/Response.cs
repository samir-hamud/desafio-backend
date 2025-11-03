using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace App.Controller.Schemas;

[SwaggerSchema]
public class Response
{
    public Response(string mensagem)
    {
        Mensagem = mensagem;
    }

    public string Mensagem { get; set; }
}

public class PlacaModificadaComSucessoExample : IExamplesProvider<Response>
{
    public Response GetExamples() => new("Placa modificada com sucesso");   
}

public class DadosInvalidosExample : IExamplesProvider<Response>
{
    public Response GetExamples() => new("Dados inválidos");  
}

public class RequestMalFormadaExample : IExamplesProvider<Response>
{
    public Response GetExamples() => new("Request mal formada");  
}

public class MotoNaoEncontradaExample : IExamplesProvider<Response>
{
    public Response GetExamples() => new("Moto não encontrada");
}

public class LocacaoNaoEncontradaExample : IExamplesProvider<Response>
{
    public Response GetExamples() => new("Locação não encontrada");
}

public class DevolucaoInformadaExample : IExamplesProvider<Response>
{
    public Response GetExamples() => new("Data de devolução informada com sucesso");
}