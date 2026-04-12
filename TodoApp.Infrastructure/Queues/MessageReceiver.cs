using TodoApp.Application.TodoTasks.Commands;
using TodoApp.Application.TodoTasks.Interfaces;

namespace TodoApp.Infrastructure.Queues;

public sealed class MessageReceiver : IMessageReceiver
{
    public Task<string?> ReceiveAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteMessageAsync(ReceivedMessage receivedMessage)
    {
        throw new NotImplementedException();
    }
}