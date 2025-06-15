using IssueTracker.Entity;
using IssueTracker.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        public bool Create(string title, string description, int type, int statusId, int priority, string assigneeUsername, string createdUsername, int effort, string projectTitle)
        {
            using (var context = new ApplicationDbContext()){
                Issue issue = new Issue
                {
                    Title = title,
                    Description = description,
                    TypeEnumId = type,
                    StatusEnumId = statusId,
                    PriorityEnumId = priority,
                    Effort = effort,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    AssignedAt = DateTime.Now,
                    Project = context.Projects.FirstOrDefault(p => p.Title == projectTitle) ?? throw new ArgumentException("Project not found", nameof(projectTitle)),
                };
                return context.SaveChanges()>0;

            }

        }
        public bool Update(string findingTitle ,string? title, string? description, int? type, int? statusId, int? priority, string? assigneeUsername, string? updatedUsername, int? effort, string? projectTitle)
        {
            using (var context=new ApplicationDbContext())
            {
                Issue issue=context.Issues.FirstOrDefault(i => i.Title == findingTitle) ?? throw new ArgumentException("Issue not found", nameof(findingTitle));
                if(issue != null)
                {
                    if (title != null) issue.Title = title;
                    if (description != null) issue.Description = description;
                    if (type != null) issue.TypeEnumId = (int)type;
                    if (statusId.HasValue) issue.StatusEnumId = statusId.Value;
                    if (priority.HasValue) issue.PriorityEnumId = priority.Value;
                    if (assigneeUsername!= null || assigneeUsername!="") issue.Assignee = context.Users.FirstOrDefault(u => u.Username == assigneeUsername) ?? throw new ArgumentException("Assignee not found", nameof(assigneeUsername));
                    if (updatedUsername != null || updatedUsername != "")
                    {
                        issue.ChangedBy = context.Users.FirstOrDefault(u => u.Username == updatedUsername) ?? throw new ArgumentException("Updater not found", nameof(updatedUsername));
                        issue.UpdatedAt = DateTime.Now;
                    }
                    if(effort.HasValue) issue.Effort = effort.Value;
                    if (projectTitle != null || projectTitle != "")
                    {
                        issue.Project = context.Projects.FirstOrDefault(p => p.Title == projectTitle) ?? throw new ArgumentException("Project not found", nameof(projectTitle));
                    }
                }
                return context.SaveChanges() > 0;
            }
        }
        public bool Delete(string title)
        {
            using (var context = new ApplicationDbContext())
            {
                var issue = context.Issues.FirstOrDefault(i => i.Title == title);
                if (issue == null)
                {
                    throw new ArgumentException("Issue not found", nameof(title));
                }
                context.Issues.Remove(issue);
                return context.SaveChanges() > 0;
            }
        }

        public List<Issue> GetAll(string? username ,string? title)
        {
            using (var context=new ApplicationDbContext())
            {
                if(username != null && username != "" && title!=null && title!="")
                {
                    return context.Issues.Where(i=>i.CreatedBy.Username==username || i.Assignee.Username==username && i.Title==title).ToList();
                }
                if ((username == null || username == "") && title != null && title != "")
                {
                    return context.Issues.Where(i => i.Title == title).ToList();
                }
                if (username != null && username != "" && (title == null || title == ""))
                {
                    return context.Issues.Where(i => i.CreatedBy.Username == username || i.Assignee.Username == username).ToList();
                }
                if (username == null && username == "" && title == null && title == "")
                {
                   return  context.Issues.ToList();
                }
                return new List<Issue>();
            }
        }

        public List<Issue> GetByFilter(int type,bool? ascending)
        {
            using (var context = new ApplicationDbContext())
            {
                var query = context.Issues.AsQueryable();
                if (type==1 || type==2)
                {
                    query = query.Where(i => i.TypeEnumId == type);
                }
                if (ascending.HasValue)
                {
                    query = ascending.Value ? query.OrderBy(i => i.CreatedAt) : query.OrderByDescending(i => i.CreatedAt);
                }
                return query.ToList();
            }
        }

        public Issue GetByTitle(string title)
        {
            using (var context = new ApplicationDbContext())
            {
                var issue = context.Issues.FirstOrDefault(i => i.Title == title);
                if (issue == null)
                {
                    throw new ArgumentException("Issue not found", nameof(title));
                }
                return issue;
            }
        }

        
    }
}
