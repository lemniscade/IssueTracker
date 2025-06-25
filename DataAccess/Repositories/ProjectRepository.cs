using FluentValidation;
using IssueTracker.Business.Services;
using IssueTracker.Business.UI;
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
    public class ProjectRepository : IProjectRepository
    {
        private User existUser;
        IValidator<User> validatorForUser = new UserValidator();
        public ProjectRepository(UserService userService)
        {
            //    IUserRepository userRepository = new UserRepository(validatorForUser);
            //    UserService userService = new UserService(validatorForUser, userRepository);
            //this.existUser = userService.existUser;
        }
        public bool Create(string title, string description, UserService userService)
        {
            List<ProjectUser> users;
            using (var context = new ApplicationDbContext())
            {
                Project previousProject = context.Projects.FirstOrDefault(p => p.Title == title);
                if (previousProject != null)
                {
                    Console.WriteLine("A project with the same title already exists.");
                    return false;
                }
                users = new List<ProjectUser>
                {
                    new ProjectUser
                    {
                        UserTypeEnumId = 1,
                        User = context.Users.FirstOrDefault(u => u.Username == userService.existUser.Username)
                    }
                };
                Project project = new Project
                {
                    Title = title,
                    Description = description,
                    CreatedAt = DateTime.Now
                };
                project.ProjectUsers = users;
                context.Projects.Add(project);
                return context.SaveChanges() > 0;
            }
        }
        public bool Update(string findingTitle, string? title, string? description, UserService userService)
        {
            List<ProjectUser> users;
            using (var context = new ApplicationDbContext())
            {
                Project previousProject = context.Projects.FirstOrDefault(p => p.Title == title);
                if (previousProject != null)
                {
                    Console.WriteLine("A project with the same title already exists.");
                    return false;
                }
                users = new List<ProjectUser>
                {
                    new ProjectUser
                    {
                        UserTypeEnumId = 1,
                        User = context.Users.FirstOrDefault(u => u.Username == userService.existUser.Username)
                    }
                };
                Project project = context.Projects.FirstOrDefault(p => p.Title == findingTitle);
                project.Title = title ?? project.Title;
                project.Description = description ?? project.Description;
                project.ChangedAt = DateTime.Now;
                project.ProjectUsers = users;
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
            List<Project> projects = new List<Project>();
            using (var context = new ApplicationDbContext())
            {

                projects = context.Projects
    .Include(p => p.ProjectUsers)
        .ThenInclude(pu => pu.User)
    .Include(p => p.Issues)
        .ThenInclude(i => i.IssueUsers)
            .ThenInclude(iu => iu.User)
    .ToList();

                // Döngüsel referansları kırmak için manuel temizleme:
                foreach (var project in projects)
                {
                    foreach (var pu in project.ProjectUsers)
                    {
                        pu.Project = null; // ProjectUser içindeki Project'i boşalt
                        if (pu.User != null)
                        {
                            pu.User.ProjectUsers = null; // User içindeki listeleri null yap
                            pu.User.IssueUsers = null;
                        }
                    }

                    foreach (var issue in project.Issues)
                    {
                        issue.Project = null; // Issue içindeki Project'i boşalt
                        foreach (var iu in issue.IssueUsers)
                        {
                            iu.Issue = null; // IssueUser içindeki Issue'yi boşalt
                            if (iu.User != null)
                            {
                                iu.User.ProjectUsers = null;
                                iu.User.IssueUsers = null;
                            }
                        }
                    }
                }


                if (!string.IsNullOrEmpty(username))
                {
                    projects = context.Projects.Where(i => i.ProjectUsers.Any(pu => pu.User.Username == username))
                                                .ToList();
                }
                if (!string.IsNullOrEmpty(title))
                {
                    projects = projects.Where(i => i.Title == title).ToList();
                }
                foreach (var proj in projects)
                {
                    proj.Issues = context.Issues.Where(issue => issue.ProjectId == proj.Id).ToList();
                }
                if (projects.Count > 0)
                {
                    return projects;
                }
                else
                {
                    return new List<Project>();
                }
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
