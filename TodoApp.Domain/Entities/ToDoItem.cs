using TodoApp.Domain.Events;

namespace TodoApp.Domain.Entities;

public class TodoItem : BaseEntity
{
    public TodoItem(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be null or whitespace.", nameof(title));

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

    public Guid Id { get; init; }

    public string Title { get; private set; }

    public bool IsCompleted { get; init; }
}
