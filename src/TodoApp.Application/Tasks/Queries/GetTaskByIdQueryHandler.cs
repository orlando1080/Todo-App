using Mapster;
using TodoApp.Application.Dtos;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.TodoTasks.Queries;

public sealed class GetTaskByIdQueryHandler
{
    private readonly ITaskItemRepository _taskItemRepository;

    public GetTaskByIdQueryHandler(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<TaskItemDto?> HandleAsync(GetTaskByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        TaskItem? todoItem = await _taskItemRepository.GetByIdAsync(query.Id).ConfigureAwait(false);

        return todoItem?.Adapt<TaskItemDto>();
    }
}