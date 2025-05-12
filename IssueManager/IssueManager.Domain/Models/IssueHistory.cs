namespace IssueManager.Domain.Models;
public class IssueHistory
{
    public int Id { get; set; }
    public int IssueId { get; set; }
    public string FieldName { get; set; } = default!;
    public string OldValue { get; set; } = default!;
    public string NewValue { get; set; } = default!;
    public int ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; }

    public Issue Issue { get; set; } = default!;
    public User ChangedByUser { get; set; } = default!;
}