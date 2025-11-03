namespace Domain.Entities;

public class Plano 
{
    public Plano(int identificador, string descricao, ushort dias, decimal valor, decimal percMulta)
    {
        Identificador = identificador;
        Descricao = descricao;
        Dias = dias;
        Valor = valor;
        PercMulta = percMulta;       
    }
    
    public int Identificador { get; set; }
    public string Descricao { get; set; }
    public ushort Dias { get; set; }
    public decimal Valor { get; set; }
    public decimal PercMulta { get; set; }
}