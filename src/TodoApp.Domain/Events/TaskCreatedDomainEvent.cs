namespace TodoApp.Domain.Events;

public sealed record TaskCreatedDomainEvent(Guid Id, string Title) : IDomainEvent;
