using FluentValidation;
using FluentValidation.AspNetCore;
using IssueTracker.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Business.Validations
{
    public class IssueValidator : AbstractValidator<Issue>
    {
        public IssueValidator()
        {
            RuleFor(issue => issue.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 50).WithMessage("Title must be between 3 and 50 characters.");
            RuleFor(issue => issue.Description)
                .NotEmpty().WithMessage("Description is required.")
                .Length(10, 500).WithMessage("Description must be between 10 and 500 characters.");
            RuleFor(issue => issue.Effort)
                .GreaterThan(0).WithMessage("Effort must be a bigger than zero.");
            RuleFor(issue => issue.Assignee)
                .NotNull().WithMessage("Assignee is required.").ChildRules(child =>
                {
                    child.RuleFor(assignee => assignee.Username)
                        .NotEmpty().WithMessage("Assignee username is required.").Length(3,20);
                });


        }
    }
}
