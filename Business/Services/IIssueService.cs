using IssueTracker.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Business.Services
{
    public interface IIssueService
    {
        List<Issue> GetAllIssues(List<Issue> listToReturn, string username, string title, int? type, int? typeAscending, int? priority, bool? priorityAscending, int? status);
        void CreateIssue(string title, string description, int type, int statusId, int priority, string assigneeUsername, string createdUsername, int effort, string projectTitle, UserService userService);
        void UpdateIssue(string findingTitle, string? title, string? description, int? type, int? statusId, int? priority, string? assigneeUsername, string? updatedUsername, int? effort, string? projectTitle);
        void DeleteIssue(string title);
    }
}
