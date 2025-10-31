namespace App.DTO;

/// <summary>
/// Classe base para os DTOs
/// Base class for DTOs
/// </summary>
public abstract class BaseDTO
{
    /// <summary>
    /// Todos os DTOs devem ter um ID
    /// All DTOs must have an ID
    /// </summary>
    public long Id { get; set; }
}