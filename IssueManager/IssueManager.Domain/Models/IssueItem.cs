using IssueManager.IssueManager.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.IssueManager.Domain.Models
{
    public class IssueItem:Entity<IssueItemId>
    {
        public IssueItem(IssueId issueId,string title,string description)
        {
            Id = IssueItemId.Of(Guid.NewGuid());
            IssueId=issueId;
            Title = title;
            Description = description;
        }

        public IssueId IssueId { get; set; } = default!;
        public string Title { get; set; }=default!;
        public string Description { get; set; } = default!;
    }
}
