using TodoApp.Application.TodoTasks.Interfaces;

namespace TodoApp.Infrastructure.TodoTasks;

public class NotificationService : INotificationService
{
    public Task SendAsync(string message)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}