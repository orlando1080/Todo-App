namespace TodoApp.Application.TodoTasks.Commands;

public sealed record ReceivedMessage(
    string MessageId,
    string PopReceipt,
    string MessageBody
);
