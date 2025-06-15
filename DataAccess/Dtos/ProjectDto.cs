namespace IssueTracker.DataAccess.Dtos
{
    public record ProjectDto(
     int Id,
     string Name,
     string Description,
     int OwnerId,
     DateTime CreatedAt
 );
}
