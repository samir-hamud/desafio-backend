namespace Domain.Entities;

public class Entregador : Entity
{
    public Entregador(string identificador, string nome, string cnpj, DateTime dataNascimento,
        string numeroCnh, string tipoCnh, string? pathImagemCnh)
    {
        Identificador = identificador;
        Nome = nome;
        Cnpj = cnpj;
        DataNascimento = dataNascimento;
        NumeroCnh = numeroCnh;
        TipoCnh = tipoCnh;
        PathImagemCnh = pathImagemCnh;   
    }
    
    public string Nome { get; set; }
    public string Cnpj { get; set; }
    public DateTime DataNascimento { get; set; }
    public string NumeroCnh { get; set; }
    public string TipoCnh { get; set; }
    public string? PathImagemCnh { get; set; }
    
    public string GetImagem() => PathImagemCnh != null ? Convert.ToBase64String(File.ReadAllBytes(PathImagemCnh)) : string.Empty; 
}