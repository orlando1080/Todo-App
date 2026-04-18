using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.TodoTasks.Commands;

public sealed class DeleteTaskCommandHandler
{
    private readonly ITodoRepository _todoRepository;

    public DeleteTaskCommandHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task HandleAsync(DeleteTaskCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        await _todoRepository.DeleteAsync(command.Id).ConfigureAwait(false);
    }
}