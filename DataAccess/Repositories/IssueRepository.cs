using FluentValidation;
using IssueTracker.Business.Exceptions;
using IssueTracker.Business.Logging;
using IssueTracker.Business.Services;
using IssueTracker.Business.Validations;
using IssueTracker.Entity;
using IssueTracker.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private IssueValidator _validator=new IssueValidator();
        public bool Create(string title, string description, int type, int statusId, int priority, string assigneeUsername, string createdUsername, int effort, string projectTitle,UserService userService)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
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
                        Assignee = context.Users.FirstOrDefault(u => u.Username == assigneeUsername) ?? throw new ArgumentException("Assignee not found", nameof(assigneeUsername)),
                        Project = context.Projects.FirstOrDefault(p => p.Title == projectTitle) ?? throw new ArgumentException("Project not found", nameof(projectTitle)),
                        CreatedBy = context.Users.FirstOrDefault(u => u.Username == userService.existUser.Username) ?? throw new ArgumentException("Creator not found", nameof(createdUsername))
                    };
                    var validationResult = _validator.Validate(issue);

                    if (!validationResult.IsValid)
                    {
                        throw new FluentValidation.ValidationException(validationResult.Errors);
                    }
                    bool exists = context.Issues.Any(i => i.Title == title);
                    if (exists)
                        throw new BusinessException("An issue with the same title already exists.");
                    context.Issues.Add(issue);
                    return context.SaveChanges() > 0;

                }
            }
            catch (FluentValidation.ValidationException vex)
            {
                Console.WriteLine("Validation Errors (try-catch ile):");
                foreach (var error in vex.Errors)
                    Console.WriteLine($"- {error.PropertyName}: {error.ErrorMessage}");
                throw;
            }
            catch (BusinessException bex)
            {
                Console.WriteLine($"Business Rule Error: {bex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }
        public bool Update(string findingTitle, string? title, string? description, int? type, int? statusId, int? priority, string? assigneeUsername, string? updatedUsername, int? effort, string? projectTitle)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    Issue issue = context.Issues.FirstOrDefault(i => i.Title == findingTitle);
                    if (issue != null)
                    {
                        if (title != null) issue.Title = title;
                        if (description != null) issue.Description = description;
                        if (type != null) issue.TypeEnumId = (int)type;
                        if (statusId != null) issue.StatusEnumId = statusId.Value;
                        if (priority != null) issue.PriorityEnumId = priority.Value;
                        if (assigneeUsername != null || assigneeUsername != "") issue.Assignee = context.Users.FirstOrDefault(u => u.Username == assigneeUsername);
                        if (updatedUsername != null || updatedUsername != "")
                        {
                            issue.ChangedBy = context.Users.FirstOrDefault(u => u.Username == updatedUsername);
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
                            Console.WriteLine($"Hata: {error.ErrorMessage}");
                        }
                    }


                    if (title != null)
                    {
                        bool titleExists = context.Issues.Any(i => i.Title == title && i.Id != issue.Id);
                        if (titleExists)
                            throw new BusinessException("An issue with the same title already exists.");
                    }

                    return context.SaveChanges() > 0;
                }
            }
            catch (ValidationException vex)
            {
                Console.WriteLine("Validation errors:");
                foreach (var err in vex.Errors)
                    Console.WriteLine($"- {err.PropertyName}: {err.ErrorMessage}");
                throw;
            }
            catch (BusinessException bex)
            {
                Console.WriteLine($"Business exception: {bex.Message}");
                throw;
            }
            catch (ArgumentException aex)
            {
                Console.WriteLine($"Argument exception: {aex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected exception: {ex.Message}");
                throw;
            }
        }
        public bool Delete(string title)
        {
            try
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
            catch (BusinessException bex)
            {
                {
                    throw new BusinessException($"{"There is no issue for delete operations"}");
                }
            }
        }

        public List<Issue> GetAll(List<Issue> issueList,string? username, string? title, int? type, bool? ascending,int? priority, int? status)
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
                    if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(title))
                    {
                        issues = context.Issues.ToList();
                    }
                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(title))
                    {
                        issues = issues.Where(i => i.CreatedBy.Username == username || i.Assignee.Username == username && i.Title == title).ToList();
                    }
                    if (string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(title))
                    {
                        issues = issues.Where(i => i.Title == title).ToList();
                    }
                    if (!string.IsNullOrEmpty(username) && string.IsNullOrEmpty(title))
                    {
                        issues = issues.Where(i => i.CreatedBy.Username == username || i.Assignee.Username == username).ToList();
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
                        if (ascending.Value)
                        {
                            issues.OrderBy(i => i.PriorityEnumId).ToList();
                        }
                        else
                        {
                            issues.OrderByDescending(i => i.PriorityEnumId).ToList();
                        }
                    }
                    return issues;
                }
        }
    }
}
