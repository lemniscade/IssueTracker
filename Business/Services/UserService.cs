﻿using FluentValidation;
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
        public User existUser;
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
            if (isUserExist)
            {
                UserRepository user = new UserRepository(_validator);
                this.existUser = user.existUser(username);
                return true;
            }
            return false;
        }

        public bool Register(string username, string password)
        {

            return _userRepository.Create(username, password);
        }

        public bool Update(string oldUsername, string newUsername, string password)
        {
            bool isUserExist = Login(oldUsername, password);
            if (isUserExist)
            {
                return _userRepository.Update(oldUsername, newUsername, password);
            }
            return false;

        }

        public bool Delete(string username)
        {
            bool isUserExist=_userRepository.IsUserExist(username, null);
            if (isUserExist)
            {
                return _userRepository.Delete(username);
            }
            return false;
        }

        public bool IsAdmin(string username, string password)
        {
            return _userRepository.IsAdmin(username, password);
        }

        public bool IsUserExist(string username, string password)
        {
            return _userRepository.IsUserExist(username, password);
        }
    }
}
