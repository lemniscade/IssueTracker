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
            RuleFor(issue => issue.IssueUsers)
            .NotNull().NotEmpty().Must(iu => iu.Any(x => x.UserTypeEnumId == 3))
            .WithMessage("At least one Assigned user is required.");
            RuleFor(issue => issue.Effort).NotEmpty().WithMessage("Effort is required.")
                .GreaterThanOrEqualTo(0).LessThan(60).WithMessage("Effort must be betwwen 0 and 60 hours.");


        }
    }
}
