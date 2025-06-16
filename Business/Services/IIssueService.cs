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
        IEnumerable<Issue> GetAllIssues(string? title, string? username);
        void CreateIssue(string title, string description, int type, int statusId, int priority, string assigneeUsername, string createdUsername, int effort, string projectTitle);
        void UpdateIssue(string findingTitle, string? title, string? description, int? type, int? statusId, int? priority, string? assigneeUsername, string? updatedUsername, int? effort, string? projectTitle);
        void DeleteIssue(string title);
    }
}
