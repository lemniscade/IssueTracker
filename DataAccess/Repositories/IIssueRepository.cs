using IssueTracker.Business.Services;
using IssueTracker.DataAccess.Dtos;
using IssueTracker.Entity.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repositories
{
    public interface IIssueRepository
    {
        bool Create(string title, string description, int type, int statusId, int priority, string assigneeUsername, string createdUsername, int effort, string projectTitle, UserService userService);
        bool Update(string findingTitle, string? title, string? description, int? type, int? statusId, int? priority, string? assigneeUsername, string? updatedUsername, int? effort, string? projectTitle);
        bool Delete(string title);
        List<Issue> GetAll(List<Issue> listToReturn, string username, string title, int? type, int? typeAscending, int? priority, bool? priorityAscending, int? status);
    }
}
