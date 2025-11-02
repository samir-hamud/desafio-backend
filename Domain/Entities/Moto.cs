using FluentValidation;

namespace Domain.Entities;

public class Moto : Entity
{
    public Moto(string identificador, ushort ano, string modelo,
        string placa)
    {
        Identificador = identificador;
        Ano = ano;
        Modelo = modelo;
        Placa = placa;
    }

    public ushort Ano { get; set; }
    public string Modelo { get; set; }
    public string Placa { get; set; }
}

public class MotoValidator : AbstractValidator<Moto>
{
    public MotoValidator()
    {
        RuleFor(moto => moto.Identificador).NotEmpty().MinimumLength(1).MaximumLength(20);  
        RuleFor(moto => moto.Modelo).NotEmpty().MinimumLength(1).MaximumLength(50);
        RuleFor(moto => moto.Ano).GreaterThan((ushort)0).LessThan((ushort)9999);
        RuleFor(moto => moto.Placa).NotEmpty().MinimumLength(1).MaximumLength(20);
    }   
}