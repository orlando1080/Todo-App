using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TodoApp.Application.TodoTasks.Events;
using TodoApp.Application.TodoTasks.Interfaces;

namespace TodoApp.Infrastructure.TodoTasks;

public class TaskCreatedWorker : BackgroundService
{
    private readonly IMessageBus _queueClient;

    private readonly ILogger _logger;

    private readonly ITaskCreatedDomainEventProcessor _taskCreatedDomainEventProcessor;


    public TaskCreatedWorker(
        IMessageBus queueClient,
        ILogger logger,
        ITaskCreatedDomainEventProcessor taskCreatedDomainEventProcessor)
    {
        _queueClient = queueClient;
        _logger = logger;
        _taskCreatedDomainEventProcessor = taskCreatedDomainEventProcessor;
    }


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

        }
    }
}