namespace TodoApp.Domain.Events;

public record TaskCreatedDomainEvent(Guid Id, string Title) : IDomainEvent;
