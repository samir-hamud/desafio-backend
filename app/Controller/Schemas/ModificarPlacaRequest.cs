using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Swashbuckle.AspNetCore.Annotations;

namespace App.Controller.Schemas;

[SwaggerSchema]
public class ModificarPlacaRequest
{
    /// <example>ABC-1234</example>   
    [Required]
    public string Placa { get; set; }
}

public class ModificarPlacaRequestValidator : AbstractValidator<ModificarPlacaRequest>
{
    public ModificarPlacaRequestValidator()
    {
        RuleFor(placa => placa.Placa).NotEmpty().MinimumLength(1).MaximumLength(20);
    }
}