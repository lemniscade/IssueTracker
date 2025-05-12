
public class Comment
{
    public int Id { get; set; }
    public int IssueId { get; set; }
    public int UserId { get; set; }
    public string CommentText { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime ModifyAt { get; set; }
    public Issue Issue { get; set; } = default!;
    public User User { get; set; } = default!;
}