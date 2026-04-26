using Moq;
using ToDoApp.Application.Tasks.Commands;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Tests.Tasks.Commands;

[TestFixture]
[TestOf(typeof(DeleteTaskCommandHandler))]
internal sealed class DeleteTaskCommandHandlerTests
{
    private readonly Mock<ITaskItemRepository> _taskItemRepositoryMock = new();

    private DeleteTaskCommandHandler _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _taskItemRepositoryMock.Reset();

        _taskItemRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _sut = new DeleteTaskCommandHandler(_taskItemRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _taskItemRepositoryMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task HandleAsync_ValidCommand_DeletesTaskItem()
    {
        Guid id = Guid.NewGuid();

        await _sut.HandleAsync(new DeleteTaskCommand(id), CancellationToken.None).ConfigureAwait(false);

        _taskItemRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
    }

    [Test]
    public void HandleAsync_InvalidCommand_ThrowsArgumentNullException()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _sut.HandleAsync(null!, CancellationToken.None));
            _taskItemRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}