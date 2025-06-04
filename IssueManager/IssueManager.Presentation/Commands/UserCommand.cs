using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using IssueManager.IssueManager.Application.Interceptor;

namespace IssueManager.IssueManager.Presentation.Commands
{
    public class UserCommand: Command<UserCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandOption("--name <Username>")]
            public string Username { get; set; }

            [CommandOption("--pass <Password>")]
            public string Password { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            UserOperations.Register(settings.Username, settings.Password);
            UserOperations.Login(settings.Username, settings.Password);
            return 0;
        }
    }
}
