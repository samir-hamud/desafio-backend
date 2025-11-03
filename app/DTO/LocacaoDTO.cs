using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace App.DTO;

public class LocacaoDTO : BaseDTO
{
    public LocacaoDTO(){}
    
    public LocacaoDTO(string identificador, string idMoto, string idEntregador, DateTime dataInicio, DateTime dataTermino, DateTime dataPrevisaoTermino, int idPlano)
    {
        Identificador = identificador;
        IdMoto = idMoto;
        IdEntregador = idEntregador;
        DataInicio = dataInicio;
        DataTermino = dataTermino;
        DataPrevisaoTermino = dataPrevisaoTermino;
        IdPlano = idPlano;
    }

    public LocacaoDTO(string identificador, string idMoto, string idEntregador, DateTime dataInicio,
        DateTime dataTermino, DateTime dataPrevisaoTermino, int idPlano, decimal valor) : this(
        identificador, idMoto, idEntregador, dataInicio, dataTermino, dataPrevisaoTermino, idPlano)
    {
        Valor = valor;
    }
    /// <example>locacao123</example>
    public sealed override string Identificador { get; set; }
    /// <example>moto123</example>
    [JsonPropertyName("moto_id")]
    public string IdMoto { get; set; }
    /// <example>entregador123</example>
    [JsonPropertyName("entregador_id")]   
    public string IdEntregador { get; set; }
    /// <example>2024-01-01T00:00:00Z</example>
    [JsonPropertyName("data_inicio")]
    public DateTime DataInicio { get; set; }
    
    /// <example>2024-01-07T23:59:59Z</example>
    [JsonPropertyName("data_termino")]
    public DateTime DataTermino { get; set; }
    /// <example>2024-01-07T18:00:00Z</example>
    [JsonPropertyName("data_previsao_termino")]
    public DateTime DataPrevisaoTermino { get; set; }
    /// <example>10</example>   
    [JsonPropertyName("valor_diaria")] 
    public decimal Valor { get; set; }
    
    [JsonIgnore]
    [SwaggerIgnore]
    public int IdPlano { get; set; }
}