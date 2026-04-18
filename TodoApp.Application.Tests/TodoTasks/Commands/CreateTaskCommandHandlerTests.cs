using Moq;
using TodoApp.Application.Dtos;
using TodoApp.Application.TodoTasks.Commands;
using TodoApp.Application.TodoTasks.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Events;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Tests.TodoTasks.Commands;

[TestFixture]
[TestOf(typeof(CreateTaskCommandHandler))]
internal sealed class CreateTaskCommandHandlerTests
{
    private readonly Mock<ITodoRepository> _todoRepositoryMock = new();

    private readonly Mock<IMessageBus> _messageBusMock = new();

    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private CreateTaskCommandHandler _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _messageBusMock.Setup(x => x.PublishAsync(It.IsAny<TaskCreatedDomainEvent>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _todoRepositoryMock.Setup(x => x.AddAsync(It.IsAny<TodoItem>()))
            .Returns(Task.CompletedTask);

        _sut = new CreateTaskCommandHandler(
            _todoRepositoryMock.Object,
            _messageBusMock.Object,
            _unitOfWorkMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _todoRepositoryMock.VerifyNoOtherCalls();
        _messageBusMock.VerifyNoOtherCalls();
        _unitOfWorkMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task HandleAsync_ValidCommand_ReturnsTodoItemDto()
    {
        TodoItemDto result = await _sut.HandleAsync(new CreateTaskCommand("test"), CancellationToken.None)
            .ConfigureAwait(false);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Title, Is.EqualTo("test"));
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.IsCompleted, Is.False);

            _todoRepositoryMock.Verify(x => x.AddAsync(It.IsAny<TodoItem>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            _messageBusMock.Verify(x => x.PublishAsync(It.IsAny<TaskCreatedDomainEvent>()), Times.Once);
        }
    }

    [Test]
    public void HandleAsync_InvalidCommand_ThrowsArgumentNullException()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _sut.HandleAsync(null!, CancellationToken.None).ConfigureAwait(false));

            _todoRepositoryMock.Verify(x => x.AddAsync(It.IsAny<TodoItem>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
            _messageBusMock.Verify(x => x.PublishAsync(It.IsAny<TaskCreatedDomainEvent>()), Times.Never);
        }
    }
}