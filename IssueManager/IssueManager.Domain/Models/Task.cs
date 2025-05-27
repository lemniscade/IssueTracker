using IssueManager.IssueManager.Domain.ValueObjects;

namespace IssueManager.IssueManager.Domain.Models;
public class Task
{
    public ProjectId ProjectId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public OwnerId OwnerId { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public User Owner { get; set; } = default!;
}