namespace IssueManager.IssueManager.Application.Extensions;
public static class IssueExtensions
{
    public static IEnumerable<IssueDto> ToIssueDtoList(this IEnumerable<Issue> issues)
    {
        return issues.Select(issue => new IssueDto(
            Id: issue.Id.Value,
            UserId: issue.UserId.Value,
            AssigneeId: issue.AssigneeId?.Value ?? Guid.Empty,  
            Title: issue.Title,
            Description: issue.Description,
            Type: issue.Type,
            StatusId: issue.StatusId,
            Priority: issue.Priority,
            CreatedBy: issue.CreatedBy.Value,
            DueDate: issue.DueDate,
            CreatedAt: issue.CreatedAt,
            UpdatedAt: issue.UpdatedAt,
            Status: issue.Status,
            Project: new ProjectDto(
                issue.Project.ProjectId.Value,
                issue.Project.Name,
                issue.Project.Description,
                issue.Project.OwnerId.Value,
                issue.Project.CreatedAt
            )));
    }

    public static IssueDto ToIssueDto(this Issue issue)
    {
        return DtoFromIssue(issue);
    }

    private static IssueDto DtoFromIssue(Issue issue)
    {
        return new IssueDto(
            Id: issue.Id.Value,
            UserId: issue.UserId.Value,
            AssigneeId: issue.AssigneeId?.Value ?? Guid.Empty, // Handle possible null reference  
            Title: issue.Title,
            Description: issue.Description,
            Type: issue.Type,
            StatusId: issue.StatusId,
            Priority: issue.Priority,
            CreatedBy: issue.CreatedBy.Value,
            DueDate: issue.DueDate,
            CreatedAt: issue.CreatedAt,
            UpdatedAt: issue.UpdatedAt,
            Status: issue.Status,
            Project: new ProjectDto(
                issue.Project.ProjectId.Value,
                issue.Project.Name,
                issue.Project.Description,
                issue.Project.OwnerId.Value,
                issue.Project.CreatedAt
            ));
    }
}
