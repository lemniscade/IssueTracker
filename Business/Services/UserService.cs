using FluentValidation;
using IssueTracker.DataAccess.Repositories;
using IssueTracker.Entity.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Business.Services
{
    public class UserService : IUserService
    {
        public string usernameOfExistUser;
        private readonly IValidator<User> _validator;
        private readonly IUserRepository _userRepository;
        public UserService(IValidator<User> validator, IUserRepository userRepository)
        {
            _validator = validator;
            _userRepository = userRepository;
        }
        public bool Login(string username, string password)
        {
            bool isUserExist = _userRepository.IsUserExist(username, password);
            if (isUserExist) {
                UserRepository user = new UserRepository();
                this.usernameOfExistUser = user.user.Username;
                return true;
            }
            return false;
        }

        public bool Register(string username, string password)
        {
            return _userRepository.Create(username, password);
        }
    }
}
