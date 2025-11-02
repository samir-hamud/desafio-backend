using System.Reflection;
using App.Context;
using App.Controller.Schemas;
using Domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddDbContext<MyDbContext>(o => o.UseNpgsql(dbStr));

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