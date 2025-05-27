namespace IssueManager.IssueManager.Domain.ValueObjects;
public record OwnerId
{
    public Guid Value { get; }
    private OwnerId(Guid value) => Value = value;
    public static OwnerId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("Issue cannot be empty.");
        }

        return new OwnerId(value);
    }
}
