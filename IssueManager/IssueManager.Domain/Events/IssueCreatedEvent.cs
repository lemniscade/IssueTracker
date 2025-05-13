

namespace IssueManager.IssueManager.Domain.Events; 
public record IssueCreatedEvent(Issue CreateIssue) : IDomainEvent;
