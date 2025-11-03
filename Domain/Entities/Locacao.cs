using FluentValidation;

namespace Domain.Entities;

public class Locacao : Entity
{
    public Locacao(){}
    
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
    public DateTime? DataDevolucao { get; set; }
    public Plano Plano { get; set; }
    
    public decimal CalcularValor()
    {
        var minimoLocacao = (int)(DataPrevisaoTermino.Date - DataInicio.Date).TotalDays;
        var totalDiarias =
            Math.Min(
                (int)((DataDevolucao?.Date ?? DataPrevisaoTermino.Date) - DataInicio.Date)
                .TotalDays, minimoLocacao);
        var valorDiarias = totalDiarias * Plano.Valor;

        var diariasMulta =
            (int)((DataDevolucao?.Date ?? DataPrevisaoTermino.Date) - DataPrevisaoTermino.Date).TotalDays;

        if (diariasMulta < 0)
            valorDiarias += Plano.Valor * (1 + Plano.PercMulta / 100) * Math.Abs(diariasMulta);
        else if (diariasMulta > 0)
            valorDiarias += (Plano.Valor + 50m) * diariasMulta;

        return valorDiarias;
    }
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