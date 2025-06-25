using FluentValidation;
using IssueTracker.Business.Exceptions;
using IssueTracker.Business.Logging;
using IssueTracker.Entity;
using IssueTracker.Entity.Models;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User user;
        private readonly IValidator<User> _validator;
        private ExceptionLogging _logging;
        public UserRepository(IValidator<User> validator)
        {
            _validator = validator;
            _logging = new ExceptionLogging();
        }
        public bool ValidateUser(User user)
        {
            var validationResult = _validator.Validate(user);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    Console.WriteLine($"Hata: {error.ErrorMessage}");
                }
            }
            return false;
        }
        public bool Create(string username, string password)
        {
                using (var context = new ApplicationDbContext())
                {
                User previosuUser = context.Users.FirstOrDefault(u => u.Username == username);
                if(previosuUser != null)
                {
                    Console.WriteLine("User already exists.");
                    return false;
                }
                var user = new User
                    {
                        Username = username,
                        Password = password
                    };
                if(IsUserExist(username, password))
                {
                    Console.WriteLine("User register operation failed, there is a user with same username.");
                    this._logging.Create("User register operation failed, there is a user with same username.", new Dictionary<string, object> { { "Attempted username", username } });
                    return false;
                }
                if (ValidateUser(user))
                {
                    Console.WriteLine("Validation failed. User not created.");
                    this._logging.Create("Validation failed. User not created.", new Dictionary<string, object> { { "Attempted username", username } });
                    return false;
                }
                    user.Password = HashPassword(password);
                    context.Users.Add(user);
                    return context.SaveChanges() > 0;
                }
            }
        public bool Update(string oldUsername, string newUsername, string password)
        {
            using (var context = new ApplicationDbContext())
                    {
                        var user = context.Users.FirstOrDefault(u => u.Username == oldUsername);
                    if (user == null)
                    {
                        Console.WriteLine("User can't find. Please repeat again with different username.");
                    }
                            user.Username = newUsername;
                            user.Password = password;
                        ValidateUser(user);
                        user.Password = HashPassword(password);
                        return context.SaveChanges() > 0;
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

        public bool IsUserExist(string username, string? password)
        {
            
            User existUser = new User();
            using (var context = new ApplicationDbContext())
            {
                if (string.IsNullOrEmpty(password))
                {
                    existUser = context.Users.FirstOrDefault(u => u.Username == username);
                } else {
                    string passwordHash = HashPassword(password);
                    existUser = context.Users.FirstOrDefault(u => u.Username == username && u.Password == passwordHash);
                }

                if (existUser != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
        }
        public User existUser(string username)
        {
            using (var context = new ApplicationDbContext())
            {
                User user = context.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    return user;
                }
            }
            return null;
        }

        public bool IsAdmin(string username,string password)
        {
                using (var context = new ApplicationDbContext())
                {
                    User user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == HashPassword(password));
                    if (user != null && user.Username=="VeriPark")
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
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                foreach (var t in bytes)
                    builder.Append(t.ToString("x2"));

                return builder.ToString();
            }
        }


    }
}
