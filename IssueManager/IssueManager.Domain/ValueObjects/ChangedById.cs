namespace IssueManager.IssueManager.Domain.ValueObjects;
public record ChangedById
{
    public Guid Value { get; }
    public ChangedById() { }
    private ChangedById(Guid value) => Value = value;
    public static ChangedById Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("Issue cannot be empty.");
        }

        return new ChangedById(value);
    }
}
