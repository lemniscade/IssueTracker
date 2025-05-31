using IssueManager.IssueManager.Domain.Abstractions;
using IssueManager.IssueManager.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace IssueManager.IssueManager.Domain.Models;

public class User : Entity<UserId>
{
    [Key]
    public UserId UserId { get; set; } = default!;
    [MaxLength(25)]
    public string Username { get; set; } = default!;
    public new string CreatedBy { get; set; } = default!;
    public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();

    public ICollection<Issue> CreatedIssues { get; set; } = new List<Issue>();
    public ICollection<Issue> AssignedIssues { get; set; } = new List<Issue>();
    public ICollection<Issue> ChangedIssues { get; set; } = new List<Issue>();


    public static User Create(UserId id, string name, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        var user = new User
        {
            Id = id,
            Username = name
        };

        return user;
    }
}