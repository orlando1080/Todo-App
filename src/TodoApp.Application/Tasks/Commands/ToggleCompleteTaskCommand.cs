namespace TodoApp.Application.TodoTasks.Commands;

public sealed record ToggleCompleteTaskCommand(Guid Id);