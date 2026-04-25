namespace TodoApp.Application.TodoTasks.Interfaces;

public interface INotificationService
{
    Task SendAsync(string message);
}