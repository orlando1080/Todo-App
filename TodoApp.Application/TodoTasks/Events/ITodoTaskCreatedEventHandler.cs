using TodoApp.Domain.Events;

namespace TodoApp.Application.TodoTasks.Events;

public interface ITodoTaskCreatedEventHandler
{
    Task HandleAsync(TaskCreatedDomainEvent domainEvent);

}