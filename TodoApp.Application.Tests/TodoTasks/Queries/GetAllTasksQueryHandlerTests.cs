using Moq;
using TodoApp.Application.Dtos;
using TodoApp.Application.TodoTasks.Queries;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Tests.TodoTasks.Queries;

[TestFixture]
[TestOf(typeof(GetAllTasksQueryHandler))]
internal sealed class GetAllTasksQueryHandlerTests
{
    private readonly Mock<ITodoRepository> _todoRepositoryMock = new();

    private GetAllTasksQueryHandler _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _todoRepositoryMock.Reset();

        _sut = new GetAllTasksQueryHandler(_todoRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _todoRepositoryMock.VerifyNoOtherCalls();
    }


    [Test]
    public async Task HandleAsync_ValidQuery_ReturnsTodoItemDtos()
    {
        TodoItem todoItem1 = TodoItem.Create("test");
        TodoItem todoItem2 = TodoItem.Create("test2");

        _todoRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync([todoItem1, todoItem2]);

        TodoItemDto[] result = await _sut.HandleAsync(new GetAllTasksQuery(), CancellationToken.None).ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Length.EqualTo(2));
            Assert.That(result[0].Title, Is.EqualTo(todoItem1.Title));
            Assert.That(result[1].Title, Is.EqualTo(todoItem2.Title));
            _todoRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }
    }

    [Test]
    public async Task HandleAsync_EmptyRepository_ReturnsEmptyArray()
    {
        _todoRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(Array.Empty<TodoItem>());

        TodoItemDto[] result = await _sut.HandleAsync(new GetAllTasksQuery(), CancellationToken.None).ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Empty);
            _todoRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }
    }
}