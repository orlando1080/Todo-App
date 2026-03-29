using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TodoApp.Application.TodoTasks.Events;
using TodoApp.Application.TodoTasks.Interfaces;

namespace TodoApp.Infrastructure.TodoTasks;

public class TaskCreatedWorker : BackgroundService
{
    private readonly IMessageBus _queueClient;

    private readonly ILogger _logger;

    private readonly ITodoTaskCreatedEventHandler _todoTaskCreatedEventHandler;


    public TaskCreatedWorker(
        IMessageBus queueClient,
        ILogger logger,
        ITodoTaskCreatedEventHandler todoTaskCreatedEventHandler)
    {
        _queueClient = queueClient;
        _logger = logger;
        _todoTaskCreatedEventHandler = todoTaskCreatedEventHandler;
    }


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

        }
    }
}