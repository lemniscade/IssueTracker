using IssueManager.IssueManager.Application.Interceptor;
using IssueManager.IssueManager.Domain.ValueObjects;
using IssueManager.IssueManager.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IssueManager.IssueManager.Application.Business
{
    public class DbOperations
    {
        public string command;
        public User user;
        public DbOperations(string command,string sequence) {
            string fullCommand=command+sequence;
            this.command = fullCommand;
        }

        public void DbCommands(string? username,string? password, ProjectId? projectId, string? projectTitle, string? projectDescription, int? statusId, int? priority, User? createdBy, User? changedBy, DateTime? createdAt, DateTime? changedAt,
            IssueId? issueId,string? issueTitle, string? issueDescription, string? type, DateTime? dueDate, Project? project) {
            switch (this.command)
            {
                case "1.1":
                    UserOperations.Register(username, password);
                    break;
                case "1.2":
                    user = UserOperations.Login(username, password);
                    break;
                case "2.1":
                    Project createdProject = Project.Create(projectTitle,projectDescription,statusId,priority,user,createdAt);
                    break;
                case "2.2":
                    using (var dbContext = new ApplicationDbContext())
                    {
                        Project? selectedProject = dbContext.Projects.FirstOrDefault(p => p.ProjectId == projectId);
                        if (selectedProject != null)
                        {
                            selectedProject.Update(selectedProject.ProjectId, projectTitle, projectDescription, user, changedAt);
                        }

                    }
                    break;
                case "3.1":
                    Issue issue = Issue.Create(issueTitle, issueDescription, type, statusId, priority, user, dueDate, createdAt, project);
                    break;
                case "3.2":
                    using (var dbContext = new ApplicationDbContext())
                    {
                        Issue? selectedIssue = dbContext.Issues.FirstOrDefault(p => p.IssueId == issueId);
                        if (selectedIssue != null)
                        {
                            selectedIssue.Update(issueId,issueTitle, issueDescription, type, statusId, priority, dueDate,user, changedAt, project);
                        }

                    }
                    break;
                default:
                    break;
            }

        }
    }
}
