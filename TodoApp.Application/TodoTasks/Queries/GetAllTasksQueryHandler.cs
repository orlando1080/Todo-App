using Mapster;
using TodoApp.Application.Dtos;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.TodoTasks.Queries;

public sealed class GetAllTasksQueryHandler
{
    private readonly ITodoRepository _todoRepository;

    public GetAllTasksQueryHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<TodoItemDto[]> HandleAsync(GetAllTasksQuery query, CancellationToken cancellationToken)
    {
        TodoItem[] todoItems = await _todoRepository.GetAllAsync().ConfigureAwait(false);

        // Manual way: items.Select(x => new TodoResponse(x.id, x.Title, x.IsCompleted)).ToArray();

        return todoItems.Adapt<TodoItemDto[]>(); // Mapster way
    }
}