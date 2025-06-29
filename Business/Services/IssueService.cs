﻿using FluentValidation;
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

        public void CreateIssue(string title, string description, int type, int statusId, int priority, string assigneeUsername, string createdUsername, int effort, string projectTitle,UserService userService)
        {
            _issueRepository.Create(title, description, type, statusId, priority, assigneeUsername, createdUsername, effort, projectTitle, userService);
        }
        public void DeleteIssue(string title)
        {
            _issueRepository.Delete(title);
        }

        public List<Issue> GetAllIssues(List<Issue>  listToReturn,string username, string title, int? type, int? typeAscending,int? priority ,bool? priorityAscending,int? status)
        {
            List<Issue> issueList=_issueRepository.GetAll(listToReturn, username, title,type, typeAscending, priority, priorityAscending,status);
            username = null;
            title = null;
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
