using FluentValidation;

namespace Domain.Entities;

public class Locacao : Entity
{
    public Locacao(string identificador, Moto moto, Entregador entregador, DateTime dataInicio,
        DateTime dataTermino, DateTime dataPrevisaoTermino, Plano plano)
    {
        Identificador = identificador;
        Moto = moto;
        Entregador = entregador;
        DataInicio = dataInicio;
        DataTermino = dataTermino;
        DataPrevisaoTermino = dataPrevisaoTermino;
        Plano = plano;
    }

    public Moto Moto { get; set; }
    public Entregador Entregador { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public DateTime DataPrevisaoTermino { get; set; }
    public Plano Plano { get; set; }
}

public class LocacaoValidator : AbstractValidator<Locacao>
{
    public LocacaoValidator()
    {
        RuleFor(locacao => locacao.Identificador).NotEmpty().MinimumLength(1).MaximumLength(20);
        RuleFor(locacao => locacao.Moto).NotNull();
        RuleFor(locacao => locacao.Entregador).NotNull().DependentRules(() =>
            RuleFor(locacao => locacao.Entregador.TipoCnh).NotEmpty()
                .Must(tipoCnh => tipoCnh.Contains('A')));
        RuleFor(locacao => locacao.DataInicio).NotEmpty();
        RuleFor(locacao => locacao.DataTermino).NotEmpty();
        RuleFor(locacao => locacao.DataPrevisaoTermino).NotEmpty();
        RuleFor(locacao => locacao.Plano).NotNull();
    }   
}