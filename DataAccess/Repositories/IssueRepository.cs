using Azure;
using FluentValidation;
using IssueTracker.Business.Exceptions;
using IssueTracker.Business.Logging;
using IssueTracker.Business.Services;
using IssueTracker.Business.Validations;
using IssueTracker.Entity;
using IssueTracker.Entity.Enums;
using IssueTracker.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private IssueValidator _validator = new IssueValidator();
        public bool Create(string title, string description, int type, int statusId, int priority, string assigneeUsername, string createdUsername, int effort, string projectTitle, UserService userService)
        {
            
            using (var context = new ApplicationDbContext())
            {
                Project project = context.Projects.FirstOrDefault(p => p.Title == projectTitle);
                if(project == null)
                {
                    Console.WriteLine("Project not found");
                    return false;
                }
                Issue previousIssue = context.Issues.FirstOrDefault(i => i.Title == title);
                if(previousIssue != null)
                {
                    Console.WriteLine("An issue with the same title already exists.");
                    return false;
                }
                List<IssueUser> users = new List<IssueUser>
            {
                new IssueUser
                {
                    UserTypeEnumId = 1,
                    User = context.Users.FirstOrDefault(u => u.Username == createdUsername)
                },
                new IssueUser
                {
                    UserTypeEnumId = 3,
                    User = context.Users.FirstOrDefault(u => u.Username == assigneeUsername)
                }
            };
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
                    Project = context.Projects.FirstOrDefault(p => p.Title == projectTitle) ?? throw new ArgumentException("Project not found", nameof(projectTitle))
                };
                issue.IssueUsers = users;

                var validationResult = _validator.Validate(issue);

                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                        return false;
                    }
                }
                bool exists = context.Issues.Any(i => i.Title == title);
                if (exists)
                {
                    Console.WriteLine("An issue with the same title already exists.");
                    return false;
                }
                context.Issues.Add(issue);
                return context.SaveChanges() > 0;

            }
        }
        public bool Update(string findingTitle, string? title, string? description, int? type, int? statusId, int? priority, string? assigneeUsername, string? updatedUsername, int? effort, string? projectTitle)
        {
            using (var context = new ApplicationDbContext())
            {
                Issue previousIssue = context.Issues.FirstOrDefault(i => i.Title == title);
                if (previousIssue != null)
                {
                    Console.WriteLine("An issue with the same title already exists.");
                    return false;
                }
                Issue issue = context.Issues.FirstOrDefault(i => i.Title == findingTitle);
                if (issue != null)
                {
                    if (title != null) issue.Title = title;
                    if (description != null) issue.Description = description;
                    if (type != null) issue.TypeEnumId = (int)type;
                    if (statusId != null) issue.StatusEnumId = statusId.Value;
                    if (priority != null) issue.PriorityEnumId = priority.Value;
                    if (assigneeUsername != null || assigneeUsername != "") {
                        issue.IssueUsers.Add(
                    new IssueUser
                    {
                        User = context.Users.FirstOrDefault(u => u.Username == assigneeUsername),
                        UserTypeEnumId = 3
                    }
                    );
                    }
                    if (updatedUsername != null || updatedUsername != "")
                    {
                        issue.IssueUsers.Add(
                    new IssueUser
                    {
                        User = context.Users.FirstOrDefault(u => u.Username == updatedUsername),
                        UserTypeEnumId = 2
                    }
                    );

                        issue.UpdatedAt = DateTime.Now;
                    }
                    if (effort != null) issue.Effort = effort.Value;
                    if (projectTitle != null || projectTitle != "")
                    {
                        issue.Project = context.Projects.FirstOrDefault(p => p.Title == projectTitle);
                    }
                }

                var validator = new IssueValidator();
                var validationResult = validator.Validate(issue);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        Console.WriteLine($"{error.ErrorMessage}");
                    }
                }


                if (title != null)
                {
                    bool titleExists = context.Issues.Any(i => i.Title == title && i.Id != issue.Id);
                    if (titleExists)
                    {

                        Console.WriteLine("An issue with the same title already exists.");
                        return false;
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
                    Console.WriteLine("Issue not found for delete operations");
                }
                context.Issues.Remove(issue);
                return context.SaveChanges() > 0;
            }
        }

        public List<Issue> GetAll(List<Issue> issueList, string? username, string? title, int? type, bool? ascending, int? priority, int? status)
        {
            List<Issue> issues;
            if (issueList.Count == 0)
            {
                issues = new List<Issue>();
            }
            else
            {
                issues = issueList;
            }

            using (var context = new ApplicationDbContext())
            {
                if (!string.IsNullOrEmpty(username))
                {
                    issues = context.Issues.Include(i => i.IssueUsers)
                                            .ThenInclude(iu => iu.User)
                                        .Where(i => i.IssueUsers.Any(iu => iu.User.Username == username))
                                        .ToList();
                }
                if (!string.IsNullOrEmpty(title))
                {
                    issues = issues.Where(i => i.Title == title).ToList();
                }
                if (type.HasValue)
                {
                    issues = issues.Where(i => i.TypeEnumId == type.Value).ToList();
                }
                if (status.HasValue)
                {
                    issues = issues.Where(i => i.StatusEnumId == status.Value).ToList();
                }
                if (ascending.HasValue)
                {
                    if (ascending.Value==true)
                    {
                        issues.OrderBy(i => i.PriorityEnumId).ToList();
                    }
                    else if(ascending.Value==false)
                    {
                        issues.OrderByDescending(i => i.PriorityEnumId).ToList();
                    }
                }
                return issues;
            }
        }
    }
}
