using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Entity.Models
{
    public class IssueUser
    {
        public int Id { get; set; } = default!;
        public int IssueId { get; set; }
        public Issue Issue { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int UserTypeEnumId { get; set; }
    }

}
