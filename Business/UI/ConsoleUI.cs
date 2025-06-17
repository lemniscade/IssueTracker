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
            string projectTitle;
            string projectDescription;
            int type;
            int? statusId;
            int? priorityId;
            string assignee;
            DateTime? assignedAt;
            User createdBy;
            DateTime? createdAt;
            DateTime? dueDate;
            string project;
            string issueTitle;
            string issueDescription;
            string findingTitle;
            string newUsername;
            string findingProjectTitle;
            int effort;

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
                        "4. Create Issue",
                        "5. Edit Issue",
                        "5. Add Issue",
                        "6. Edit Issue"
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
                        if(approve){
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

                        projectService.UpdateProject(findingProjectTitle,projectTitle, projectDescription, assignee);
                        break;
                    case "4. Create Issue":
                        issueTitle = AnsiConsole.Ask<string>("Enter the title of issue:");
                        issueDescription = AnsiConsole.Ask<string>("Enter the description of issue:");
                        var bugOrFeature = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("\n[green]What is the type of issue? (ex: Bug, Feature):[/]")
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
                            .Title("\n[green]Wht is the status? (ex: Proposed,Active,Pending Deployment,Resolved):[/]")
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

                        issueService.CreateIssue(issueTitle, issueDescription, type, (int)statusId, (int)priorityId, assignee, usernameOfExistUser, effort, project);

                        break;
                    case "5. Edit Issue":
                        findingTitle = AnsiConsole.Ask<string>("Değiştirilecek issue başlığını girin:");
                        approve = AnsiConsole.Confirm("Do you want to update the project title? (y/n)", false);
                        if (approve)
                        {
                            issueTitle = AnsiConsole.Ask<string>("Enter the new issue title:");
                        }
                        else
                        {
                            issueTitle = null;
                        }
                        approve = AnsiConsole.Confirm("Do you want to update the issue description? (y/n)", false);
                        if (approve)
                        {
                            issueDescription = AnsiConsole.Ask<string>("Enter the new issue description:");
                        }
                        else
                        {
                            issueDescription = null;
                        }
                        approve = AnsiConsole.Confirm("Do you want to update the issue type? (y/n)", false);
                        if (approve)
                        {
                            bugOrFeature = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("\n[green]Issue tipi nedir? (örn: Bug, Feature):[/]")
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
                        }
                        else
                        {
                            type = 0;
                        }
                            status = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("\n[green]Status nedir? (örn: Proposed,Active,Pending Deployment,Resolved):[/]")
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

                        priority = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("\n[green]Status nedir? (örn: Proposed,Active,Pending Deployment,Resolved):[/]")
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

                        assignee = AnsiConsole.Ask<string>("Atanacak kullanıcı adını girin:");

                        project = AnsiConsole.Ask<string>("Proje adını girin:");
                        issueService.UpdateIssue(findingTitle, issueTitle, issueDescription, type, (int)statusId, (int)priorityId, assignee, usernameOfExistUser, 5, project);
                        break;
                    case "6. Delete Issue":
                        findingTitle = AnsiConsole.Ask<string>("Silinecek issue başlığını girin:");
                        issueService.DeleteIssue(findingTitle);
                        break;
                    case "7. Delete Project":
                        projectTitle = AnsiConsole.Ask<string>("Silinecek proje başlığını girin:");
                        projectService.DeleteProject(projectTitle);
                        break;
                    case "8. Edit User":
                        username = AnsiConsole.Ask<string>("Değiştirilecek kullancının adını girin:");
                        newUsername = AnsiConsole.Ask<string>("Yeni kullanıcı adını girin:");
                        password = AnsiConsole.Prompt(
                            new TextPrompt<string>("Şifrenizi girin:")
                                .PromptStyle("red")
                                .Secret());
                        userService.Update(username,newUsername, password);
                        break;
                    case "9. Delete User":

                    case "10. Exit":
                        AnsiConsole.MarkupLine("[bold red]Çıkılıyor...[/]");
                        break;
                }
            }
        }
    }
}