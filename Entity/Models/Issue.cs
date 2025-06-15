using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Entity.Models;
public class Issue
{
    [Key]
    public int Id { get; set; } = default!;

    public int CreatedById { get; set; }

    [ForeignKey(nameof(CreatedById))]
    public User CreatedBy { get; set; } = default!;

    public int AssigneeId { get; set; }

    [ForeignKey(nameof(AssigneeId))]
    public User Assignee { get; set; } = default!;

    public int? ChangedById { get; set; }

    [ForeignKey(nameof(ChangedById))]
    public User? ChangedBy { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public int TypeEnumId { get; set; } = default!;

    public int StatusEnumId { get; set; } = 1;

    public int PriorityEnumId { get; set; } = 1;

    public int Effort { get; set; }

    public new DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime AssignedAt { get; set; }
    public int ProjectId { get; set; } = default!;
    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = default!;

    public static Issue Create(string issueTitle, string issueDescription, int type, int? statusId, int? priority, User? assiggnee, User createdBy, int effort, Project project)
    {
        var issue = new Issue
        {
            Title = issueTitle,
            Description = issueDescription,
            TypeEnumId = type,
            StatusEnumId = (int)statusId,
            PriorityEnumId = (int)priority,
            CreatedBy = createdBy,
            Effort = effort,
            CreatedAt = DateTime.Now,
            AssignedAt = DateTime.Now,
            Project = project
        };
        return issue;
    }

    public void Update(string issueTitle, string issueDescription, int type, int? statusId, int? priority, int effort, User changedBy, Project project)
    {
        Title = issueTitle;
        Description = issueDescription;
        TypeEnumId = type;
        StatusEnumId = (int)statusId;
        PriorityEnumId = (int)priority;
        Effort = effort;
        UpdatedAt = DateTime.Now;
        ChangedBy = changedBy;
        Project = project;
    }

}