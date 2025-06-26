using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace IssueTracker.Entity.Models;
public class Issue
{
    [Key]
    public int Id { get; set; } = default!;

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
    public Project Project { get; set; } = default!;
    [JsonIgnore]
    public ICollection<IssueUser> IssueUsers { get; set; }

}