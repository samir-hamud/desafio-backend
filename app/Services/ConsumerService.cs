using System.Text;
using System.Text.Json;
using App.DTO;
using App.MessageBroker.Services;
using Domain.Entities;
using Domain.Interfaces;

namespace App.Services;

public class ConsumerService : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly RabbitMqConsumer _consumer;

    public ConsumerService(IServiceScopeFactory factory, RabbitMqConsumer consumer)
    {
        _factory = factory;
        _consumer = consumer;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumer.AddListenerAsync("Moto2024", async (_, args) =>
        {
            using var scope = _factory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IMensagemService>();
            var json = Encoding.UTF8.GetString(args.Body.Span);

            var message = new Mensagem
            {
                Data = DateTime.Now,
                JsonMoto = json
            };
            
            await service.AddAsync(message);
        });
    }
}