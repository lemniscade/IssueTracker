using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.IssueManager.Domain.ValueObjects
{
    public class AssigneeId
    {
        public Guid Value { get; }
        public AssigneeId() { }
        private AssigneeId(Guid value) => Value = value;
        public static AssigneeId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("Issue cannot be empty.");
            }

            return new AssigneeId(value);
        }
    }
}
