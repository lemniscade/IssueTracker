namespace IssueManager.IssueManager.Application.Dtos
{
    public record ProjectDto(
     Guid Id,
     string Name,
     string Description,
     Guid OwnerId,
     DateTime CreatedAt
 );

}
