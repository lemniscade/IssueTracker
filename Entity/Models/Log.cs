using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Entity.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string JsonLog { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
