using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Business.Services
{
    public interface IUserService
    {
        bool Register(string username, string password);
        bool Login(string username, string password);
        
    }
}
