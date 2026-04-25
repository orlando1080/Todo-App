using TodoApp.Domain.Events;

namespace TodoApp.Application.TodoTasks.Events;

public interface ITaskCreatedDomainEventProcessor
{
    Task HandleAsync(TaskCreatedDomainEvent domainEvent);

}