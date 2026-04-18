using System.ComponentModel.DataAnnotations;

namespace TodoApp.Application.Dtos;

public sealed record TodoItemDto(
    [property: Required] Guid Id,
    string Title,
    bool IsCompleted);