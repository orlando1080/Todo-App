using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Adtos;
using TodoApp.Application.Services;
using TodoApp.Application.TodoTasks.Commands;

namespace TodoApp.Api.Controllers;

[ApiController, Route("api/[Controller]")] // This makes the URL: api/todo
public class TodoController : ControllerBase
{
    private readonly ITodoApplicationService _todoApplicationService;
    private readonly CreateTodoTaskCommandHandler _createTodoTaskCommandHandler;

    public TodoController(ITodoApplicationService todoApplicationService, CreateTodoTaskCommandHandler createTodoTaskCommandHandler)
    {
        _todoApplicationService = todoApplicationService;
        _createTodoTaskCommandHandler = createTodoTaskCommandHandler;
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
            TodoItemDto todoItemDto = await  _todoApplicationService.AddTaskAsync(new CreateTodoDto(title)).ConfigureAwait(false);

            AddTaskCommand command = new(title);

            await _createTodoTaskCommandHandler.HandleAsync(command, CancellationToken.None).ConfigureAwait(false);

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