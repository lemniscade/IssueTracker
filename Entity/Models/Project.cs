using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Entity.Models;
public class Project
{
    [Key]
    public int Id { get; set; } = default!;
    [MaxLength(10)]
    public string Title { get; set; } = default!;
    [MaxLength(250)]
    public string Description { get; set; } = default!;
    public DateTime AssignedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ChangedAt { get; set; }
    public int CreatedById { get; set; }

    [ForeignKey(nameof(CreatedById))]
    public User CreatedBy { get; set; } = default!;

    public int AssigneeId { get; set; }

    [ForeignKey(nameof(AssigneeId))]
    public User Assignee { get; set; } = default!;

    public int? ChangedById { get; set; }

    [ForeignKey(nameof(ChangedById))]
    public User? ChangedBy { get; set; }

    public ICollection<Issue> Issues { get; set; } = new List<Issue>();

    public static Project Create(string projectTitle, string projectDescription, string username, DateTime? createdAt, string? assiggnee, DateTime? assignedAt)
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        User user = _context.Users.FirstOrDefault(x => x.Username == username);
        User assignedUser = _context.Users.FirstOrDefault(x => x.Username == assiggnee);
        var project = new Project
        {
            Title = projectTitle,
            Description = projectDescription,
            CreatedBy = user,
            CreatedAt = (DateTime)createdAt,
            Assignee = assignedUser,
            AssignedAt = (DateTime)assignedAt
        };
        _context.Projects.Add(project);
        _context.SaveChanges();
        return project;
    }

    public void Update(int id, string projectTitle, string projectDescription, User changedBy, DateTime? changedAt, User? assiggnee, DateTime? assignedAt)
    {
        using (var dbContext = new ApplicationDbContext())
        {
            Project? project = dbContext.Projects.FirstOrDefault(p => p.Id == id);
            if (project != null)
            {
                project.Title = projectTitle;
                project.Description = projectDescription;
                project.ChangedBy = changedBy;
                project.ChangedAt = (DateTime)changedAt;
                Assignee = assiggnee;
                AssignedAt = (DateTime)assignedAt;
            }
            dbContext.SaveChanges();
        }
    }
}