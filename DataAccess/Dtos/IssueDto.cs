namespace IssueTracker.DataAccess.Dtos
{
    public record IssueDto(
    int Id,
    int UserId,
    int AssigneeId,
    string Title,
    string Description,
    string Type,
    int StatusId,
    int? Priority,
    int CreatedBy,
    int Effort,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int Status,
    ProjectDto Project
);
}
