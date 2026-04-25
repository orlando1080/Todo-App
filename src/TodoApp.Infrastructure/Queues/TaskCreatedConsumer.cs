using MassTransit;
using ToDoApp.Application.Tasks.Events;
using TodoApp.Domain.Events;

namespace TodoApp.Infrastructure.Queues;

public sealed class TaskCreatedConsumer : IConsumer<TaskCreatedDomainEvent>
{
    private readonly ITaskCreatedDomainEventProcessor _taskCreatedDomainEventProcessor;

    public TaskCreatedConsumer(ITaskCreatedDomainEventProcessor taskCreatedDomainEventProcessor)
    {
        _taskCreatedDomainEventProcessor = taskCreatedDomainEventProcessor;
    }

    public async Task Consume(ConsumeContext<TaskCreatedDomainEvent> context)
    {
        ArgumentNullException.ThrowIfNull(context);

        await _taskCreatedDomainEventProcessor.HandleAsync(context.Message).ConfigureAwait(false);
    }
}