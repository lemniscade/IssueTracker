using IssueManager.IssueManager.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.IssueManager.Domain.ValueObjects
{
    public record CreatedById
    {
        public Guid Value { get; }
        public CreatedById() { }
        private CreatedById(Guid value) => Value = value;
        public static CreatedById Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("Issue cannot be empty.");
            }

            return new CreatedById(value);
        }
    }
}