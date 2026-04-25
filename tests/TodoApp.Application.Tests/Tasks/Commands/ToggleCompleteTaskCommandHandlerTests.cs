using Moq;
using TodoApp.Application.TodoTasks.Commands;
using TodoApp.Application.TodoTasks.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Tests.TodoTasks.Commands;

[TestFixture]
[TestOf(typeof(ToggleCompleteTaskCommandHandler))]
internal sealed class ToggleCompleteTaskCommandHandlerTests
{
    private readonly Mock<ITaskItemRepository> _todoRepositoryMock = new();

    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly Guid _todoItemId = Guid.NewGuid();

    private ToggleCompleteTaskCommandHandler _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _todoRepositoryMock.Reset();
        _unitOfWorkMock.Reset();

        _todoRepositoryMock.Setup(x => x.GetByIdAsync(_todoItemId)).ReturnsAsync(TaskItem.Create("test"));

        _todoRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _sut = new ToggleCompleteTaskCommandHandler(_todoRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _todoRepositoryMock.VerifyNoOtherCalls();
        _unitOfWorkMock.VerifyNoOtherCalls();
    }


    [Test]
    public async Task HandleAsync_ValidTodoItem_TogglesIsCompleted()
    {
        TaskItem? todoItem = null;

        _todoRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TaskItem>()))
            .Callback<TaskItem>(item => todoItem = item)
            .Returns(Task.CompletedTask);

        await _sut.HandleAsync(new ToggleCompleteTaskCommand(_todoItemId), CancellationToken.None).ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(todoItem, Is.Not.Null);
            Assert.That(todoItem.IsCompleted, Is.True);
            _todoRepositoryMock.Verify(x => x.GetByIdAsync(_todoItemId), Times.Once);
            _todoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TaskItem>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Test]
    public void HandleAsync_NullCommand_ThrowsArgumentNullException() =>
        Assert.ThrowsAsync<ArgumentNullException>(() => _sut.HandleAsync(null!, CancellationToken.None));

    [Test]
    public void HandleAsync_TodoItemNotFound_ThrowsInvalidOperationException()
    {
        _todoRepositoryMock.Setup(x => x.GetByIdAsync(_todoItemId)).ReturnsAsync((TaskItem?) null);

        using (Assert.EnterMultipleScope())
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => _sut.HandleAsync(new ToggleCompleteTaskCommand(_todoItemId), CancellationToken.None));
            _todoRepositoryMock.Verify(x => x.GetByIdAsync(_todoItemId), Times.Once);
        }
    }
}