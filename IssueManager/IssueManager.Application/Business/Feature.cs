using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.IssueManager.Application.Business
{
    public class Feature : IIssue
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Status { get; private set; }
        public int Priority { get; private set; }
        public UserBuilder Assignee { get; set; }

        private Feature() { }

        public void Display()
        {
            Console.WriteLine($"[FEATURE] {Title} - {Description} | Status: {Status} | Priority: {Priority} | Assignee: {Assignee?.Name ?? "Unassigned"}");
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

        public static FeatureBuilder CreateBuilder() => new FeatureBuilder();

        public class FeatureBuilder
        {
            private readonly Feature _feature = new Feature();

            public FeatureBuilder WithTitle(string title)
            {
                _feature.Title = title;
                return this;
            }

            public FeatureBuilder WithDescription(string description)
            {
                _feature.Description = description;
                return this;
            }

            public FeatureBuilder WithStatus(int status)
            {
                _feature.Status = status;
                return this;
            }

            public FeatureBuilder WithPriority(int priority)
            {
                _feature.Priority = priority;
                return this;
            }

            public FeatureBuilder WithAssignee(UserBuilder assignee)
            {
                _feature.Assignee = assignee;
                return this;
            }

            public Feature Build()
            {
                return _feature;
            }
        }
    }

}
