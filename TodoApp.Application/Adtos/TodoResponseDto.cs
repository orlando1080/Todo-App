namespace TodoApp.Application.Adtos;

public record TodoResponseDto(
    Guid Id,
    string Title,
    bool IsCompleted);