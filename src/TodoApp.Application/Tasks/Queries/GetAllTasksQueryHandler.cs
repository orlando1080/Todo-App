using Mapster;
using TodoApp.Application.Dtos;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.TodoTasks.Queries;

public sealed class GetAllTasksQueryHandler
{
    private readonly ITaskItemRepository _taskItemRepository;

    public GetAllTasksQueryHandler(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<TaskItemDto[]> HandleAsync(GetAllTasksQuery query, CancellationToken cancellationToken)
    {
        TaskItem[] todoItems = await _taskItemRepository.GetAllAsync().ConfigureAwait(false);

        // Manual way: items.Select(x => new TodoResponse(x.id, x.Title, x.IsCompleted)).ToArray();

        return todoItems.Adapt<TaskItemDto[]>(); // Mapster way
    }
}