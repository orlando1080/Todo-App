using Moq;
using TodoApp.Application.Dtos;
using ToDoApp.Application.Tasks.Queries;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Tests.Tasks.Queries;

[TestFixture]
[TestOf(typeof(GetAllTasksQueryHandler))]
internal sealed class GetAllTasksQueryHandlerTests
{
    private readonly Mock<ITaskItemRepository> _taskItemRepositoryMock = new();

    private GetAllTasksQueryHandler _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _taskItemRepositoryMock.Reset();

        _sut = new GetAllTasksQueryHandler(_taskItemRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _taskItemRepositoryMock.VerifyNoOtherCalls();
    }


    [Test]
    public async Task HandleAsync_ValidQuery_ReturnsTaskItemDtos()
    {
        TaskItem taskItem1 = TaskItem.Create("test");
        TaskItem taskItem2 = TaskItem.Create("test2");

        _taskItemRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync([taskItem1, taskItem2]);

        TaskItemDto[] result = await _sut.HandleAsync(new GetAllTasksQuery(), CancellationToken.None).ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Length.EqualTo(2));
            Assert.That(result[0].Title, Is.EqualTo(taskItem1.Title));
            Assert.That(result[1].Title, Is.EqualTo(taskItem2.Title));
            _taskItemRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }
    }

    [Test]
    public async Task HandleAsync_EmptyRepository_ReturnsEmptyArray()
    {
        _taskItemRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(Array.Empty<TaskItem>());

        TaskItemDto[] result = await _sut.HandleAsync(new GetAllTasksQuery(), CancellationToken.None).ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Empty);
            _taskItemRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }
    }
}