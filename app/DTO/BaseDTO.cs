using Swashbuckle.AspNetCore.Annotations;

namespace App.DTO;

/// <summary>
/// Classe base para os DTOs
/// Base class for DTOs
/// </summary>
public abstract class BaseDTO
{
    public abstract string Identificador { get; set; }
}