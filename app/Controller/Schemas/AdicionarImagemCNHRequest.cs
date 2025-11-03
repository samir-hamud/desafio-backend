using System.Text.Json.Serialization;
using FluentValidation;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace App.Controller.Schemas;

[SwaggerSchema]
public class AdicionarImagemCNHRequest
{
    public AdicionarImagemCNHRequest(string imagemCnh)
    {
        ImagemCnh = imagemCnh;
    }

    public AdicionarImagemCNHRequest()
    {
        
    }

    /// <example>base64string</example>
    [JsonPropertyName("imagem_cnh")]
    public string ImagemCnh { get; set; }
}

public class AdicionarIagemCNHRequestValidator : AbstractValidator<AdicionarImagemCNHRequest>
{
    public AdicionarIagemCNHRequestValidator()
    {
        RuleFor(imagem => imagem.ImagemCnh).NotEmpty();
    }
}