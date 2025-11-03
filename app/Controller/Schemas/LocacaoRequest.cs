using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace App.Controller.Schemas;

[SwaggerSchema]
public class LocacaoRequest
{
    public LocacaoRequest(){}
    
    /// <example>entregador123</example>   
    [JsonPropertyName("entregador_id")]
    public string IdEntregador { get; set; }
    
    /// <example>moto123</example>   
    [JsonPropertyName("moto_id")]
    public string IdMoto { get; set; }
    
    /// <example>2024-01-01T00:00:00Z</example>
    [JsonPropertyName("data_inicio")]
    public DateTime DataInicio { get; set; }
    
    /// <example>2024-01-07T23:59:59Z</example>
    [JsonPropertyName("data_termino")]
    public DateTime DataTermino { get; set; }
    
    /// <example>2024-01-07T18:00:00Z</example>  
    [JsonPropertyName("data_previsao_termino")]
    public DateTime DataPrevisaoTermino { get; set; }
    
    /// <example>7</example> 
    [JsonPropertyName("plano")]
    public int IdPlano { get; set; }
}