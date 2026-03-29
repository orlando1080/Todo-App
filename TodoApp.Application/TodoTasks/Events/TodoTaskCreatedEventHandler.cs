using TodoApp.Application.TodoTasks.Interfaces;
using TodoApp.Domain.Events;

namespace TodoApp.Application.TodoTasks.Events;

public class TodoTaskCreatedEventHandler : ITodoTaskCreatedEventHandler
{
    private readonly INotificationService _notificationService;

    public TodoTaskCreatedEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task HandleAsync(TaskCreatedDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        await _notificationService.SendAsync($"{domainEvent.Title} has been created").ConfigureAwait(false);
    }
}
