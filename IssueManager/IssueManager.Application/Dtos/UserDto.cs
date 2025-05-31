namespace IssueManager.IssueManager.Application.Dtos;
public record UserDto(
    int Id,
    string Name,
    string Email,
    int RoleId,
    bool IsActive,
    DateTime CreatedAt,
    DateTime ModifyAt,
    string ModifyBy,
    string CreatedBy
);

