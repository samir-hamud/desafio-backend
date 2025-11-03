using RabbitMQ.Client;

namespace App.MessageBroker.Services;

public abstract class RabbitMqBase
{
    protected readonly IConnection _connection;
    protected IChannel _channel;

    protected RabbitMqBase(ConnectionFactory factory)
    {
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();   
    }

    protected async Task CreateQueueIfNotExists(string queueName)
    {
        await _channel.QueueDeclareAsync(queueName, autoDelete: false, durable: true,
            exclusive: false,
            arguments: new Dictionary<string, object?>());
    }

    ~RabbitMqBase()
    {
        _connection.Dispose();
        _channel.Dispose();   
    }
}