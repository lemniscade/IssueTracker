using Microsoft.EntityFrameworkCore;

namespace IssueManager.IssueManager.Application.Data;
public interface IApplicationDbContext
{
    DbSet<Comment> Comment { get; }
    DbSet<Issue> Issue { get; }
    DbSet<IssueHistory> IssueHistory { get; }
    DbSet<IssueStatus> IssueStatus { get; }

    DbSet<Role> Role { get; }
    DbSet<Task> Task { get; }
    DbSet<User> User { get; }
    DbSet<UserRoles> UserRole { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
