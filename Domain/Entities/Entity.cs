using FluentValidation;

namespace Domain.Entities;

public abstract class Entity
{
    public string Identificador { get; set; }
}