using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Entity.Models;

public class User
{
    [Key]
    public int Id { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
    public ICollection<IssueUser> IssueUsers { get; set; }

}