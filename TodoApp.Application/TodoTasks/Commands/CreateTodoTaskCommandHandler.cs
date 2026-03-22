using TodoApp.Application.TodoTasks.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Events;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.TodoTasks.Commands;

public class CreateTodoTaskCommandHandler
{
    private readonly ITodoRepository _todoRepository;
    private readonly IMessageBus _messageBus;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTodoTaskCommandHandler(
        ITodoRepository todoRepository,
        IMessageBus messageBus,
        IUnitOfWork unitOfWork)
    {
        _todoRepository = todoRepository;
        _messageBus = messageBus;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(AddTaskCommand addTaskCommand, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(addTaskCommand);

        TodoItem todoItem = TodoItem.Create(addTaskCommand.Title);

        await _todoRepository.AddAsync(todoItem).ConfigureAwait(false);
        await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

        foreach (IDomainEvent domainEvent in todoItem.DomainEvents)
        {
            if (domainEvent is TaskCreatedDomainEvent created)
            {
                await _messageBus.PublishAsync(created).ConfigureAwait(false);
            }
        }
    }
}