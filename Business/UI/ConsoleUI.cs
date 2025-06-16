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
        IUserRepository userRepository = new UserRepository();
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
            this.userService = new UserService(validatorForUser, userRepository);
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
                        "5. Add Issue",
                        "6. Edit Issue"
                            }));
                var app = new CommandApp();
                switch (secim)
                {
                    case "1. Register":
                        username = AnsiConsole.Ask<string>("Kullanıcı adınızı girin:");
                        password = AnsiConsole.Prompt(
                            new TextPrompt<string>("Şifrenizi girin:")
                                .PromptStyle("red")
                                .Secret());
                        userService.Register(username, password);
                        break;
                    case "2. Login":
                        username = AnsiConsole.Ask<string>("Kullanıcı adınızı girin:");
                        password = AnsiConsole.Prompt(
                            new TextPrompt<string>("Şifrenizi girin:")
                                .PromptStyle("red")
                                .Secret());
                        userService.Login(username, password);
                        this.usernameOfExistUser = userService.usernameOfExistUser;
                        break;
                    case "3. Add Project":
                        projectTitle = AnsiConsole.Ask<string>("Issue başlığını girin:");
                        projectDescription = AnsiConsole.Ask<string>("Issue açıklamasını girin:");

                        assignee = AnsiConsole.Ask<string>("Atanacak kullanıcı adını girin:");

                        project = AnsiConsole.Ask<string>("Proje adını girin:");

                        projectService.CreateProject(projectTitle, projectDescription, assignee, project);

                        break;
                    case "4. Create Issue":
                        issueTitle = AnsiConsole.Ask<string>("Issue başlığını girin:");
                        issueDescription = AnsiConsole.Ask<string>("Issue açıklamasını girin:");
                        var bugOrFeature = AnsiConsole.Prompt(
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
                        var status = AnsiConsole.Prompt(
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

                        var priority = AnsiConsole.Prompt(
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

                        issueService.CreateIssue(issueTitle, issueDescription, type, (int)statusId, (int)priorityId, assignee, usernameOfExistUser, 5, project);

                        break;
                    case "5. Edit Issue":
                        findingTitle = AnsiConsole.Ask<string>("Değiştirilecek issue başlığını girin:");
                        issueTitle = AnsiConsole.Ask<string>("Issue başlığını girin:");
                        issueDescription = AnsiConsole.Ask<string>("Issue açıklamasını girin:");
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
                    case "6. Exit":
                        AnsiConsole.MarkupLine("[bold red]Çıkılıyor...[/]");
                        break;
                }
            }
        }
    }
}