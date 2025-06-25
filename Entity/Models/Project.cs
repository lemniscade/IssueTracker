using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IssueTracker.Entity.Models;
public class Project
{
    [Key]
    public int Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime ChangedAt { get; set; }
    public ICollection<ProjectUser> ProjectUsers { get; set; }
    public ICollection<Issue> Issues { get; set; } = new List<Issue>();
}