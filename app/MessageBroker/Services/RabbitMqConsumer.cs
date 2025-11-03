using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace App.MessageBroker.Services;

public class RabbitMqConsumer : RabbitMqBase
{
    private readonly AsyncEventingBasicConsumer _consumer;
    
    public RabbitMqConsumer(ConnectionFactory factory) : base(factory)
    {
        _consumer = new AsyncEventingBasicConsumer(_channel);
    }

    public async Task AddListenerAsync(string queueName, AsyncEventHandler<BasicDeliverEventArgs> handler) 
    {
        _consumer.ReceivedAsync += handler;
        await CreateQueueIfNotExists(queueName);

        await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer: _consumer,
            noLocal: false, exclusive: false, consumerTag: Guid.NewGuid().ToString(),
            arguments: new Dictionary<string, object?>());   
    }

    public async Task RemoveListenerAsync(AsyncEventHandler<BasicDeliverEventArgs> handler)
    {
        _consumer.ReceivedAsync -= handler;
    }
}