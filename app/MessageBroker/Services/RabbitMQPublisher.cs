using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace App.MessageBroker.Services;

public class RabbitMqPublisher : RabbitMqBase
{
    public RabbitMqPublisher(ConnectionFactory factory) : base(factory)
    {
    }

    public async Task PublishMessageAsync(string queueName, object message)
    {
        await CreateQueueIfNotExists(queueName);
        
        var messageJson = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageJson);

        await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body);
    }
}