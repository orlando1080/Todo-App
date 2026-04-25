using Moq;
using TodoApp.Application.Dtos;
using ToDoApp.Application.Tasks.Queries;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Tests.TodoTasks.Queries;

[TestFixture]
[TestOf(typeof(GetTaskByIdQueryHandler))]
internal sealed class GetTaskByIdQueryHandlerTests
{
    private readonly Mock<ITaskItemRepository> _todoRepositoryMock = new();

    private GetTaskByIdQueryHandler _sut = null!;


    [SetUp]
    public void SetUp()
    {
        _todoRepositoryMock.Reset();

        _sut = new GetTaskByIdQueryHandler(_todoRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _todoRepositoryMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task HandleAsync_ValidQuery_ReturnsTodoItemDto()
    {
        TaskItem taskItem = TaskItem.Create("test");
        _todoRepositoryMock.Setup(x => x.GetByIdAsync(taskItem.Id)).ReturnsAsync(taskItem);

        TaskItemDto? result = await _sut.HandleAsync(new GetTaskByIdQuery(taskItem.Id), CancellationToken.None).ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo(taskItem.Title));
            Assert.That(result.Id, Is.EqualTo(taskItem.Id));
            Assert.That(result.IsCompleted, Is.EqualTo(taskItem.IsCompleted));
            _todoRepositoryMock.Verify(x => x.GetByIdAsync(taskItem.Id), Times.Once);
        }
    }

    [Test]
    public void HandleAsync_NullQuery_ThrowsArgumentNullException() =>
        Assert.ThrowsAsync<ArgumentNullException>(() => _sut.HandleAsync(null!, CancellationToken.None));

    [Test]
    public async Task HandleAsync_TaskNotFound_ReturnsNull()
    {
        _todoRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((TaskItem?) null);

        TaskItemDto? result = await _sut.HandleAsync(new GetTaskByIdQuery(Guid.NewGuid()), CancellationToken.None).ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Null);
            _todoRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}