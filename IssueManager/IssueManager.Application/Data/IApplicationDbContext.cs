using Microsoft.EntityFrameworkCore;

namespace IssueManager.IssueManager.Application.Data;
public interface IApplicationDbContext
{
    DbSet<Issue> Issues { get; }
    DbSet<Project> Projects { get; }
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
