using App.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.EnableAnnotations();
    o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "App.xml"));
    o.SwaggerDoc("v1",
        new OpenApiInfo { Title = "Sistema de manutenção de motos", Version = "v1" });    
} );

var dbStr = builder.Configuration.GetConnectionString("DbContext");
builder.Services.AddDbContext<MyDbContext>(o => o.UseNpgsql(dbStr));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    await ctx.Database.EnsureCreatedAsync();
}

app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "Desafio Backend");
    o.RoutePrefix = string.Empty;
});


app.UseHttpsRedirection();
app.MapControllers();
app.Run();