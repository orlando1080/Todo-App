using MassTransit;
using ToDoApp.Application.Tasks.Interfaces;

namespace TodoApp.Infrastructure.Queues;

public sealed class MassTransitMessageBus : IMessageBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitMessageBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync<T>(T payload) where T : class
    {
        await _publishEndpoint.Publish(payload).ConfigureAwait(false);
    }
}