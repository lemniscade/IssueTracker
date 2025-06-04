using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.IssueManager.Application.Business
{
    public class Bug : IIssue
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Status { get; private set; }
        public int Priority { get; private set; }
        public UserBuilder Assignee { get; set; }

        private Bug() { }

        public void Display()
        {
            Console.WriteLine($"[BUG] {Title} - {Description} | Status: {Status} | Priority: {Priority} | Assignee: {Assignee?.Name ?? "Unassigned"}");
        }

        public IIssue ChangePriority(int newPriority)
        {
            Priority = newPriority;
            return this;
        }

        public IIssue ChangeAssignee(UserBuilder newAssignee)
        {
            Assignee = newAssignee;
            return this;
        }

        public IIssue ChangeStatus(int newStatus)
        {
            Status = newStatus;
            return this;
        }

        public IIssue ChangeTitle(string title)
        {
            Title = title;
            return this;
        }

        public IIssue ChangeDescription(string description)
        {
            Description = description;
            return this;
        }

        public static BugBuilder CreateBuilder() => new BugBuilder();

        public class BugBuilder
        {
            private readonly Bug _bug = new Bug();

            public BugBuilder WithTitle(string title)
            {
                _bug.Title = title;
                return this;
            }

            public BugBuilder WithDescription(string description)
            {
                _bug.Description = description;
                return this;
            }

            public BugBuilder WithStatus(int status)
            {
                _bug.Status = status;
                return this;
            }

            public BugBuilder WithPriority(int priority)
            {
                _bug.Priority = priority;
                return this;
            }

            public BugBuilder WithAssignee(UserBuilder assignee)
            {
                _bug.Assignee = assignee;
                return this;
            }

            public Bug Build()
            {
                return _bug;
            }
        }
    }


}
