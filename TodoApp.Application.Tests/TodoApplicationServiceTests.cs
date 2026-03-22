using Microsoft.Extensions.Logging;
using Moq;
using TodoApp.Application.Adtos;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Tests;

[TestFixture]
internal class TodoApplicationServiceTests
{
    private Mock<ITodoRepository> _todoRepositoryMock;

    private Mock<ILogger<TodoApplicationService>> _loggerMock;

    private TodoApplicationService _sut;

    [SetUp]
    public void Setup()
    {
        _todoRepositoryMock = new Mock<ITodoRepository>();
        _loggerMock = new Mock<ILogger<TodoApplicationService>>();

        _loggerMock.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        _loggerMock.Setup(x => x.BeginScope(It.IsAny<object>())).Returns(Mock.Of<IDisposable>());

        _sut = new TodoApplicationService(
            _todoRepositoryMock.Object,
            _loggerMock.Object);
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

    [Test]
    public async Task AddTaskAsync_ReturnsTodoItemDto()
    {
        CreateTodoDto createTodoDto = new("test");
        _todoRepositoryMock.Setup(x => x.AddAsync(It.IsAny<TodoItem>()));

        TodoItemDto result = await _sut.AddTaskAsync(createTodoDto).ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            _todoRepositoryMock.Verify(x => x.AddAsync(It.IsAny<TodoItem>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo(createTodoDto.Title));
        }
    }

    [Test]
    public void AddTaskAsync_ThrowsArgumentException_WhenTitleIsEmpty()
    {
        CreateTodoDto createTodoDto = new("");
        Assert.ThrowsAsync<ArgumentException>(() => _sut.AddTaskAsync(createTodoDto));
    }

    [Test]
    public void AddTaskAsync_ThrowsException_WhenDatabaseErrorOccurs()
    {
        CreateTodoDto createTodoDto = new("test");
        _todoRepositoryMock.Setup(x => x.AddAsync(It.IsAny<TodoItem>())).ThrowsAsync(new Exception());

        Assert.ThrowsAsync<Exception>(() => _sut.AddTaskAsync(createTodoDto));
    }
}