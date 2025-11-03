using App.MessageBroker.Services;
using App.Services;
using Domain.Interfaces;
using DotNet.Testcontainers.Builders;
using Infra.Context;
using Infra.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
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
            services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.WriteIndented = true;
            });

            services.AddScoped<IMotoRepository, MotoRepository>();
            services.AddScoped<IEntregadorRepository, EntregadorRepository>();
            services.AddScoped<IMotoService, MotoService>();
            services.AddScoped<IEntregadorService, EntregadorService>();
            services.AddScoped<ILocacaoService, LocacaoService>();
            services.AddScoped<ILocacaoRepository, LocacaoRepository>();
            services.AddScoped<IMensagemRepository, MensagemRepository>();
            services.AddScoped<IMensagemService, MensagemService>();
        });
        
        builder.ConfigureAppConfiguration(config =>
        {
            config.Add(new PostgresConfigSource());
            config.Add(new RabbitMqConfigSource());
        }); 
        
        builder.ConfigureServices((context, services) =>
        {
            var pgHost = context.Configuration.GetConnectionString("DbContext");
            services.AddDbContext<MyDbContext>(o => o.UseNpgsql(pgHost));
            var rabbitMqHost = context.Configuration.GetConnectionString("RabbitMQ");
            if (!string.IsNullOrEmpty(rabbitMqHost))
            {
                var uri = new Uri(rabbitMqHost);
                services.AddSingleton(new ConnectionFactory
                {
                    HostName = uri.Host,
                    Port = uri.Port,
                    UserName = uri.UserInfo.Split(':')[0],
                    Password = uri.UserInfo.Split(':')[1]
                });
                services.AddSingleton<RabbitMqPublisher>();
                services.AddSingleton<RabbitMqConsumer>();
                services.AddHostedService<ConsumerService>();
            }
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
            .WithPassword("1234")
            .WithEnvironment("POSTGRES_PASSWORD", "1234")
            .WithPortBinding(5436, 5432)
            .Build();
        
        await pgContainer.StartAsync().ConfigureAwait(false);
        
        var connectionString = pgContainer.GetConnectionString();
        Data.Add("ConnectionStrings:DbContext", connectionString);
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
            .WithName("rabbitmq-test")
            .WithUsername("desafio")
            .WithPassword("desafio")
            .WithEnvironment("RABBITMQ_DEFAULT_USER", "desafio")
            .WithEnvironment("RABBITMQ_DEFAULT_PASS", "desafio")
            .WithPortBinding(5674, 5672)
            .WithPortBinding(15674, 15672)
            .Build();

        await rbContainer.StartAsync().ConfigureAwait(false);

        Data.Add("ConnectionStrings:RabbitMQ",  rbContainer.GetConnectionString());
    }
}