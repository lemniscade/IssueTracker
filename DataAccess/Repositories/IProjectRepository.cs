using IssueTracker.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repositories
{
    public interface IProjectRepository
    {
        bool Create(string title, string description, int? statusId, int? priority, string assigneeUsername, string createdUsername, int? effort, Project? project);
        bool Update(string findingTitle, string? title, string? description, string? assigneeUsername);
        bool Delete(string title);
        public List<Issue> GetCreated(string title, string username);
        public List<Issue> GetAssigned(string title, string username);
        public List<Issue> GetAll(string title, string username);
    }
}
