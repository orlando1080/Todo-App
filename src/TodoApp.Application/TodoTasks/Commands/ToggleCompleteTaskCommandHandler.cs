using TodoApp.Application.TodoTasks.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.TodoTasks.Commands;

public sealed class ToggleCompleteTaskCommandHandler
{
    private readonly ITodoRepository _todoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ToggleCompleteTaskCommandHandler(ITodoRepository todoRepository, IUnitOfWork unitOfWork)
    {
        _todoRepository = todoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(ToggleCompleteTaskCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        TodoItem? todoItem = await _todoRepository.GetByIdAsync(command.Id).ConfigureAwait(false);

        if (todoItem is null) throw new InvalidOperationException();

        todoItem.ToggleIsCompleted();

        await _todoRepository.UpdateAsync(todoItem).ConfigureAwait(false);

        await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
    }
}