using Mapster;
using TodoApp.Application.Dtos;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace ToDoApp.Application.Tasks.Queries;

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

        TaskItem? taskItem = await _taskItemRepository.GetByIdAsync(query.Id).ConfigureAwait(false);

        return taskItem?.Adapt<TaskItemDto>();
    }
}