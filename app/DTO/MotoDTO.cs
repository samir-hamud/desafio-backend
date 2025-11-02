using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace App.DTO;

[SwaggerSchema]
public class MotoDTO : BaseDTO
{
    public MotoDTO(string identificador, ushort ano, string modelo, string placa)
    {
        Identificador = identificador;
        Ano = ano;
        Modelo = modelo;
        Placa = placa;
    }

    public MotoDTO()
    {
        
    }

    /// <example>moto123</example>
    public sealed override string Identificador { get; set; }
    
    /// <example>2020</example>
    public ushort Ano { get; set; }

    /// <example>Yamaha</example>
    public string Modelo { get; set; }

    /// <example>CDX-0101</example>j
    public string Placa { get; set; }

}

public class MotoExample : IExamplesProvider<MotoDTO>
{
    public MotoDTO GetExamples() => new("moto123", 2020, "Yamaha", "CDX-0101");  
}
