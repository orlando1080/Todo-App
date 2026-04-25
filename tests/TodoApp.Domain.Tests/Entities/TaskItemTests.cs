using System;
using System.Linq;
using NUnit.Framework;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Events;

namespace TodoApp.Domain.Tests.Entities;

[TestFixture]
[TestOf(typeof(TaskItem))]
internal sealed class TaskItemTests
{
    [Test]
    public void Create_ValidTodoItem_ReturnsTodoItem()
    {
        TaskItem result = TaskItem.Create("test");

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
        Assert.Throws<ArgumentException>(() => TaskItem.Create(title));

    [Test]
    public void Create_NullTodoItem_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => TaskItem.Create(null!));

    [Test]
    public void Create_ExtraSpacesTitle_ReturnsTrimmedTitle()
    {
        TaskItem result = TaskItem.Create("   test   ");

        Assert.That(result.Title, Is.EqualTo("test"));
    }

    [Test]
    public void Create_ValidTodoItem_RaisesTaskCreatedDomainEvent()
    {
        TaskItem taskItem = TaskItem.Create("test");

        TaskCreatedDomainEvent domainEvent = taskItem.DomainEvents.OfType<TaskCreatedDomainEvent>().Single();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(domainEvent.Title, Is.EqualTo("test"));
            Assert.That(domainEvent.Id, Is.EqualTo(taskItem.Id));
        }
    }

    [Test]
    public void ToggleIsCompleted_ToggleOnce_EqualsTrue()
    {
        TaskItem taskItem = TaskItem.Create("test");

        taskItem.ToggleIsCompleted();

        Assert.That(taskItem.IsCompleted, Is.True);
    }

    [Test]
    public void ToggleIsCompleted_ToggleTwice_EqualsFalse()
    {
        TaskItem taskItem = TaskItem.Create("test");

        taskItem.ToggleIsCompleted();
        taskItem.ToggleIsCompleted();

        Assert.That(taskItem.IsCompleted, Is.False);
    }
}