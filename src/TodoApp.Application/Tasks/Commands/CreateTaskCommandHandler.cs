using Mapster;
using TodoApp.Application.Dtos;
using ToDoApp.Application.Tasks.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Events;
using TodoApp.Domain.Interfaces;

namespace ToDoApp.Application.Tasks.Commands;

public sealed class CreateTaskCommandHandler
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IMessageBus _messageBus;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskCommandHandler(
        ITaskItemRepository taskItemRepository,
        IMessageBus messageBus,
        IUnitOfWork unitOfWork)
    {
        _taskItemRepository = taskItemRepository;
        _messageBus = messageBus;
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskItemDto> HandleAsync(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        TaskItem taskItem = TaskItem.Create(command.Title);

        await _taskItemRepository.Add(taskItem, cancellationToken).ConfigureAwait(false);

        await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

        foreach (IDomainEvent domainEvent in taskItem.DomainEvents)
        {
            if (domainEvent is TaskCreatedDomainEvent created)
            {
                await _messageBus.PublishAsync(created).ConfigureAwait(false);
            }
        }

        return taskItem.Adapt<TaskItemDto>();
    }
}