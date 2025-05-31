using IssueManager.IssueManager.Application.Data;
using IssueManager.IssueManager.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.IssueManager.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<Issue> Issues => Set<Issue>();

        public DbSet<Project> Projects => Set<Task>();

        public DbSet<User> Users => Set<User>();
        private readonly IConfiguration _configuration;

        public ApplicationDbContext()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var projectIdConverterforProject = new ValueConverter<ProjectId, Guid>(
    id => id.Value,
    value => new ProjectId(value)
);

            modelBuilder.Entity<Project>()
                .Property(p => p.ProjectId)
                .HasConversion(projectIdConverterforProject);

            modelBuilder.Entity<Project>()
                .HasKey(p => p.ProjectId);

            modelBuilder.Ignore<ProjectId>();





            var uerIdConverter = new ValueConverter<UserId, Guid>(
    id => id.Value,
    value => new UserId(value)
);

            modelBuilder.Entity<User>()
                .Property(p => p.UserId)
                .HasConversion(uerIdConverter);

            modelBuilder.Entity<User>()
                .HasKey(p => p.UserId);

            modelBuilder.Ignore<UserId>();









            modelBuilder.Entity<User>()
            .Property(e => e.UserId)
            .HasConversion(
                v => v.Value,
                v => new UserId(v)
            );

            modelBuilder.Entity<User>()
        .HasKey(p => p.UserId);











            // Value Object: IssueId -> Guid
            var issueIdConverter = new ValueConverter<IssueId, Guid>(
                v => v.Value,
                v => new IssueId(v)
            );

            modelBuilder.Entity<Issue>()
                .Property(i => i.IssueId)
                .HasConversion(issueIdConverter);

            modelBuilder.Entity<Issue>()
                .HasKey(i => i.IssueId);

            // ProjectId için de aynısını yap
            var projectIdConverter = new ValueConverter<ProjectId, Guid>(
                v => v.Value,
                v => new ProjectId(v)
            );

            modelBuilder.Entity<Issue>()
                .Property(i => i.ProjectId)
                .HasConversion(projectIdConverter);

            // İlişkiler (gerekirse)
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Project)
                .WithMany(p => p.Issues)
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // İlişkiler için kullanıcılar
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.CreatedBy)
                .WithMany()
                .HasForeignKey(i => i.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Assignee)
                .WithMany()
                .HasForeignKey(i => i.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Issue>()
                .HasOne(i => i.ChangedBy)
                .WithMany()
                .HasForeignKey(i => i.ChangedById)
                .OnDelete(DeleteBehavior.Restrict);

            // EF'nin IssueId sınıfını entity sanmaması için IGNORE et
            modelBuilder.Ignore<IssueId>();
            modelBuilder.Ignore<ProjectId>();

            modelBuilder.Entity<Issue>()
            .HasOne(i => i.CreatedBy)
            .WithMany()
            .HasForeignKey(i => i.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Assignee)
                .WithMany()
                .HasForeignKey(i => i.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Issue>()
                .HasOne(i => i.ChangedBy)
                .WithMany()
                .HasForeignKey(i => i.ChangedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(i => i.CreatedBy)
                .WithMany()
                .HasForeignKey(i => i.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(i => i.Assignee)
                .WithMany()
                .HasForeignKey(i => i.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(i => i.ChangedBy)
                .WithMany()
                .HasForeignKey(i => i.ChangedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
    }
