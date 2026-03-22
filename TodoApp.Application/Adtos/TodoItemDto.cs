using System.ComponentModel.DataAnnotations;

namespace TodoApp.Application.Adtos;

public record TodoItemDto(
    [property: Required] Guid Id,
    string Title,
    bool IsCompleted);