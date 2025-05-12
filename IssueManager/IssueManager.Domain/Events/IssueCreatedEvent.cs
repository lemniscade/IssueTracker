using IssueManager.Domain.Abstractions;
using IssueManager.Domain.Models;

namespace IssueManager.Domain.Events;

public record IssueCreatedEvent(Issue issue) : IDomainEvent;
