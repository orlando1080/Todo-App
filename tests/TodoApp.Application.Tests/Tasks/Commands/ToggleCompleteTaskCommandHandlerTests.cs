using Moq;
using ToDoApp.Application.Tasks.Commands;
using ToDoApp.Application.Tasks.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Tests.Tasks.Commands;

[TestFixture]
[TestOf(typeof(ToggleCompleteTaskCommandHandler))]
internal sealed class ToggleCompleteTaskCommandHandlerTests
{
    private readonly Mock<ITaskItemRepository> _taskItemRepositoryMock = new();

    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly Guid _todoItemId = Guid.NewGuid();

    private ToggleCompleteTaskCommandHandler _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _taskItemRepositoryMock.Reset();
        _unitOfWorkMock.Reset();

        _taskItemRepositoryMock.Setup(x => x.GetByIdAsync(_todoItemId)).ReturnsAsync(TaskItem.Create("test"));

        _taskItemRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _sut = new ToggleCompleteTaskCommandHandler(_taskItemRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _taskItemRepositoryMock.VerifyNoOtherCalls();
        _unitOfWorkMock.VerifyNoOtherCalls();
    }


    [Test]
    public async Task HandleAsync_ValidTodoItem_TogglesIsCompleted()
    {
        TaskItem? taskItem = null;

        _taskItemRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TaskItem>()))
            .Callback<TaskItem>(item => taskItem = item)
            .Returns(Task.CompletedTask);

        await _sut.HandleAsync(new ToggleCompleteTaskCommand(_todoItemId), CancellationToken.None).ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(taskItem, Is.Not.Null);
            Assert.That(taskItem.IsCompleted, Is.True);
            _taskItemRepositoryMock.Verify(x => x.GetByIdAsync(_todoItemId), Times.Once);
            _taskItemRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TaskItem>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Test]
    public void HandleAsync_NullCommand_ThrowsArgumentNullException() =>
        Assert.ThrowsAsync<ArgumentNullException>(() => _sut.HandleAsync(null!, CancellationToken.None));

    [Test]
    public void HandleAsync_TodoItemNotFound_ThrowsInvalidOperationException()
    {
        _taskItemRepositoryMock.Setup(x => x.GetByIdAsync(_todoItemId)).ReturnsAsync((TaskItem?) null);

        using (Assert.EnterMultipleScope())
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => _sut.HandleAsync(new ToggleCompleteTaskCommand(_todoItemId), CancellationToken.None));
            _taskItemRepositoryMock.Verify(x => x.GetByIdAsync(_todoItemId), Times.Once);
        }
    }
}