using IssueManager.Domain.Abstractions;
using IssueManager.Domain.ValueObjects;

namespace IssueManager.Domain.Models;
public class Issue:Aggregate<IssueId>
{
    public new int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Type { get; set; } = default!;
    public int StatusId { get; set; }
    public string Priority { get; set; } = default!;
    public int ProjectId { get; set; }
    public new int CreatedBy { get; set; }
    public int? AssignedTo { get; set; }
    public DateTime? DueDate { get; set; }
    public new DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public int Status { get; set; } = default!;
    public User Creator { get; set; } = default!;
    public User Assignee { get; set; } = default!;
    public Task Project { get; set; } = default!;
}