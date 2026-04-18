using System;
using System.Linq;
using NUnit.Framework;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Events;

namespace TodoApp.Domain.Tests.Entities;

[TestFixture]
[TestOf(typeof(TodoItem))]
internal sealed class TodoItemTests
{
    [Test]
    public void Create_ValidTodoItem_ReturnsTodoItem()
    {
        TodoItem result = TodoItem.Create("test");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("test"));
            Assert.That(result.IsCompleted, Is.False);
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        }
    }

    [TestCase(" ")]
    [TestCase("")]
    public void Create_InvalidTodoItem_ThrowsArgumentException(string title) =>
        Assert.Throws<ArgumentException>(() => TodoItem.Create(title));

    [Test]
    public void Create_NullTodoItem_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => TodoItem.Create(null!));

    [Test]
    public void Create_ExtraSpacesTitle_ReturnsTrimmedTitle()
    {
        TodoItem result = TodoItem.Create("   test   ");

        Assert.That(result.Title, Is.EqualTo("test"));
    }

    [Test]
    public void Create_ValidTodoItem_RaisesTaskCreatedDomainEvent()
    {
        TodoItem todoItem = TodoItem.Create("test");

        TaskCreatedDomainEvent domainEvent = todoItem.DomainEvents.OfType<TaskCreatedDomainEvent>().Single();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(domainEvent.Title, Is.EqualTo("test"));
            Assert.That(domainEvent.Id, Is.EqualTo(todoItem.Id));
        }
    }
}