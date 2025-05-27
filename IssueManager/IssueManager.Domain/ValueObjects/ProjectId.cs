namespace IssueManager.IssueManager.Domain.ValueObjects
{
    public record ProjectId
    {
        public Guid Value { get; }
        private ProjectId(Guid value) => Value = value;
        public static ProjectId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("User cannot be empty.");
            }

            return new ProjectId(value);
        }
    }
}
