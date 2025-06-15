using FluentValidation;
using IssueTracker.Entity.Models;

namespace IssueTracker.Business.Validations
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        public ProjectValidator() { 
            RuleFor(project => project.Title)
                .NotEmpty().WithMessage("Project name is required.")
                .Length(3, 50).WithMessage("Project name must be between 3 and 50 characters.");
            RuleFor(project => project.Description)
                .NotEmpty().WithMessage("Project description is required.")
                .Length(10, 500).WithMessage("Project description must be between 10 and 500 characters.");
            RuleFor(project => project.CreatedBy)
                .NotEmpty().WithMessage("Project must have a creator.")
                .Must(user => user != null).WithMessage("Creator must be a valid user.");
            RuleFor(project => project.Assignee)
                .Must(user => user != null).WithMessage("Project must have an assignee.")
                .WithMessage("Assignee must be a valid user.");
            RuleFor(project => project.CreatedAt)
                .NotEmpty().WithMessage("Project creation date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Project creation date cannot be in the future.");
        }
    }
}
