using IssueManager.IssueManager.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IssueManager.IssueManager.Domain.Models;
public class Issue : Aggregate<IssueId>
{
    [Key]
    public IssueId IssueId { get; set; } = default!;

    public UserId CreatedById { get; set; }

    [ForeignKey(nameof(CreatedById))]
    public User CreatedBy { get; set; } = default!;

    public UserId AssigneeId { get; set; }

    [ForeignKey(nameof(AssigneeId))]
    public User Assignee { get; set; } = default!;

    public UserId? ChangedById { get; set; }

    [ForeignKey(nameof(ChangedById))]
    public User? ChangedBy { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Type { get; set; } = default!;

    public int StatusEnumId { get; set; } = 1;

    public int PriorityEnumId { get; set; } = 1;

    public DateTime? DueDate { get; set; }

    public new DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    public ProjectId ProjectId { get; set; } = default!;
    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = default!;

    public static Issue Create(string issueTitle, string issueDescription, string type, int? statusId, int? priority, User createdBy, DateTime? dueDate, DateTime? createdAt, Project project)
    {
        var issue = new Issue
        {
            Title = issueTitle,
            Description = issueDescription,
            Type = type,
            StatusEnumId = (int)statusId,
            PriorityEnumId = (int)priority,
            CreatedBy = createdBy,
            DueDate = dueDate,
            CreatedAt = (DateTime)createdAt,
            Project = project
        };
        return issue;
    }

    public void Update(IssueId issueId,string issueTitle, string issueDescription, string type, int? statusId, int? priority, DateTime? dueDate,User changedBy, DateTime? changedAt, Project project)
    {
        IssueId=issueId;
        Title = issueTitle;
        Description = issueDescription;
        Type = type;
        StatusEnumId = (int)statusId;
        PriorityEnumId = (int)priority;
        DueDate = dueDate;
        UpdatedAt = (DateTime)changedAt;
        ChangedBy= changedBy;
        Project = project;
    }

}