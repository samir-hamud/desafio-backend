using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace App.Controller.Schemas;

[SwaggerSchema]
public class DevolucaoRequest
{
    /// <example>2024-01-07T18:00:00Z</example>  
    [JsonPropertyName("data_devolucao")]
    public DateTime DataDevolucao { get; set; }
}