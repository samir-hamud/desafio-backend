namespace Domain.Entities;

public class Plano 
{
    public Plano(int identificador, string descricao, ushort dias, decimal valor)
    {
        Identificador = identificador;
        Descricao = descricao;
        Dias = dias;
        Valor = valor;
    }
    
    public int Identificador { get; set; }
    public string Descricao { get; set; }
    public ushort Dias { get; set; }
    public decimal Valor { get; set; }
}