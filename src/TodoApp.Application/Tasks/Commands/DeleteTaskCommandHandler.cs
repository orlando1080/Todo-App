using TodoApp.Domain.Interfaces;

namespace ToDoApp.Application.Tasks.Commands;

public sealed class DeleteTaskCommandHandler
{
    private readonly ITaskItemRepository _taskItemRepository;

    public DeleteTaskCommandHandler(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task HandleAsync(DeleteTaskCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        await _taskItemRepository.DeleteAsync(command.Id).ConfigureAwait(false);
    }
}