using IssueManager.IssueManager.Domain.ValueObjects;
using IssueManager.IssueManager.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.IssueManager.Application.Interceptor
{
    public static class UserOperations
    {   
        
        
        private static User user;
        public static User getUser()
        {
            return user;
        }
        public static void Register(string Username,string Password)
        {
        ApplicationDbContext _context = new ApplicationDbContext();
        dynamic passwordHasher = new PasswordHasher<object>();

            User user = new User
            {
                UserId= new UserId(Guid.NewGuid()),
                Username = Username,
                Password = passwordHasher.HashPassword(null, Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

        }

        public static User Login(string username,string password)
        {
            ApplicationDbContext _context=new ApplicationDbContext();
            dynamic passwordHasher = new PasswordHasher<object>();
            User user=_context.Users.FirstOrDefault(x => x.Username == username);
            if (user != null)
            {
                var verifyResult = passwordHasher.VerifyHashedPassword(null, user.Password, password);

                if (verifyResult == PasswordVerificationResult.Success)
                {
                    return user;
                }
            }

            return null;
        }
    }
}
