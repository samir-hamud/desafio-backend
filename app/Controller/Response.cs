namespace App.Controller;

public class Response
{
    public Response(string mensagem)
    {
        this.mensagem = mensagem;
    }

    public string mensagem { get; set; }
}