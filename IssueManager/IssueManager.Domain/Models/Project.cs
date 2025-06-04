using IssueManager.IssueManager.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using IssueManager.IssueManager.Domain.Enums;
using Microsoft.VisualBasic;
using IssueManager.IssueManager.Infrastructure.Data;

namespace IssueManager.IssueManager.Domain.Models;
public class Project
{
    [Key]
    public ProjectId ProjectId { get; set; } = default!;
    [MaxLength(10)]
    public string Title { get; set; } = default!;
    [MaxLength(250)]
    public string Description { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime ChangedAt { get; set; }
    public UserId CreatedById { get; set; }

    [ForeignKey(nameof(CreatedById))]
    public User CreatedBy { get; set; } = default!;

    public UserId AssigneeId { get; set; }

    [ForeignKey(nameof(AssigneeId))]
    public User Assignee { get; set; } = default!;

    public UserId? ChangedById { get; set; }

    [ForeignKey(nameof(ChangedById))]
    public User? ChangedBy { get; set; }

    public ICollection<Issue> Issues { get; set; } = new List<Issue>();

    public static Project Create(string projectTitle, string projectDescription, int? statusId, int? priority, User createdBy, DateTime? createdAt)
    {
        var project = new Project
        {
            Title = projectTitle,
            Description = projectDescription,
            CreatedBy = createdBy,
            CreatedAt = (DateTime)createdAt,
        };
        return project;
    }

    public void Update(ProjectId id, string projectTitle, string projectDescription, User changedBy, DateTime? changedAt)
    {
        using (var dbContext = new ApplicationDbContext())
        {
            Project? project = dbContext.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (project != null)
            {
                project.Title = projectTitle;
                project.Description = projectDescription;
                project.ChangedBy = changedBy;
                project.ChangedAt = (DateTime)changedAt;
            }
        }
    }
}