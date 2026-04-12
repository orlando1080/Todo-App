namespace TodoApp.Application.Adtos;

public sealed record TodoResponseDto(
    Guid Id,
    string Title,
    bool IsCompleted);