using Microsoft.Extensions.Logging;
using Moq;
using TodoApp.Application.Adtos;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Tests;

[TestFixture]
[TestOf(typeof(TodoApplicationService))]
internal sealed class TodoApplicationServiceTests
{
    private readonly Mock<ITodoRepository> _todoRepositoryMock = new();

    private readonly Mock<ILogger<TodoApplicationService>> _loggerMock = new();

    private TodoApplicationService _sut = null!;

    [SetUp]
    public void Setup()
    {
        _sut = new TodoApplicationService(
            _todoRepositoryMock.Object,
            _loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _todoRepositoryMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllTodoItems()
    {
        TodoItem todoItem = new("test");
        TodoItem todoItem2 = new("test2");

        _todoRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync([todoItem, todoItem2]);

        TodoItemDto[] results = await _sut.GetAllAsync().ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            _todoRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
            Assert.That(results, Is.Not.Null);
            Assert.That(results, Has.Length.EqualTo(2));
            Assert.That(results[0].Id, Is.EqualTo(todoItem.Id));
        }
    }
}