using System.Text.Json.Serialization;
using Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace App.DTO;

[SwaggerSchema]
public class EntregadorDTO : BaseDTO
{
    public EntregadorDTO(string identificador, string nome, string cnpj, DateTime dataNascimento,
        string numeroCnh, string tipoCnh, string imagemCnh, string pathImagem)
    {
        Identificador = identificador;
        Nome = nome;
        Cnpj = cnpj;
        DataNascimento = dataNascimento;
        NumeroCnh = numeroCnh;
        TipoCnh = tipoCnh;
        ImagemCnh = imagemCnh;
        PathImagem = pathImagem;   
    }

    /// <example>entregador123</example>
    public sealed override string Identificador { get; set; }

    /// <example>João da Silva</example>
    public string Nome { get; set; }
    
    /// <example>12345678901234</example>
    public string Cnpj { get; set; }
    
    /// <example>2020-01-01T00:00:00Z</example>
    [JsonPropertyName("data_nascimento")]
    public DateTime DataNascimento { get; set; }
    
    /// <example>12345678900</example>
    [JsonPropertyName("numero_cnh")]
    public string NumeroCnh { get; set; }
    
    /// <example>A</example>
    [JsonPropertyName("tipo_cnh")]
    public string TipoCnh { get; set; }
    
    /// <example>base64string</example>
    [JsonPropertyName("imagem_cnh")]
    public string ImagemCnh { get; set; }
    
    [SwaggerIgnore]
    public string PathImagem { get; set; }
}

public class EntregadorExample : IExamplesProvider<Entregador>
{
    public Entregador GetExamples() => new("entregador123", "João da Silva", "12345678901234",
        new DateTime(1990, 1, 1), "12345678900", "A", "base64string");
}