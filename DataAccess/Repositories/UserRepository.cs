using IssueTracker.Entity;
using IssueTracker.Entity.Models;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User user;
        public bool Create(string username, string password)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = new User
                {
                    Username = username,
                    Password = HashPassword(password)
                };
                context.Users.Add(user);
                return context.SaveChanges() > 0;
            }
        }

        public bool Update(string oldUsername, string newUsername, string password)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == oldUsername);
                if (user != null)
                {
                    user.Username = newUsername;
                    user.Password = HashPassword(password);
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

        public bool IsUserExist(string username, string password)
        {
            string passwordHash = HashPassword(password);
            using (var context = new ApplicationDbContext())
            {
                User user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == passwordHash);
                this.user = user;
                if (user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private string HashPassword(string password)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, 16, 10000))
            {
                var salt = deriveBytes.Salt;
                var hash = deriveBytes.GetBytes(32);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
