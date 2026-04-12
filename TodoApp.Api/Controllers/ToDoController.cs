using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Adtos;
using TodoApp.Application.Services;
using TodoApp.Application.TodoTasks.Commands;

namespace TodoApp.Api.Controllers;

[ApiController, Route("api/[Controller]")] // This makes the URL: api/todo
public class TodoController : ControllerBase
{
    private readonly ITodoApplicationService _todoApplicationService;
    private readonly CreateTaskCommandHandler _createTaskCommandHandler;

    public TodoController(ITodoApplicationService todoApplicationService, CreateTaskCommandHandler createTaskCommandHandler)
    {
        _todoApplicationService = todoApplicationService;
        _createTaskCommandHandler = createTaskCommandHandler;
    }

    [HttpGet]
    public async Task<ActionResult<TodoItemDto[]>> GetAll()
    {
        TodoItemDto[] todoItemDtos = await _todoApplicationService.GetAllAsync().ConfigureAwait(false);

        return Ok(todoItemDtos); // Use Ok() to ensure a 200 status with the body
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

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] Guid id)
    {
        await _todoApplicationService.DeleteTaskAsync(id).ConfigureAwait(false);

        return NoContent();
    }
}