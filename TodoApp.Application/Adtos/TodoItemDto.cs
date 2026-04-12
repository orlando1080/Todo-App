using System.ComponentModel.DataAnnotations;

namespace TodoApp.Application.Adtos;

public sealed record TodoItemDto(
    [property: Required] Guid Id,
    string Title,
    bool IsCompleted);