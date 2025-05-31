using IssueManager.IssueManager.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IssueManager.IssueManager.Domain.Models;
public class Project
{
    [Key]
    public ProjectId ProjectId { get; set; } = default!;
    public string Name { get; set; } = default!;
    [MaxLength(250)]
    public string Description { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime ChangedAt { get; set; }
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

    public ICollection<Issue> Issues { get; set; } = new List<Issue>();

}