using Moq;
using ToDoApp.Application.Tasks.Commands;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Tests.TodoTasks.Commands;

[TestFixture]
[TestOf(typeof(DeleteTaskCommandHandler))]
internal sealed class DeleteTaskCommandHandlerTests
{
    private readonly Mock<ITaskItemRepository> _todoRepositoryMock = new();

    private DeleteTaskCommandHandler _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _todoRepositoryMock.Reset();

        _todoRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        _sut = new DeleteTaskCommandHandler(_todoRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _todoRepositoryMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task HandleAsync_ValidCommand_DeletesTodoItem()
    {
        Guid id = Guid.NewGuid();

        await _sut.HandleAsync(new DeleteTaskCommand(id), CancellationToken.None).ConfigureAwait(false);

        _todoRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
    }

    [Test]
    public void HandleAsync_InvalidCommand_ThrowsArgumentNullException()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _sut.HandleAsync(null!, CancellationToken.None));
            _todoRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}