using IssueTracker.Entity;
using IssueTracker.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repositories
{
    public class ProjectRepository:IProjectRepository
    {
        public bool Create(string title, string description, int? statusId, int? priority, string assigneeUsername, string createdUsername, int? effort, Project? project)
        {
            using (var context = new ApplicationDbContext())
            {
                Issue issue = new Issue
                {
                    Title = title,
                    Description = description,
                    StatusEnumId = (int)statusId,
                    PriorityEnumId = (int)priority,
                    Assignee = context.Users.FirstOrDefault(u => u.Username == assigneeUsername),
                    CreatedBy = context.Users.FirstOrDefault(u => u.Username == createdUsername),
                    Effort = (int)effort,
                    Project = project,
                };
                context.Issues.Add(issue);
                return context.SaveChanges() > 0;
            }
        }
        public bool Update(string findingTitle,string? title, string? description, string? assigneeUsername)
        {
            using (var context = new ApplicationDbContext()) {
                Project project = context.Projects.FirstOrDefault(p => p.Title == findingTitle);
                project.Title = title ?? project.Title;
                project.Description = description ?? project.Description;
                project.Assignee = assigneeUsername != null ? context.Users.FirstOrDefault(u => u.Username == assigneeUsername) : project.Assignee;
                //project.ChangedBy =  sistemden alınacak
                project.ChangedAt = DateTime.Now;
                return context.SaveChanges() > 0;
            }
        }
        public bool Delete(string title)
        {
            using (var context = new ApplicationDbContext())
            {
                var issue = context.Issues.FirstOrDefault(i => i.Title == title);
                if (issue != null)
                {
                    context.Issues.Remove(issue);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
        }

        public List<Issue> GetAssigned(string title, string username)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Issues.Where(i => i.Title == title && i.Assignee.Username == username).ToList();
            }
        }

        public List<Issue> GetCreated(string title, string username)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Issues
                    .Where(i => i.Title == title && i.CreatedBy.Username == username)
                    .ToList();
            }
        }

        public List<Issue> GetAll(string title, string username)
        {
            List<Issue> issues = new List<Issue>();
            issues = GetCreated(title, username);
            issues.AddRange(GetAssigned(title, username));
            return issues;
        }

    }
}
