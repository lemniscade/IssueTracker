public class User : Aggregate<UserId>
{
    public new int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; }=default!;
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
    public new DateTime CreatedAt { get; set; }
    public DateTime ModifyAt { get; set; }
    public string ModifyBy { get; set; } = default!;

    public string CreateBy { get;set; } = default!;
    public DateTime? LastLoginAt { get; set; }

    public Role Role { get; set; } = default!;
}