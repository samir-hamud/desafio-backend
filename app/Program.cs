using System.Configuration;
using System.Reflection;
using App.Controller.Schemas;
using App.MessageBroker.Services;
using App.Services;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infra;
using Infra.Context;
using Infra.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvc();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblies([
    typeof(Program).Assembly,
    typeof(MotoValidator).Assembly,
    typeof(ModificarPlacaRequestValidator).Assembly
]);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = _ =>
        new BadRequestObjectResult(new Response("Request mal formada"));
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Desafio Backend",
        Version = "v1"
    });
    options.ExampleFilters();
    options.EnableAnnotations();
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "App.xml"));
});

builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

var dbStr = builder.Configuration.GetConnectionString("DbContext");
builder.Services.AddInfra(dbStr ?? throw new ArgumentNullException($"ConnectionString"));
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IEntregadorService, EntregadorService>();
builder.Services.AddScoped<ILocacaoService, LocacaoService>();
builder.Services.AddScoped<ILocacaoMappingService, LocacaoMappingService>();
builder.Services.AddScoped<IMensagemService, MensagemService>();
builder.Services.AddSingleton(new ConnectionFactory
{
    HostName = builder.Configuration.GetConnectionString("RabbitMQ") ?? throw new ArgumentNullException(nameof(RabbitMQ))
});
builder.Services.AddSingleton<RabbitMqPublisher>();
builder.Services.AddSingleton<RabbitMqConsumer>();
builder.Services.AddHostedService<ConsumerService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    await ctx.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Desafio Backend");
        c.RoutePrefix = string.Empty;
        c.DefaultModelsExpandDepth(-1);
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

public partial class Program
{
}