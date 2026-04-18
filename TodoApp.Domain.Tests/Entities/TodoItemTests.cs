using System;
using System.Threading.Tasks;
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

    [TestCase(null)]
    [TestCase(" ")]
    [TestCase("")]
    public void Create_InvalidTodoItem_ThrowsArgumentException(string title) =>
        Assert.Throws<ArgumentException>(() => TodoItem.Create(title));


}