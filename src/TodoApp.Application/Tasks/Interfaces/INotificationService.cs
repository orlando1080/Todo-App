namespace ToDoApp.Application.Tasks.Interfaces;

public interface INotificationService
{
    Task SendAsync(string message);
}