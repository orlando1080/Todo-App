using Mapster;
using TodoApp.Application.Dtos;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace ToDoApp.Application.Tasks.Queries;

public sealed class GetAllTasksQueryHandler
{
    private readonly ITaskItemRepository _taskItemRepository;

    public GetAllTasksQueryHandler(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<TaskItemDto[]> HandleAsync(GetAllTasksQuery query, CancellationToken cancellationToken)
    {
        TaskItem[] taskItems = await _taskItemRepository.GetAllAsync().ConfigureAwait(false);

        // Manual way: items.Select(x => new TodoResponse(x.id, x.Title, x.IsCompleted)).ToArray();

        return taskItems.Adapt<TaskItemDto[]>(); // Mapster way
    }
}