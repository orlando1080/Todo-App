using ToDoApp.Application.Tasks.Interfaces;

namespace TodoApp.Infrastructure.TodoTasks;

public sealed class NotificationService : INotificationService
{
    public Task SendAsync(string message)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}