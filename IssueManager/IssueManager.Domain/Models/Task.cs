namespace IssueManager.IssueManager.Domain.Models;
public class Task
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User Owner { get; set; } = default!;
}