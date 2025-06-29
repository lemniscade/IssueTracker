﻿using FluentValidation;
using IssueTracker.Business.Logging;
using IssueTracker.Business.Services;
using IssueTracker.Business.Validations;
using IssueTracker.DataAccess.Repositories;
using IssueTracker.Entity.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Business.UI
{
    public class ConsoleUI
    {
        private IssueService issueService;
        private ProjectService projectService;
        private UserService userService;
        private User existUser;
        private JsonExport jsonExport;
        private ExceptionLogging _logging = new ExceptionLogging();
        private IUserRepository userRepository;
        public ConsoleUI()
        {
            IValidator<Issue> validatorForIssue = new IssueValidator();
            IIssueRepository issueRepository = new IssueRepository();
            IValidator<Project> validatorForProject = new ProjectValidator();

            IValidator<User> validatorForUser = new UserValidator();
            
            this.issueService = new IssueService(validatorForIssue, issueRepository);

            IUserRepository userRepository = new UserRepository(validatorForUser);
            this.userService = new UserService(validatorForUser, userRepository);
            IProjectRepository projectRepository = new ProjectRepository(userService);
            this.projectService = new ProjectService(validatorForProject, projectRepository);
            jsonExport = new JsonExport();
        }
        public User getCurrentUser()
        {
            IValidator<User> validatorForUser = new UserValidator();
            IUserRepository userRepository = new UserRepository(validatorForUser);
            return this.existUser;
        }
        public async void Execute()
        {

            string username;
            string password;
            string? projectTitle = null;
            string? projectDescription = null;
            int? type = null;
            int? statusId = null;
            int? priorityId = null;
            string? assignee = null;
            string? project = null;
            string? issueTitle = null;
            string? issueDescription = null;
            string? findingTitle = null;
            string? newUsername = null;
            string? findingProjectTitle = null;
            int? effort = null;
            string bugOrFeature;
            string status;
            string priority;
            string usernameOfChanger;
            string passwordOfChanger;
            bool isUserExist;
            bool breakWhile = false;
            bool breakWhileOuter = false;
            User currentUser = getCurrentUser();
            string path;
            string choosePriority = "1. Ascending";
            bool flag = false;
            int typeIdtoOrder = 1;

            AnsiConsole.Write(new FigletText("Issue Tracker")
                .Centered()
                .Color(Color.Orange1));


            var app = new CommandApp();
            while (true)
            {
                var option = AnsiConsole.Prompt(
                   new SelectionPrompt<string>()
                       .Title("\n[green]Select an operation:[/]")
                       .PageSize(11)
                       .AddChoices(new[] {
                        "1. Register",
                        "2. Login"
                       }));
                switch (option)
                {
                    case "1. Register":
                        username = AnsiConsole.Ask<string>("Enter the username:");
                        password = AnsiConsole.Prompt(
                            new TextPrompt<string>("Enter the password:")
                                .PromptStyle("red")
                                .Secret());
                        userService.Register(username, password);
                        break;
                    case "2. Login":
                        username = AnsiConsole.Ask<string>("Enter the username:");
                        password = AnsiConsole.Prompt(
                            new TextPrompt<string>("Enter the password:")
                                .PromptStyle("red")
                                .Secret());
                        bool loggedIn = userService.Login(username, password);
                        if (!loggedIn)
                        {
                            _logging.Create("User login failed.", new Dictionary<string, object> { { "Username", username } });
                            AnsiConsole.MarkupLine("\n[red]Login failed. Please check your username and password.[/]");
                        }
                        else {
                            this.existUser = userService.existUser;
                            AnsiConsole.MarkupLine($"\n[green]Successfully logged in. Welcome {this.existUser.Username}.[/]");

                        }
                        if (this.existUser != null)
                        {
                            flag = true;
                        }
                        break;
                }
                if(flag)
                {
                    flag = false;
                    break;
                }
            }

                while (true)
                {
                    if (this.existUser != null)
                    {
                        var optionAfterLogin = AnsiConsole.Prompt(
                               new SelectionPrompt<string>()
                                   .Title("\n[green]Select an operation:[/]")
                                   .PageSize(11)
                                   .AddChoices(new[] {
                        "1. Add Project",
                        "2. Update Project",
                        "3. Create Issue",
                        "4. Edit Issue",
                        "5. Delete Issue",
                        "6. Delete Project",
                        "7. List Projects",
                        "8. List Issues",
                        "9. Edit User",
                        "10. Delete User",
                        "11. Exit"
                                   }));
                        switch (optionAfterLogin)
                        {
                            case "1. Add Project":
                                if (!string.IsNullOrEmpty(this.existUser.Username))
                                {
                                    projectTitle = AnsiConsole.Ask<string>("Enter the project title:");
                                    projectDescription = AnsiConsole.Ask<string>("Enter the project description:");
                                    projectService.CreateProject(projectTitle, projectDescription, userService);
                                }
                                else
                                {
                                _logging.Create("You must login first to add a project!", new Dictionary<string, object> { { "Error", "User not logged in." } });
                                AnsiConsole.MarkupLine("[bold red]You must login first to add a project![/]");
                                }
                                break;
                            case "2. Update Project":
                                if (!string.IsNullOrEmpty(this.existUser.Username))
                                {
                                    findingProjectTitle = AnsiConsole.Ask<string>("Enter the project title of project you want to edit:");
                                    bool approve = AnsiConsole.Confirm("Do you want to update the project title? (y/n)", false);
                                    if (approve)
                                    {
                                        projectTitle = AnsiConsole.Ask<string>("Enter the new project title:");
                                    }
                                    else
                                    {
                                        projectTitle = null;
                                    }
                                    approve = AnsiConsole.Confirm("Do you want to update the project description? (y/n)", false);
                                    if (approve)
                                    {
                                        projectDescription = AnsiConsole.Ask<string>("Enter the new project description:");
                                    }
                                    else
                                    {
                                        projectDescription = null;
                                    }
                                    projectService.UpdateProject(findingProjectTitle, projectTitle, projectDescription, userService);
                                }
                                else
                                {
                                _logging.Create("User tried to update a project without being logged in.", new Dictionary<string, object> { { "Error", "User not logged in." } });
                                AnsiConsole.MarkupLine("[bold red]User tried to update a project without being logged in.[/]");
                                }
                                break;
                            case "3. Create Issue":
                                if (!string.IsNullOrEmpty(this.existUser.Username))
                                {
                                    issueTitle = AnsiConsole.Ask<string>("Enter the title of issue:");
                                    issueDescription = AnsiConsole.Ask<string>("Enter the description of issue:");
                                    bugOrFeature = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                        .Title("\n[green]What is the type?:[/]")
                                        .PageSize(6)
                                        .AddChoices(new[] {
                        "1. Bug",
                        "2. Feature",
                                        }));

                                    switch (bugOrFeature)
                                    {
                                        case "1. Bug":
                                            type = 1;
                                            break;
                                        case "2. Feature":
                                            type = 2;
                                            break;
                                        default:
                                            type = 1;
                                            break;
                                    }
                                    status = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                        .Title("\n[green]What is the status? :[/]")
                                        .PageSize(6)
                                        .AddChoices(new[] {
                        "1. Proposed",
                        "2. Active",
                        "3. Pending Deployment",
                        "4. Resolved",
                                        }));

                                    switch (status)
                                    {
                                        case "1. Proposed":
                                            statusId = 1;
                                            break;
                                        case "2. Active":
                                            statusId = 2;
                                            break;
                                        case "3. Pending Deployment":
                                            statusId = 3;
                                            break;
                                        case "4. Resolved":
                                            statusId = 4;
                                            break;
                                        default:
                                            statusId = 1;
                                            break;
                                    }

                                    priority = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                        .Title("\n[green]What is the priority? (örn: Proposed,Active,Pending Deployment,Resolved):[/]")
                                        .PageSize(6)
                                        .AddChoices(new[] {
                        "1. Low",
                        "2. Medium",
                        "3. High"
                                        }));

                                    switch (priority)
                                    {
                                        case "1. Low":
                                            priorityId = 1;
                                            break;
                                        case "2. Medium":
                                            priorityId = 2;
                                            break;
                                        case "3. High":
                                            priorityId = 3;
                                            break;
                                        default:
                                            priorityId = 1;
                                            break;
                                    }

                                    assignee = AnsiConsole.Ask<string>("Entert the username of assignee:");

                                    project = AnsiConsole.Ask<string>("Enter the name of project to connect:");

                                    effort = AnsiConsole.Ask<int>("Enter the effort of issue as hour:");
                                    issueService.CreateIssue(issueTitle, issueDescription, (int)type, (int)statusId, (int)priorityId, assignee, existUser.Username, (int)effort, project, userService);

                                }
                                else
                                {
                                _logging.Create("You must login first to create an issue!", new Dictionary<string, object> { { "Error", "User not logged in." } });
                                AnsiConsole.MarkupLine("[bold red]You must login first to create an issue![/]");
                                }
                                break;
                            case "4. Edit Issue":
                                if (!string.IsNullOrEmpty(this.existUser.Username))
                                {
                                    findingTitle = AnsiConsole.Ask<string>("Enter the issue title to change:");

                                    var editIssue = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                        .Title("\n[green]Bir işlem seçin:[/]")
                                        .PageSize(8)
                                        .AddChoices(new[] {
                        "1. Edit Title",
                        "2. Edit Description",
                        "3. Edit Type",
                        "4. Edit Status",
                        "5. Edit Priority",
                        "6. Edit Assignee",
                        "7. Edit Project",
                        "8. Edit Effort"
                                        }));
                                    switch (editIssue)
                                    {
                                        case "1. Edit Title":
                                            issueTitle = AnsiConsole.Ask<string>("Enter the new issue title:");
                                            if (string.IsNullOrEmpty(issueTitle))
                                                issueTitle = null;
                                            break;
                                        case "2. Edit Description":
                                            issueDescription = AnsiConsole.Ask<string>("Enter the new issue description:");
                                            if (string.IsNullOrEmpty(issueDescription))
                                                issueDescription = null;
                                            break;
                                        case "3. Edit Type":
                                            bugOrFeature = AnsiConsole.Prompt(
                                            new SelectionPrompt<string>()
                                                .Title("\n[green]What is the type? [/]")
                                                .PageSize(6)
                                                .AddChoices(new[] {
                        "1. Bug",
                        "2. Feature",
                                                }));

                                            switch (bugOrFeature)
                                            {
                                                case "1. Bug":
                                                    type = 1;
                                                    break;
                                                case "2. Feature":
                                                    type = 2;
                                                    break;
                                                default:
                                                    type = 1;
                                                    break;

                                            }
                                            if (type != 1 && type != 2)
                                                type = 0;
                                            break;
                                        case "4. Edit Status":
                                            status = AnsiConsole.Prompt(
                                                new SelectionPrompt<string>()
                                                    .Title("\n[green]What is the status? :[/]")
                                                    .PageSize(6)
                                                    .AddChoices(new[] {
                        "1. Proposed",
                        "2. Active",
                        "3. Pending Deployment",
                        "4. Resolved",
                                                    }));

                                            switch (status)
                                            {
                                                case "1. Proposed":
                                                    statusId = 1;
                                                    break;
                                                case "2. Active":
                                                    statusId = 2;
                                                    break;
                                                case "3. Pending Deployment":
                                                    statusId = 3;
                                                    break;
                                                case "4. Resolved":
                                                    statusId = 4;
                                                    break;
                                                default:
                                                    statusId = 1;
                                                    break;
                                            }
                                            if (statusId != 1 && statusId != 2 && statusId != 3 && statusId != 4)
                                                statusId = 0;
                                            break;
                                        case "5. Edit Priority":
                                            priority = AnsiConsole.Prompt(
                                                new SelectionPrompt<string>()
                                                    .Title("\n[green]What is the priority? :[/]")
                                                    .PageSize(6)
                                                    .AddChoices(new[] {
                        "1. Low",
                        "2. Medium",
                        "3. High"
                                                    }));

                                            switch (priority)
                                            {
                                                case "1. Low":
                                                    priorityId = 1;
                                                    break;
                                                case "2. Medium":
                                                    priorityId = 2;
                                                    break;
                                                case "3. High":
                                                    priorityId = 3;
                                                    break;
                                                default:
                                                    priorityId = 1;
                                                    break;
                                            }
                                            if (priorityId != 1 && priorityId != 2 && priorityId != 3)
                                                priorityId = 0;
                                            break;
                                        case "6. Edit Assignee":
                                            assignee = AnsiConsole.Ask<string>("Enter the assignee user:");
                                            if (string.IsNullOrEmpty(assignee))
                                            {
                                                assignee = null;
                                            }
                                            break;
                                        case "7. Edit Project":
                                            project = AnsiConsole.Ask<string>("Enter the project:");
                                            if (string.IsNullOrEmpty(project))
                                                project = null;
                                            break;
                                        case "8. Edit Effort":
                                            effort = AnsiConsole.Ask<int>("Enter the effort for issue (as hour):");
                                            if (effort <= 0)
                                                effort = 0;
                                            break;
                                    }
                                    issueService.UpdateIssue(findingTitle, issueTitle, issueDescription, type, statusId, priorityId, assignee, existUser.Username, effort, project);
                                assignee = null;
                                effort = null;
                                issueTitle = null;
                            }
                                else
                                {
                                _logging.Create("You must login first to edit an issue!", new Dictionary<string, object> { { "Error", "User not logged in." } });
                                AnsiConsole.MarkupLine("[bold red]You must login first to edit an issue![/]");
                                }
                                break;
                            case "5. Delete Issue":
                                if (!string.IsNullOrEmpty(this.existUser.Username))
                                {
                                    findingTitle = AnsiConsole.Ask<string>("Enter the title of issue to delete:");
                                    issueService.DeleteIssue(findingTitle);
                                }
                                else
                                {
                                _logging.Create("You must login first to delete an issue!", new Dictionary<string, object> { { "Error", "User not logged in." } });
                                AnsiConsole.MarkupLine("[bold red]You must login first to delete an issue![/]");
                                }
                                break;
                            case "6. Delete Project":
                                if (!string.IsNullOrEmpty(this.existUser.Username))
                                {
                                    projectTitle = AnsiConsole.Ask<string>("Enter the title of project to delete:");
                                    projectService.DeleteProject(projectTitle);
                                }
                                else
                                {
                                _logging.Create("You must login first to delete a project!", new Dictionary<string, object> { { "Error", "User not logged in." } });
                                AnsiConsole.MarkupLine("[bold red]You must login first to delete a project![/]");
                                }
                                break;
                            case "7. List Projects":
                                if (!string.IsNullOrEmpty(this.existUser.Username))
                                {
                                    projectTitle = AnsiConsole.Prompt(new TextPrompt<string>("Enter the title of project to search:").AllowEmpty());
                                    assignee = AnsiConsole.Prompt(new TextPrompt<string>("Enter the user of project to search:").AllowEmpty());
                                    if (string.IsNullOrEmpty(projectTitle))
                                    {
                                        projectTitle = null;
                                    }
                                    if (string.IsNullOrEmpty(assignee))
                                    {
                                        assignee = null;
                                    }
                                    List<Project> projects = (List<Project>)projectService.GetAllProjects(assignee, projectTitle);
                                    //if (projects.Count > 0)
                                    //{
                                    //    AnsiConsole.MarkupLine("[bold green]Projects:[/]");
                                    //    //foreach (var proj in projects)
                                    //    //{
                                    //    //    AnsiConsole.MarkupLine($"[bold blue]Title:[/] {proj.Title} [bold yellow]Description:[/] {proj.Description}");
                                    //    //}
                                    //    path = AnsiConsole.Ask<string>("Enter path you want to export:");
                                    //    jsonExport.ExportIssuesToJsonAsync(path, projects);
                                    //}
                                    if(projects.Count==0)
                                    {
                                    _logging.Create("No projects found.", new Dictionary<string, object> { { "Error", "There is not any projects to show." } });
                                    AnsiConsole.MarkupLine("[bold red]No projects found![/]");
                                }
                                else
                                {
                                    path = AnsiConsole.Ask<string>("Enter path you want to export:");
                                    jsonExport.ExportIssuesToJsonAsync(path, projects);
                                }
                            }
                                else
                                {
                                _logging.Create("You must login first to list projects!", new Dictionary<string, object> { { "Error", "User not logged in." } });
                                AnsiConsole.MarkupLine("[bold red]You must login first to list projects![/]");
                                }
                                break;
                            case "8. List Issues":
                                List<Issue> issues = new List<Issue>();
                            if (!string.IsNullOrEmpty(this.existUser.Username))
                            {
                                
                                
                                while (true)
                                {
                                    breakWhile = false;
                                    
                                    while (true)
                                {
                                        string choose = AnsiConsole.Prompt(
                                            new SelectionPrompt<string>()
                                                .Title("\n[green]Select the value you want :[/]")
                                                .PageSize(8)
                                                .AddChoices(new[] {
                        "0. Filter By Username",
                        "1. Filter by Issue Title",
                        "2. Filter By Type",
                        "3. Order By Type",
                        "4. Filter By Status",
                        "5. Filter by Priority",
                        "6. Order by Priority",
                        "7. Get All Issues",
                        "8. Export Issues to Json",
                                                }));
                                        if (breakWhile)
                                    {
                                        break;
                                    }
                                    switch (choose)
                                    {
                                        case "0. Filter By Username":
                                                breakWhile = true;
                                                string usernameToFilter = AnsiConsole.Ask<string>("Enter the username to filter issues:");

                                            //if (userService.IsUserExist(usernameToFilter, null))
                                            //{
                                            //    usernameToFilter = this.existUser.Username;
                                            //}
                                            //else
                                            //{
                                            //    AnsiConsole.MarkupLine("[bold red]Username to filter does not exist![/]");
                                            //}
                                            issues = issueService.GetAllIssues(issues, usernameToFilter, null, null, null, null, null, null);
                                            break;
                                        case "1. Filter by Issue Title":
                                                breakWhile = true;
                                                issueTitle = AnsiConsole.Ask<string>("Enter the title of issue to search:");
                                            issues = issueService.GetAllIssues(issues, this.existUser.Username, issueTitle, null, null, null, null, null);
                                            break;
                                        case "2. Filter By Type":
                                                breakWhile = true;
                                                bugOrFeature = AnsiConsole.Prompt(
                                            new SelectionPrompt<string>()
                                                .Title("\n[green]What is the type?:[/]")
                                                .PageSize(6)
                                                .AddChoices(new[] {
                        "1. Bug",
                        "2. Feature",
                                                }));

                                            switch (bugOrFeature)
                                            {
                                                case "1. Bug":
                                                    type = 1;
                                                    break;
                                                case "2. Feature":
                                                    type = 2;
                                                    break;
                                            }
                                            issues = issueService.GetAllIssues(issues, this.existUser.Username, null, type, null, null, null, null);
                                            break;
                                        case "3. Order By Type":
                                                breakWhile = true;
                                                var orderByType = AnsiConsole.Prompt(
                                                new SelectionPrompt<string>()
                                                    .Title("\n[green]Select the type you want to order:[/]")
                                                    .PageSize(6)
                                                    .AddChoices(new[] {
                                                    "1. Bug first",
                                                    "2. Feature first"
                                                    }));
                                            if (orderByType == "1. Bug first")
                                            {
                                                typeIdtoOrder = 1;

                                            }
                                            else if (orderByType == "2. Feature first")
                                            {
                                                typeIdtoOrder = 2;
                                            }
                                            issues = issueService.GetAllIssues(issues, null, null, null, typeIdtoOrder, null, null, null);
                                            break;
                                        case "4. Filter By Status":
                                                breakWhile = true;
                                                status = AnsiConsole.Prompt(
                                                new SelectionPrompt<string>()
                                                    .Title("\n[green]What is the status? :[/]")
                                                    .PageSize(6)
                                                    .AddChoices(new[] {
                        "1. Proposed",
                        "2. Active",
                        "3. Proposed",
                        "4. Active",
                                                    }));

                                            switch (status)
                                            {
                                                case "1. Proposed":
                                                    statusId = 1;
                                                    break;
                                                case "2. Active":
                                                    statusId = 2;
                                                    break;
                                                case "3. Pending Deployment":
                                                    statusId = 3;
                                                    break;
                                                case "4. Resolved":
                                                    statusId = 4;
                                                    break;
                                            }
                                            issues = issueService.GetAllIssues(issues, null, null, null, null, null, null, statusId);
                                            break;
                                        case "5. Filter by Priority":
                                                breakWhile = true;
                                                var priorityToFilter = AnsiConsole.Prompt(
                                                        new SelectionPrompt<string>()
                                                            .Title("\n[green]Select the priority you want to order:[/]")
                                                            .PageSize(6)
                                                            .AddChoices(new[] {
                                                            "1. Low",
                                                            "2. Medium",
                                                            "3. High"
                                                            }));


                                            break;
                                        case "6. Order by Priority":
                                                breakWhile = true;
                                                choosePriority = AnsiConsole.Prompt(
                                                    new SelectionPrompt<string>()
                                                        .Title("\n[green]Select the priority you want to order:[/]")
                                                        .PageSize(6)
                                                        .AddChoices(new[] {
                        "1. Ascending",
                        "2. Descending",
                                                        }));
                                            issues = issueService.GetAllIssues(issues, null, null, null, null, null, choosePriority == "1. Ascending" ? true : false, null);
                                            break;
                                        case "7. Get All Issues":
                                            issues = issueService.GetAllIssues(issues, null, null, null, null, null, null, null);
                                                breakWhile = true;
                                                break;
                                        case "8. Export Issues to Json":
                                            if (issues.Count == 0)
                                            {
                                                _logging.Create("No issues found.", new Dictionary<string, object> { { "Error", "There is not any issues to show." } });
                                                AnsiConsole.MarkupLine("[red]No issues found![/]");
                                            }
                                            else
                                            {

                                                path = AnsiConsole.Ask<string>("Enter path you want to export:");
                                                jsonExport.ExportIssuesToJsonAsync(path, issues);
                                            }

                                                breakWhileOuter = true;    
                                            break;
                                    }


                                    break;
                                }
                                    if (breakWhileOuter)
                                    {
                                        break;
                                    }
                            }
                            }
                            break;                                
                            case "9. Edit User":
                                usernameOfChanger = AnsiConsole.Ask<string>("Enter your username to edit user:");
                                passwordOfChanger = AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter your password:")
                                        .PromptStyle("red")
                                        .Secret());
                                if (userService.IsAdmin(usernameOfChanger, passwordOfChanger))
                                {
                                    username = AnsiConsole.Ask<string>("Enter the username of user to delete:");
                                    newUsername = AnsiConsole.Ask<string>("Enter the new user's username:");
                                    password = AnsiConsole.Prompt(
                                        new TextPrompt<string>("Enter the new user's password:")
                                            .PromptStyle("red")
                                            .Secret());
                                    userService.Update(username, newUsername, password);
                                }
                                else
                                {
                                _logging.Create("You can't update any user because of your priviliege!", new Dictionary<string, object> { { "Error", "User not admin." } });
                                AnsiConsole.MarkupLine("[bold red]You can't update any user because of your priviliege![/]");
                                }
                                break;
                            case "10. Delete User":
                                usernameOfChanger = AnsiConsole.Ask<string>("Enter your username to edit user:");
                                passwordOfChanger = AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter your password:")
                                        .PromptStyle("red")
                                        .Secret());
                                if (userService.IsAdmin(usernameOfChanger, passwordOfChanger))
                                {
                                    username = AnsiConsole.Ask<string>("Enter the username of user to delete:");
                                    bool isDeleted = userService.Delete(username);
                                    if (isDeleted)
                                    {
                                    _logging.Create("User deleted successfully!", new Dictionary<string, object> { { "DeletedAt", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") } });
                                        AnsiConsole.MarkupLine("[bold green]User deleted successfully![/]");
                                    }
                                    else
                                    {
                                        _logging.Create("User could not be deleted because of user not exist or already deleted!", new Dictionary<string, object> { { "Error", "User not exist or already deleted." } });
                                        AnsiConsole.MarkupLine("[bold red]User could not be deleted because of user not exist or already deleted![/]");
                                    }
                                }
                                else
                                {
                                _logging.Create("You can't delete any user because of your priviliege!", new Dictionary<string, object> { { "Error", "User not admin." } });
                                AnsiConsole.MarkupLine("[bold red]You can't delete any user because of your priviliege![/]");
                                }
                                break;
                            case "11. Exit":
                                AnsiConsole.MarkupLine("[bold red]Exiting...[/]");
                                break;
                        }
                    }
                }

        }
    }
}