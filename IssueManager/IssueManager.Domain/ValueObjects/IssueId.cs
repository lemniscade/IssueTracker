namespace IssueManager.IssueManager.Domain.ValueObjects;
public record IssueId
{
    public Guid Value { get; }
    public IssueId() { }
    public IssueId(Guid value) => Value = value;
    public static IssueId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("Issue cannot be empty.");
        }

        return new IssueId(value);
    }
}
