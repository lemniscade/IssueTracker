using IssueTracker.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Business.Services
{
    public interface IProjectService
    {
        IEnumerable<Project> GetAllProjects(string? title, string? username);
        void CreateProject(string title, string description,UserService userService);
        void UpdateProject(string findingTitle, string? title, string? description, UserService userService);
        void DeleteProject(string title);
    }
}
