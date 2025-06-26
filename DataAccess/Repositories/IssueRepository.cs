using Azure;
using FluentValidation;
using IssueTracker.Business.Exceptions;
using IssueTracker.Business.Logging;
using IssueTracker.Business.Services;
using IssueTracker.Business.Validations;
using IssueTracker.Entity;
using IssueTracker.Entity.Enums;
using IssueTracker.Entity.Models;
using IssueTracker.Migrations;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IssueTracker.DataAccess.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private IssueValidator _validator = new IssueValidator();
        private ExceptionLogging _logging = new ExceptionLogging();
        public bool Create(string title, string description, int type, int statusId, int priority, string assigneeUsername, string createdUsername, int effort, string projectTitle, UserService userService)
        {
            
            using (var context = new ApplicationDbContext())
            {
                Project project = context.Projects.FirstOrDefault(p => p.Title == projectTitle);
                if(project == null)
                {
                    _logging.Create("Project not found.", new Dictionary<string, object> { { "Error", "There is not any project which you want." } });
                    AnsiConsole.MarkupLine("[red]Project not found[/]");
                    return false;
                }
                Issue previousIssue = context.Issues.FirstOrDefault(i => i.Title == title);
                if(previousIssue != null)
                {
                    _logging.Create("An issue with the same title already exists.", new Dictionary<string, object> { { "Error", "There is another issue with same title" } });
                    AnsiConsole.MarkupLine("[red]An issue with the same title already exists.[/]");
                    return false;
                }
                User user=context.Users.FirstOrDefault(u => u.Username == assigneeUsername);
                if (user == null)
                {
                    _logging.Create("User to assign is not exist.", new Dictionary<string, object> { { "Error", "User to assign is not exist." } });
                    AnsiConsole.MarkupLine("[red]User to assign is not exist.[/]");
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
                        _logging.Create(error.ErrorMessage,new Dictionary<string, object> { { "Error", error.ErrorMessage} });
                        AnsiConsole.MarkupLine($"\n[red]{error.ErrorMessage}[/]");
                        return false;
                    }
                }
                bool exists = context.Issues.Any(i => i.Title == title);
                if (exists)
                {
                    _logging.Create("Issue creation failed, an issue with the same title already exists.", new Dictionary<string, object> { { "Error", "Tere is another issue with same title" } });
                    AnsiConsole.MarkupLine("\n[red]An issue with the same title already exists.[/]");
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
                    _logging.Create("An issue with the same title already exists.", new Dictionary<string, object> { { "Error", "There is another issue with same title" } });
                    AnsiConsole.MarkupLine("\n[red]An issue with the same title already exists..[/]");
                    return false;
                }
                //Issue issue = context.Issues.FirstOrDefault(i => i.Title == findingTitle);

                //            List<IssueUser> assignedBefore = new List<IssueUser>();
                //            assignedBefore = context.Issues.Include(i => i.IssueUsers)
                //    .ThenInclude(iu => iu.User)
                //.Where(i => i.Id == issue.Id)
                //.SelectMany(i => i.IssueUsers).ToList();
                //            issue.IssueUsers= assignedBefore;


                Issue issue = context.Issues
    .Include(i => i.IssueUsers)
        .ThenInclude(iu => iu.User)
    .FirstOrDefault(i => i.Title == findingTitle);
                //.Include(i => i.IssueUsers)
                //    .ThenInclude(iu => iu.User)
                //.Where(i => i.Id == issue.Id)
                //.SelectMany(i => i.IssueUsers).FirstOrDefault(x => x.UserTypeEnumId == 3);
                //            if (issue.IssueUsers.Count==0){
                //           IssueUser assignedBefore = context.Issues
                //.Include(i => i.IssueUsers)
                //    .ThenInclude(iu => iu.User)
                //.Where(i => i.Id == issue.Id)
                //.SelectMany(i => i.IssueUsers).FirstOrDefault(x => x.UserTypeEnumId == 3);
                //            List<IssueUser> listOfAssignedUser=new List<IssueUser>();
                //                issue.IssueUsers.Add(assignedBefore);
                //            }

                if (issue != null)
                {
                    if (title != null) issue.Title = title;
                    if (description != null) issue.Description = description;
                    if (type != null) issue.TypeEnumId = (int)type;
                    if (statusId != null) issue.StatusEnumId = statusId.Value;
                    if (priority != null) issue.PriorityEnumId = priority.Value;
                    if (!string.IsNullOrEmpty(assigneeUsername)) {
                        User user = context.Users.FirstOrDefault(u => u.Username == assigneeUsername);
                        if (user == null)
                        {
                            _logging.Create("User to assign is not exist.", new Dictionary<string, object> { { "Error", "User to assign is not exist." } });
                            AnsiConsole.MarkupLine("[red]User to assign is not exist.[/]");
                            assigneeUsername = null;
                            return false;
                        }



                        var assignee = issue.IssueUsers.FirstOrDefault(iu => iu.UserTypeEnumId == 3);
                        if (assignee != null)
                        {
                            assignee.User = user;
                        }
                        else
                        {
                            issue.IssueUsers.Add(
                    new IssueUser
                    {
                        User = context.Users.FirstOrDefault(u => u.Username == assigneeUsername),
                        UserTypeEnumId = 3
                    }
                    );
                        }
                    }
                    if (!string.IsNullOrEmpty(updatedUsername))
                    {
                        var created = issue.IssueUsers.FirstOrDefault(iu => iu.UserTypeEnumId == 1);
                        if (created != null)
                        {
                            created.User = context.Users.FirstOrDefault(u => u.Username == updatedUsername);
                        }

                        issue.UpdatedAt = DateTime.Now;
                    }
                    if (effort != null) issue.Effort = effort.Value;
                    if (!string.IsNullOrEmpty(projectTitle))
                    {
                        
                            Project isExistProject= context.Projects.FirstOrDefault(p => p.Title == projectTitle);
                        if(isExistProject!=null){
                            issue.Project = isExistProject;
                        }
                        else
                        {
                            _logging.Create("There is not any project with this name", new Dictionary<string, object> { { "Error", "There is not any project with this name" } });
                            AnsiConsole.MarkupLine("\n[red]There is not any project with this name[/]");
                        }
                    }
                }

                var validator = new IssueValidator();
                var validationResult = validator.Validate(issue);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        _logging.Create(error.ErrorMessage, new Dictionary<string, object> { { "Error", error.ErrorMessage } });
                        AnsiConsole.MarkupLine($"\n[red]{error.ErrorMessage}[/]");
                    }
                }


                if (title != null)
                {
                    bool titleExists = context.Issues.Any(i => i.Title == title && i.Id != issue.Id);
                    if (titleExists)
                    {
                        _logging.Create("An issue with the same title already exists.", new Dictionary<string, object> { { "Error", "There is another issue with same title" } });
                        AnsiConsole.MarkupLine("\n[red]An issue with the same title already exists.[/]");
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
                    _logging.Create("Issue not found for delete operations.", new Dictionary<string, object> { { "Error","There is no issue for delete." } });
                    AnsiConsole.MarkupLine("\n[red]Issue not found for delete operations.[/]");
                }
                context.Issues.Remove(issue);
                return context.SaveChanges() > 0;
            }
        }

        public List<Issue> GetAll(List<Issue> issueList, string? username, string? title, int? type, int? typeAscending,  int? priority, bool? priorityAscending, int? status)
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
            {   if(issues.Count == 0)
                {
                    issues = context.Issues.Include(i => i.IssueUsers)
                                            .ThenInclude(iu => iu.User)
                                        .ToList();
                }
                
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
                if(priority.HasValue)
                {
                    issues = issues.Where(i => i.PriorityEnumId == priority.Value).ToList();
                }
                if (status.HasValue)
                {
                    issues = issues.Where(i => i.StatusEnumId == status.Value).ToList();
                }
                if (priorityAscending.HasValue)
                {
                    if (priorityAscending.Value==true)
                    {
                        issues=issues.OrderBy(i => i.PriorityEnumId).ToList();
                    }
                    else if(priorityAscending.Value==false)
                    {
                        issues=issues.OrderByDescending(i => i.PriorityEnumId).ToList();
                    }
                }
                if(typeAscending.HasValue)
                {
                    if (typeAscending.Value == 1)
                    {
                        issues = issues.OrderBy(i => i.TypeEnumId==1).ToList();
                    }
                    else if (typeAscending.Value == 2)
                    {
                        issues = issues.OrderByDescending(i => i.TypeEnumId==2).ToList();
                    }
                }
                return issues;
            }
        }

    }
}
