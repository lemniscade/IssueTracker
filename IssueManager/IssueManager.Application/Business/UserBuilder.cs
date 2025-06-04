using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.IssueManager.Application.Business
{
    public class UserBuilder
    {
        public string Name { get; set; }
        public UserBuilder(string name) => Name = name;
    }
}
