namespace IssueManager.IssueManager.Domain.Models;
public class IssueStatus
    {
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; }=default!;
    public int StatusEnumNo { get; set; }
    }
