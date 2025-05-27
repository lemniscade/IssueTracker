

namespace IssueManager.IssueManager.Application.Dtos;
public record IssueDto(
    Guid Id,
    Guid UserId,
    Guid AssigneeId,
    string Title,
    string Description,
    string Type,
    int StatusId,
    int? Priority,
    Guid CreatedBy,
    DateTime? DueDate,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int Status,
    ProjectDto Project
);

