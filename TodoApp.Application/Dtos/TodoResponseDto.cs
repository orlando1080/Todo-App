namespace TodoApp.Application.Dtos;

public sealed record TodoResponseDto(
    Guid Id,
    string Title,
    bool IsCompleted);