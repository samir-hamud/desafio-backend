using App.Context;
using Domain.Interfaces;
using Infra.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace Tests.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    public TestWebApplicationFactory()
    {
        
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
            {
                options.SerializerOptions.WriteIndented = true;
            });

            services.AddScoped<IMotoRepository, MotoRepository>();
            services.AddDbContext<MyDbContext>();
        });
        
        builder.ConfigureAppConfiguration(config =>
        {
            config.Add(new PostgresConfigSource());
            config.Add(new RabbitMqConfigSource());
        });
    }
}

sealed class PostgresConfigSource : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new PostgresConfigProvider();
    }
}

sealed class PostgresConfigProvider : ConfigurationProvider
{
    private static readonly TaskFactory TaskFactory = new(CancellationToken.None,
        TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);
    
    public override void Load()
    {
        TaskFactory.StartNew(LoadAsync)
            .Unwrap()
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    }
    
    public async Task LoadAsync()
    {
        var pgContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithEnvironment("POSTGRES_PASSWORD", "1234")
            .WithPortBinding(5435, 5432)
            .Build();
        
        await pgContainer.StartAsync().ConfigureAwait(false);
        
        var connectionString = pgContainer.GetConnectionString();
        Data.Add("ConnectionStrings:DefaultConnection", connectionString);
    }
}

sealed class RabbitMqConfigSource : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new RabbitMqConfigProvider();
    }
}

sealed class RabbitMqConfigProvider: ConfigurationProvider
{
    private static readonly TaskFactory TaskFactory = new(CancellationToken.None,
        TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);
    
    public override void Load()
    {
        TaskFactory.StartNew(LoadAsync)
            .Unwrap()
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    }
    
    public async Task LoadAsync()
    {
        var rbContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:latest")
            .WithName("rabbitmqtests")
            .WithPortBinding(5672, 5672)
            .Build();

        await rbContainer.StartAsync().ConfigureAwait(false);
        Data.Add("RabbitMq:ConnectionString", rbContainer.GetConnectionString());
    }
}