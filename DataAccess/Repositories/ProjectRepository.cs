using FluentValidation;
using IssueTracker.Business.Services;
using IssueTracker.Business.UI;
using IssueTracker.Business.Validations;
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
        private string usernameOfExistUser;
        IValidator<User> validatorForUser = new UserValidator();
        public ProjectRepository()
        {
            IUserRepository userRepository = new UserRepository(validatorForUser);
            UserService userService = new UserService(validatorForUser, userRepository);
            this.usernameOfExistUser = userService.usernameOfExistUser;
        }
        public bool Create(string title, string description, string assigneeUsername)
        {
            using (var context = new ApplicationDbContext())
            {
                Project project = new Project
                {
                    Title = title,
                    Description = description,
                    Assignee = context.Users.FirstOrDefault(u => u.Username == assigneeUsername),
                    CreatedBy = context.Users.FirstOrDefault(u => u.Username == usernameOfExistUser)
                };
                context.Projects.Add(project);
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
                project.ChangedBy = context.Users.FirstOrDefault(u => u.Username == usernameOfExistUser);
                project.ChangedAt = DateTime.Now;
                return context.SaveChanges() > 0;
            }
        }
        public bool Delete(string title)
        {
            using (var context = new ApplicationDbContext())
            {
                var issue = context.Projects.FirstOrDefault(i => i.Title == title);
                if (issue != null)
                {
                    context.Projects.Remove(issue);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
        }

        public List<Project> GetAll(string? username, string? title)
        {
            using (var context = new ApplicationDbContext())
            {
                if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(title))
                {
                    return context.Projects.ToList();
                }
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(title))
                {
                    return context.Projects.Where(i => i.CreatedBy.Username == username || i.Assignee.Username == username && i.Title == title).ToList();
                }
                if (string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(title))
                {
                    return context.Projects.Where(i => i.Title == title).ToList();
                }
                if (!string.IsNullOrEmpty(username) && string.IsNullOrEmpty(title))
                {
                    return context.Projects.Where(i => i.CreatedBy.Username == username || i.Assignee.Username == username).ToList();
                }

                return new List<Project>();
            }
        }
        //todo
        //public List<Issue> GetAssigned(string title, string username)
        //{
        //    using (var context = new ApplicationDbContext())
        //    {
        //        return context.Projects.Where(i => i.Title == title && i.Assignee.Username == username).ToList();
        //    }
        //}

        //public List<Issue> GetCreated(string title, string username)
        //{
        //    using (var context = new ApplicationDbContext())
        //    {
        //        return context.Projects
        //            .Where(i => i.Title == title && i.CreatedBy.Username == username)
        //            .ToList();
        //    }
        //}

        //public List<Issue> GetAll(string title, string username)
        //{
        //    List<Issue> issues = new List<Issue>();
        //    issues = GetCreated(title, username);
        //    issues.AddRange(GetAssigned(title, username));
        //    return issues;
        //}

    }
}
