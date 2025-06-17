using FluentValidation;
using IssueTracker.Business.Exceptions;
using IssueTracker.Business.Logging;
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
        private readonly IValidator<Issue> _validator;
        public bool Create(string title, string description, int type, int statusId, int priority, string assigneeUsername, string createdUsername, int effort, string projectTitle)
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
                        Project = context.Projects.FirstOrDefault(p => p.Title == projectTitle) ?? throw new ArgumentException("Project not found", nameof(projectTitle)),
                    };
                    var validationResult = _validator.Validate(issue);

                    if (!validationResult.IsValid)
                    {
                        throw new FluentValidation.ValidationException(validationResult.Errors);
                    }
                    bool exists = context.Issues.Any(i => i.Title == title);
                    if (exists)
                        throw new BusinessException("An issue with the same title already exists.");
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
                    Issue issue = context.Issues.FirstOrDefault(i => i.Title == findingTitle) ?? throw new ArgumentException("Issue not found", nameof(findingTitle));
                    if (issue != null)
                    {
                        if (title != null) issue.Title = title;
                        if (description != null) issue.Description = description;
                        if (type != null) issue.TypeEnumId = (int)type;
                        if (statusId!=null) issue.StatusEnumId = statusId.Value;
                        if (priority!=null) issue.PriorityEnumId = priority.Value;
                        if (assigneeUsername != null || assigneeUsername != "") issue.Assignee = context.Users.FirstOrDefault(u => u.Username == assigneeUsername) ?? throw new ArgumentException("Assignee not found", nameof(assigneeUsername));
                        if (updatedUsername != null || updatedUsername != "")
                        {
                            issue.ChangedBy = context.Users.FirstOrDefault(u => u.Username == updatedUsername) ?? throw new ArgumentException("Updater not found", nameof(updatedUsername));
                            issue.UpdatedAt = DateTime.Now;
                        }
                        if (effort!=null) issue.Effort = effort.Value;
                        if (projectTitle != null || projectTitle != "")
                        {
                            issue.Project = context.Projects.FirstOrDefault(p => p.Title == projectTitle) ?? throw new ArgumentException("Project not found", nameof(projectTitle));
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

        public List<Issue> GetAll(string? username, string? title)
        {
            using (var context = new ApplicationDbContext())
            {
                if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(title))
                {
                    return context.Issues.ToList();
                }
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(title))
                {
                    return context.Issues.Where(i => i.CreatedBy.Username == username || i.Assignee.Username == username && i.Title == title).ToList();
                }
                if (string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(title))
                {
                    return context.Issues.Where(i => i.Title == title).ToList();
                }
                if (!string.IsNullOrEmpty(username) && string.IsNullOrEmpty(title))
                {
                    return context.Issues.Where(i => i.CreatedBy.Username == username || i.Assignee.Username == username).ToList();
                }
                
                return new List<Issue>();
            }
        }

        public List<Issue> GetByFilter(int type, bool? ascending)
        {
            using (var context = new ApplicationDbContext())
            {
                var query = context.Issues.AsQueryable();
                if (type == 1 || type == 2)
                {
                    query = query.Where(i => i.TypeEnumId == type);
                }
                if (ascending.HasValue)
                {
                    query = ascending.Value ? query.OrderBy(i => i.PriorityEnumId) : query.OrderByDescending(i => i.PriorityEnumId);
                }
                return query.ToList();
            }
        }
    }
}
