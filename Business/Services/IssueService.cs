using FluentValidation;
using IssueTracker.Business.Exceptions;
using IssueTracker.DataAccess.Repositories;
using IssueTracker.Entity;
using IssueTracker.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Business.Services
{
    public class IssueService : IIssueService
    {
        private readonly IValidator<Issue> _validator;
        private readonly IIssueRepository _issueRepository;
        public IssueService(IValidator<Issue> validator,IIssueRepository issueRepository)
        {
            _validator = validator;
            _issueRepository = issueRepository;
        }

        public void CreateIssue(string title, string description, int type, int statusId, int priority, string assigneeUsername, string createdUsername, int effort, string projectTitle)
        {
            _issueRepository.Create(title, description, type, statusId, priority, assigneeUsername, createdUsername, effort, projectTitle);
        }
        public void DeleteIssue(string title)
        {
            _issueRepository.Delete(title);
        }

        public IEnumerable<Issue> GetAllIssues(string username,string title)
        {
            List<Issue> issueList=_issueRepository.GetAll(username, title);
            if(issueList.Count == 0)
            {
                throw new BusinessException("There is no issue for this operation");
            }
            return issueList;
            //specter ile foreach ile içinde dön
            //todo
        }

        public void UpdateIssue(string findingTitle, string? title, string? description, int? type, int? statusId, int? priority, string? assigneeUsername, string? updatedUsername, int? effort, string? projectTitle)
        {
            _issueRepository.Update(findingTitle, title, description, type, statusId, priority, assigneeUsername, updatedUsername, effort, projectTitle);
        }
    }
}
