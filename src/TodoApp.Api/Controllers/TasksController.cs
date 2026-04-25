using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Dtos;
using ToDoApp.Application.Tasks.Commands;
using ToDoApp.Application.Tasks.Queries;

namespace TodoApp.Api.Controllers;

[ApiController, Route("api/[Controller]")] // This makes the URL: api/todo
public sealed class TasksController : ControllerBase
{
    private readonly CreateTaskCommandHandler _createTaskCommandHandler;
    private readonly GetAllTasksQueryHandler _getAllTasksQueryHandler;
    private readonly DeleteTaskCommandHandler _deleteTaskCommandHandler;
    private readonly GetTaskByIdQueryHandler _getTaskByIdQueryHandler;
    private readonly ToggleCompleteTaskCommandHandler _toggleCompleteTaskCommandHandler;

    public TasksController(
        CreateTaskCommandHandler createTaskCommandHandler,
        GetAllTasksQueryHandler getAllTasksQueryHandler,
        DeleteTaskCommandHandler deleteTaskCommandHandler,
        GetTaskByIdQueryHandler getTaskByIdQueryHandler,
        ToggleCompleteTaskCommandHandler toggleCompleteTaskCommandHandler)
    {
        _createTaskCommandHandler = createTaskCommandHandler;
        _getAllTasksQueryHandler = getAllTasksQueryHandler;
        _deleteTaskCommandHandler = deleteTaskCommandHandler;
        _getTaskByIdQueryHandler = getTaskByIdQueryHandler;
        _toggleCompleteTaskCommandHandler = toggleCompleteTaskCommandHandler;
    }

    [HttpGet]
    public async Task<ActionResult<TaskItemDto[]>> GetAll(CancellationToken cancellationToken)
    {
        TaskItemDto[] todoItemDtos = await _getAllTasksQueryHandler.HandleAsync(new GetAllTasksQuery(), cancellationToken).ConfigureAwait(false);

        return Ok(todoItemDtos); // Use Ok() to ensure a 200 status with the body
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        TaskItemDto? taskItemDto = await _getTaskByIdQueryHandler.HandleAsync(new GetTaskByIdQuery(id), cancellationToken).ConfigureAwait(false);

        return taskItemDto is not null ? Ok(taskItemDto) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<TaskItemDto>> Create([FromBody] string title, CancellationToken cancellationToken)
    {
        try
        {
            CreateTaskCommand command = new(title);

            TaskItemDto taskItemDto = await _createTaskCommandHandler.HandleAsync(command, cancellationToken).ConfigureAwait(false);

            return CreatedAtAction(nameof(Create), taskItemDto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> ToggleCompletion([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _toggleCompleteTaskCommandHandler.HandleAsync(new ToggleCompleteTaskCommand(id), cancellationToken).ConfigureAwait(false);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _deleteTaskCommandHandler.HandleAsync(new DeleteTaskCommand(id), cancellationToken).ConfigureAwait(false);

        return NoContent();
    }
}