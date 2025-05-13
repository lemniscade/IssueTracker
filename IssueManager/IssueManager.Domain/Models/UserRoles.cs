namespace IssueManager.IssueManager.Domain.Models;
public class UserRoles
{
    public int Id { get; set; }
    public string RoleName { get; set; } = default!;
    public int RoleEnumNo { get; set; }
    public string RoleDescription { get; set; } = default!;

}

