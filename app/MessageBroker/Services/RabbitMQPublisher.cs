using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace App.MessageBroker.Services;

public interface IRabbitMqPublisher<in T> 
{
    Task PublishMessageAsync(T message, string queueName);
}

public class RabbitMQPublisher<T>(IOptions<RabbitMqSettings> settings) : IRabbitMqPublisher<T>
{
    private readonly RabbitMqSettings _settings = settings.Value;

    public async Task PublishMessageAsync(T message, string queueName)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(queueName, durable: false, exclusive: false,
            autoDelete: false, arguments: null);
        
        var messageJson = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageJson);

        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body);

    }
}