using TodoApp.Domain.Events;

namespace TodoApp.Domain.Entities;

public sealed class TodoItem : BaseEntity
{
    private TodoItem(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        Id = Guid.NewGuid();
        Title = title.Trim();
        IsCompleted = false;
    }

    public static TodoItem Create(string title)
    {
        TodoItem todoItem = new(title);

        // ANNOUNCING: "This is a new object"

        todoItem.AddDomainEvent(new TaskCreatedDomainEvent(todoItem.Id, todoItem.Title));

        return todoItem;
    }

    public void ToggleIsCompleted()
    {
        IsCompleted = !IsCompleted;
    }

    public Guid Id { get; init; }

    public string Title { get; private set; }

    public bool IsCompleted { get; private set; }
}
