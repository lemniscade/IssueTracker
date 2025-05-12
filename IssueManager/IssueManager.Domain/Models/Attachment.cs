namespace IssueManager.Domain.Models;
public class Attachment
{
    public int Id { get; set; }
    public int IssueId { get; set; }
    public string FileName { get; set; } = default!;
    public string FilePath { get; set; } = default!;
    public int UploadedBy { get; set; }
    public DateTime UploadedAt { get; set; }
    public string CloudinaryUrl { get; set; } = default!;
    public Issue Issue { get; set; } = default!;
    public User Uploader { get; set; } = default!;
}