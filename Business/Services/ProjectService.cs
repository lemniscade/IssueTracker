﻿using FluentValidation;
using IssueTracker.DataAccess.Repositories;
using IssueTracker.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Business.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IValidator<Project> _validator;
        private readonly IProjectRepository _projectRepository;
        public ProjectService(IValidator<Project> validator, IProjectRepository projectRepository)
        {
            _validator = validator;
            _projectRepository = projectRepository;
        }
        public void CreateProject(string title, string description, UserService userService)
        {
            _projectRepository.Create(title, description,userService);
        }

        public void DeleteProject(string title)
        {
            _projectRepository.Delete(title);
        }

        public IEnumerable<Project> GetAllProjects(string? title, string? username)
        {
           List<Project> projectList=_projectRepository.GetAll(title, username).ToList();

           return projectList;

        }

        public void UpdateProject(string findingTitle, string? title, string? description,UserService userService)
        {
            _projectRepository.Update(findingTitle, title, description,userService);
        }
    }
}
