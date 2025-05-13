namespace IssueManager.IssueManager.Domain.Events; 
public record IssueUpdatedEvent(Issue UpdateIssue) : IDomainEvent;
