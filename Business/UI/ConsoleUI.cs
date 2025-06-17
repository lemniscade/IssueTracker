using FluentValidation;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Business.UI
{
    public class ConsoleUI
    {
        IValidator<Issue> validatorForIssue = new IssueValidator();
        IIssueRepository issueRepository = new IssueRepository();
        IValidator<Project> validatorForProject = new ProjectValidator();
        IProjectRepository projectRepository = new ProjectRepository();
        IValidator<User> validatorForUser = new UserValidator();

        private IssueService issueService;
        private ProjectService projectService;
        private UserService userService;
        private string usernameOfExistUser;
        public ConsoleUI()
        {
            AnsiConsole.MarkupLine("[bold green]Issue Tracker Console UI[/]");
            AnsiConsole.MarkupLine("Hoş geldiniz!");
            this.issueService = new IssueService(validatorForIssue, issueRepository);
            this.projectService = new ProjectService(validatorForProject, projectRepository);
            IUserRepository userRepository = new UserRepository(validatorForUser);
            this.userService = new UserService(validatorForUser, userRepository);
        }
        public string getCurrentUser()
        {
            return this.usernameOfExistUser;
        }
        public void Execute()
        {

            string username;
            string password;
            string? projectTitle=null;
            string? projectDescription = null;
            int? type=null;
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

            while (true)
            {
                var secim = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("\n[green]Bir işlem seçin:[/]")
                            .PageSize(6)
                            .AddChoices(new[] {
                        "1. Register",
                        "2. Login",
                        "3. Add Project",
                        "4. Update Project",
                        "5. Create Issue",
                        "6. Edit Issue",
                        "7. Delete Issue",
                        "8. Delete Project",
                        "9. Edit User",
                        "10. Delete User",
                        "11. Exit"
                            }));
                var app = new CommandApp();
                switch (secim)
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
                        userService.Login(username, password);
                        this.usernameOfExistUser = userService.usernameOfExistUser;
                        break;
                    case "3. Add Project":
                        projectTitle = AnsiConsole.Ask<string>("Enter the project title:");
                        projectDescription = AnsiConsole.Ask<string>("Enter the project description:");

                        assignee = AnsiConsole.Ask<string>("Enter the the username of assignee user:");

                        projectService.CreateProject(projectTitle, projectDescription, assignee);

                        break;
                    case "4. Update Project":
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
                        approve = AnsiConsole.Confirm("Do you want to update the assignee? (y/n)", false);
                        if (approve)
                        {
                            assignee = AnsiConsole.Ask<string>("Enter the username of assignee user:");
                        }
                        else
                        {
                            assignee = null;
                        }

                        projectService.UpdateProject(findingProjectTitle, projectTitle, projectDescription, assignee);
                        break;
                    case "5. Create Issue":
                        issueTitle = AnsiConsole.Ask<string>("Enter the title of issue:");
                        issueDescription = AnsiConsole.Ask<string>("Enter the description of issue:");
                        var bugOrFeature = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("\n[green]What is the type? (ex: Bug, Feature):[/]")
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
                        var status = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("\n[green]What is the status? (ex: Proposed,Active,Pending Deployment,Resolved):[/]")
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

                        var priority = AnsiConsole.Prompt(
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

                        issueService.CreateIssue(issueTitle, issueDescription, (int)type, (int)statusId, (int)priorityId, assignee, usernameOfExistUser, (int)effort, project);

                        break;
                    case "6. Edit Issue":
                        findingTitle = AnsiConsole.Ask<string>("Enter the issue title to change:");

                        var editIssue = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("\n[green]Bir işlem seçin:[/]")
                            .PageSize(6)
                            .AddChoices(new[] {
                        "1. Edit Title",
                        "2. Edit Descrption",
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
                                    .Title("\n[green]What is the type? (örn: Bug, Feature):[/]")
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
                                        .Title("\n[green]What is the status? (ex: Proposed,Active,Pending Deployment,Resolved):[/]")
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
                                if (priorityId != 1 && priorityId != 2 && priorityId != 3)
                                    priorityId = 0;
                                break;
                            case "6. Edit Assignee":
                                assignee = AnsiConsole.Ask<string>("Enter the assignee user:");
                                if (string.IsNullOrEmpty(assignee))
                                    assignee = null;
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
                        issueService.UpdateIssue(findingTitle, issueTitle, issueDescription, type, (int)statusId, (int)priorityId, assignee, usernameOfExistUser, effort, project);
                        break;
                            case "7. Delete Issue":
                                findingTitle = AnsiConsole.Ask<string>("Enter the title of issue to delete:");
                                issueService.DeleteIssue(findingTitle);
                                break;
                            case "8. Delete Project":
                                projectTitle = AnsiConsole.Ask<string>("Enter the title of project to delete:");
                                projectService.DeleteProject(projectTitle);
                                break;
                            case "9. Edit User":
                                string usernameOfChanger = AnsiConsole.Ask<string>("Enter your username to edit user:");
                                string passwordOfChanger = AnsiConsole.Prompt(
                                    new TextPrompt<string>("Enter your password:")
                                        .PromptStyle("red")
                                        .Secret());
                                bool isUserExist = userService.Login(usernameOfChanger, passwordOfChanger);
                        if (isUserExist)
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
                            AnsiConsole.MarkupLine("[bold red]You can't update any user because of your priviliege![/]");
                        }

                            break;
                            case "10. Delete User":
                        usernameOfChanger = AnsiConsole.Ask<string>("Enter your username to edit user:");
                        passwordOfChanger = AnsiConsole.Prompt(
                            new TextPrompt<string>("Enter your password:")
                                .PromptStyle("red")
                                .Secret());
                        isUserExist = userService.Login(usernameOfChanger, passwordOfChanger);
                        if (isUserExist)
                        {
                            username = AnsiConsole.Ask<string>("Enter the username of user to delete:");
                            bool isDeleted = userService.Delete(username);
                            if (isDeleted)
                            {
                                AnsiConsole.MarkupLine("[bold green]User deleted successfully![/]");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[bold red]User could not be deleted because of user not exist or already deleted![/]");
                            }
                        }
                        else
                        {
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