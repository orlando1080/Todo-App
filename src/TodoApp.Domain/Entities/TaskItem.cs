using TodoApp.Domain.Events;

namespace TodoApp.Domain.Entities;

public sealed class TaskItem : BaseEntity
{
    private TaskItem(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        Id = Guid.NewGuid();
        Title = title.Trim();
        IsCompleted = false;
    }

    public static TaskItem Create(string title)
    {
        TaskItem taskItem = new(title);

        // ANNOUNCING: "This is a new object"

        taskItem.AddDomainEvent(new TaskCreatedDomainEvent(taskItem.Id, taskItem.Title));

        return taskItem;
    }

    public void ToggleIsCompleted()
    {
        IsCompleted = !IsCompleted;
    }

    public Guid Id { get; init; }

    public string Title { get; private set; }

    public bool IsCompleted { get; private set; }
}
