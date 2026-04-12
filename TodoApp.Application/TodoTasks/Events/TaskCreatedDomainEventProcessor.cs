using TodoApp.Application.TodoTasks.Interfaces;
using TodoApp.Domain.Events;

namespace TodoApp.Application.TodoTasks.Events;

public sealed class TaskCreatedDomainEventProcessor : ITaskCreatedDomainEventProcessor
{
    private readonly INotificationService _notificationService;

    public TaskCreatedDomainEventProcessor(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task HandleAsync(TaskCreatedDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        await _notificationService.SendAsync($"{domainEvent.Title} has been created").ConfigureAwait(false);
    }
}
