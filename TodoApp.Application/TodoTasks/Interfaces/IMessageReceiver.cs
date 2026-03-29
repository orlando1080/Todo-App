using TodoApp.Application.TodoTasks.Commands;

namespace TodoApp.Application.TodoTasks.Interfaces;

public interface IMessageReceiver
{
    Task<string?> ReceiveAsync(CancellationToken cancellationToken);

    Task DeleteMessageAsync(ReceivedMessage receivedMessage);
}