

namespace IssueManager.IssueManager.Application.Dtos;
public record IssueDto(
    int Id,
    string Title,
    string Description,
    string Type,
    int StatusId,
    int? Priority,
    int ProjectId,
    int CreatedBy,
    int? AssignedTo,
    DateTime? DueDate,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int Status,
    User Creator,
    User? Assignee,
    Task Project
);

