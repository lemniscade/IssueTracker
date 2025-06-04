using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.IssueManager.Application.Business
{
    public interface IIssue
    {
        string Title { get; }
        string Description { get; }
        int Status { get; }
        int Priority { get; }
        UserBuilder Assignee { get; set; }

        void Display();
        IIssue ChangePriority(int newPriority);

        IIssue ChangeAssignee(UserBuilder newAssignee);

        IIssue ChangeStatus(int newStatus);

        IIssue ChangeTitle(string title);

        IIssue ChangeDescription(string description);
    }

}
