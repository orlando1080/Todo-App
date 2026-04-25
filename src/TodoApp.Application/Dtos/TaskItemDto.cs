using System.ComponentModel.DataAnnotations;

namespace TodoApp.Application.Dtos;

public sealed record TaskItemDto(
    [property: Required] Guid Id,
    string Title,
    bool IsCompleted);