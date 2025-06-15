using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repositories
{
    public interface IUserRepository
    {
        bool Create(string username, string password);
        bool Update(string oldUsername, string? newUsername, string? password);
        bool Delete(string username);
    }
}
