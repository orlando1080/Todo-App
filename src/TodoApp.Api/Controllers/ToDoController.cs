using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Dtos;
using TodoApp.Application.TodoTasks.Commands;
using TodoApp.Application.TodoTasks.Queries;

namespace TodoApp.Api.Controllers;

[ApiController, Route("api/[Controller]")] // This makes the URL: api/todo
public sealed class TodoController : ControllerBase
{
    private readonly CreateTaskCommandHandler _createTaskCommandHandler;
    private readonly GetAllTasksQueryHandler _getAllTasksQueryHandler;
    private readonly DeleteTaskCommandHandler _deleteTaskCommandHandler;
    private readonly GetTaskByIdQueryHandler _getTaskByIdQueryHandler;

    public TodoController(
        CreateTaskCommandHandler createTaskCommandHandler,
        GetAllTasksQueryHandler getAllTasksQueryHandler,
        DeleteTaskCommandHandler deleteTaskCommandHandler,
        GetTaskByIdQueryHandler getTaskByIdQueryHandler)
    {
        _createTaskCommandHandler = createTaskCommandHandler;
        _getAllTasksQueryHandler = getAllTasksQueryHandler;
        _deleteTaskCommandHandler = deleteTaskCommandHandler;
        _getTaskByIdQueryHandler = getTaskByIdQueryHandler;
    }

    [HttpGet]
    public async Task<ActionResult<TodoItemDto[]>> GetAll()
    {
        TodoItemDto[] todoItemDtos = await _getAllTasksQueryHandler.HandleAsync(new GetAllTasksQuery(), CancellationToken.None).ConfigureAwait(false);

        return Ok(todoItemDtos); // Use Ok() to ensure a 200 status with the body
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDto>> GetById([FromRoute] Guid id)
    {
        TodoItemDto? todoItemDto = await _getTaskByIdQueryHandler.HandleAsync(new GetTaskByIdQuery(id), CancellationToken.None).ConfigureAwait(false);

        return todoItemDto is not null ? Ok(todoItemDto) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<TodoItemDto>> Create([FromBody] string title)
    {
        try
        {
            CreateTaskCommand command = new(title);

            TodoItemDto todoItemDto = await _createTaskCommandHandler.HandleAsync(command, CancellationToken.None).ConfigureAwait(false);

            return CreatedAtAction(nameof(Create), todoItemDto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _deleteTaskCommandHandler.HandleAsync(new DeleteTaskCommand(id), CancellationToken.None).ConfigureAwait(false);

        return NoContent();
    }
}