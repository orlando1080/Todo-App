namespace ToDoApp.Application.Tasks.Interfaces;

public interface IMessageBus
{
    public Task PublishAsync<T>(T message) where T : class;
}