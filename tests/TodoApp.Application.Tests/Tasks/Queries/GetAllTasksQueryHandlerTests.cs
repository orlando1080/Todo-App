using Moq;
using TodoApp.Application.Dtos;
using ToDoApp.Application.Tasks.Queries;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Tests.TodoTasks.Queries;

[TestFixture]
[TestOf(typeof(GetAllTasksQueryHandler))]
internal sealed class GetAllTasksQueryHandlerTests
{
    private readonly Mock<ITaskItemRepository> _todoRepositoryMock = new();

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
        TaskItem taskItem1 = TaskItem.Create("test");
        TaskItem taskItem2 = TaskItem.Create("test2");

        _todoRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync([taskItem1, taskItem2]);

        TaskItemDto[] result = await _sut.HandleAsync(new GetAllTasksQuery(), CancellationToken.None).ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Length.EqualTo(2));
            Assert.That(result[0].Title, Is.EqualTo(taskItem1.Title));
            Assert.That(result[1].Title, Is.EqualTo(taskItem2.Title));
            _todoRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }
    }

    [Test]
    public async Task HandleAsync_EmptyRepository_ReturnsEmptyArray()
    {
        _todoRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(Array.Empty<TaskItem>());

        TaskItemDto[] result = await _sut.HandleAsync(new GetAllTasksQuery(), CancellationToken.None).ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Empty);
            _todoRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }
    }
}