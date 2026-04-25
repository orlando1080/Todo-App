using TodoApp.Domain.Events;

namespace ToDoApp.Application.Tasks.Events;

public interface ITaskCreatedDomainEventProcessor
{
    Task HandleAsync(TaskCreatedDomainEvent domainEvent);

}