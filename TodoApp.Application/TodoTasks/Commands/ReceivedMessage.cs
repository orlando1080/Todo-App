namespace TodoApp.Application.TodoTasks.Commands;

public record ReceivedMessage(
    string MessageId,
    string PopReceipt,
    string MessageBody
);
