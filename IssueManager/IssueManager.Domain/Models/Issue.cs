using IssueManager.IssueManager.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IssueManager.IssueManager.Domain.Models;
public class Issue : Aggregate<IssueId>
{
    [Key]
    public IssueId IssueId { get; set; } = default!;
    [Required]
    public UserId CreatedById { get; set; }

    [ForeignKey(nameof(CreatedById))]
    public User CreatedBy { get; set; } = default!;

    [Required]
    public UserId AssigneeId { get; set; }

    [ForeignKey(nameof(AssigneeId))]
    public User Assignee { get; set; } = default!;

    public UserId? ChangedById { get; set; }

    [ForeignKey(nameof(ChangedById))]
    public User? ChangedBy { get; set; }

    [Required]
    public string Title { get; set; } = default!;

    [Required]
    public string Description { get; set; } = default!;

    [Required]
    public string Type { get; set; } = default!;

    public int StatusEnumId { get; set; } = 1;

    public int PriorityEnumId { get; set; } = 1;

    public DateTime? DueDate { get; set; }

    public new DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    public ProjectId ProjectId { get; set; } = default!;
    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = default!;

    public static Issue Create(IssueId id, string issueTitle, string issueDescription, string type, int statusId, int priority, User createdBy, DateTime dueDate, DateTime createdAt, DateTime updatedAt, Project project)
    {
        var issue = new Issue
        {
            Id = id,
            Title = issueTitle,
            Description = issueDescription,
            Type = type,
            StatusEnumId = statusId,
            PriorityEnumId = priority,
            CreatedBy = createdBy,
            DueDate = dueDate,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            Project = project
        };
        return issue;
    }

    public void Update(string issueTitle, string issueDescription, string type, int statusId, int priority, DateTime dueDate, DateTime updatedAt, Task project)
    {
        Title = issueTitle;
        Description = issueDescription;
        Type = type;
        StatusEnumId = statusId;
        PriorityEnumId = priority;
        DueDate = dueDate;
        UpdatedAt = updatedAt;
        Project = project;
    }

}