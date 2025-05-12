namespace IssueManager.Domain.Abstractions;
public interface IDomainEvent
{
    Guid EventId => Guid.NewGuid();
    public DateTime OccurredOn => DateTime.Now;
}
