using IssueManager.IssueManager.Domain.ValueObjects;

namespace IssueManager.IssueManager.Domain.Models;
public class Issue:Aggregate<IssueId>
{
    public UserId UserId { get; set; } = default!;

    public UserId? AssigneeId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Type { get; set; } = default!;
    public int StatusId { get; set; }
    public int Priority { get; set; } = default!;
    public new required UserId CreatedBy { get; set; }
    public DateTime? DueDate { get; set; }
    public new DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Status { get; set; } = default!;
    public Task Project { get; set; } = default!;

    //public static Issue Create(IssueId id, UserId userId, string issueTitle, string issueDescription, string type, int statusId, int priority, UserId createdBy, DateTime dueDate, DateTime createdAt, DateTime updatedAt, int status, Task project)
    //{
    //    var issue = new Issue
    //    {
    //        Id = id,
    //        UserId = userId,
    //        Title = issueTitle,
    //        Description = issueDescription,
    //        Type = type,
    //        StatusId = statusId,
    //        Priority = priority,
    //        CreatedBy = createdBy,
    //        DueDate = dueDate,
    //        CreatedAt = createdAt,
    //        UpdatedAt = updatedAt,
    //        Status = status,
    //        Project = project
    //    };
    //    return issue;
    //}

    //public void Update(string issueTitle, string issueDescription, string type, int statusId, int priority, DateTime dueDate, DateTime updatedAt, int status, Task project)
    //{
    //    Title = issueTitle;
    //    Description = issueDescription;
    //    Type = type;
    //    StatusId = statusId;
    //    Priority = priority;
    //    DueDate = dueDate;
    //    UpdatedAt = updatedAt;
    //    Status = status;
    //    Project = project;
    //}

    //public void Add(ProductId productId, int quantity, decimal price)
    //{
    //    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
    //    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

    //    var issueItem = new IssueItem(Id, productId, quantity, price);
    //    _issueItems.Add(issueItem);
    //}

    //public void Remove(ProductId productId)
    //{
    //    var issueItem = _issueItems.FirstOrDefault(x => x.ProductId == productId);
    //    if (issueItem is not null)
    //    {
    //        _issueItems.Remove(issueItem);
    //    }
    //}
}