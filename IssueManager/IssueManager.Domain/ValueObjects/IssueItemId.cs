using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.IssueManager.Domain.ValueObjects
{
    public record IssueItemId
    {
        public Guid Value { get; }
        private IssueItemId(Guid value) => Value = value;
        public static IssueItemId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("Issue cannot be empty.");
            }

            return new IssueItemId(value);
        }
    }
}
