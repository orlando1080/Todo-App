using System;
using NUnit.Framework;
using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Tests.Entities;

[TestFixture]
[TestOf(typeof(TodoItem))]
internal sealed class TodoItemTests
{
    [Test]
    public void Create_ValidTodoItem_returnsTodoItem()
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
    public void Create_ValidTodoItem_RaisesTaskCreatedDomainEven()
    {
        TodoItem todoItem = TodoItem.Create("test");

        Assert.That(todoItem.DomainEvents, Has.Count.EqualTo(1));
    }
}