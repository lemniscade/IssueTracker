using IssueManager.IssueManager.Domain.Abstractions;
using IssueManager.IssueManager.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace IssueManager.IssueManager.Domain.Models;

public class User : Entity<UserId>
{
    [Key]
    public UserId UserId { get; set; } = default!;
    [MaxLength(10)]
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();

    public ICollection<Issue> CreatedIssues { get; set; } = new List<Issue>();
    public ICollection<Issue> AssignedIssues { get; set; } = new List<Issue>();
    public ICollection<Issue> ChangedIssues { get; set; } = new List<Issue>();

}