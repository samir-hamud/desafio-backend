using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MyDbContext>(o => o.UseNpgsql(connectionString));

        services.AddScoped<IMotoRepository, MotoRepository>();
        services.AddScoped<IEntregadorRepository, EntregadorRepository>();
        services.AddScoped<ILocacaoRepository, LocacaoRepository>();
        services.AddScoped<IMensagemRepository, MensagemRepository>();

        return services;
    }
}