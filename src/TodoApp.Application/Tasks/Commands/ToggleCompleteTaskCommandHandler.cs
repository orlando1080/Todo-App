using ToDoApp.Application.Tasks.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace ToDoApp.Application.Tasks.Commands;

public sealed class ToggleCompleteTaskCommandHandler
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ToggleCompleteTaskCommandHandler(ITaskItemRepository taskItemRepository, IUnitOfWork unitOfWork)
    {
        _taskItemRepository = taskItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(ToggleCompleteTaskCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        TaskItem? todoItem = await _taskItemRepository.GetByIdAsync(command.Id).ConfigureAwait(false);

        if (todoItem is null) throw new InvalidOperationException();

        todoItem.ToggleIsCompleted();

        await _taskItemRepository.UpdateAsync(todoItem).ConfigureAwait(false);

        await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
    }
}