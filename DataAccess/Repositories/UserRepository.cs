using IssueTracker.Entity;
using IssueTracker.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repositories
{
    public class UserRepository:IUserRepository
    {
        public bool Create(string username,string password)
        {
            using (var context= new ApplicationDbContext())
            {
                var user = new User
                {
                    Username = username,
                    Password = password
                };
                context.Users.Add(user);
                return context.SaveChanges() > 0;
            }
        }

        public bool Update(string oldUsername, string newUsername, string password) { 
            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == oldUsername);
                if (user != null)
                {
                    user.Username = newUsername;
                    user.Password = password;
                    return context.SaveChanges() > 0;
                }
                return false;
            }
        }

        public bool Delete(string username)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    context.Users.Remove(user);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
        }
    }
}
