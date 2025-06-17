using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Entity.Models;

public class User
{
    [Key]
    public int Id { get; set; } = default!;
    [MaxLength(10)]
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();

    public ICollection<Issue> CreatedIssues { get; set; } = new List<Issue>();
    public ICollection<Issue> AssignedIssues { get; set; } = new List<Issue>();
    public ICollection<Issue> ChangedIssues { get; set; } = new List<Issue>();

}