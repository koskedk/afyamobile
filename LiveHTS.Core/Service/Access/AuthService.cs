﻿using System;
using System.Linq;
using LiveHTS.Core.Interfaces.Repository;
using LiveHTS.Core.Interfaces.Repository.Subject;
using LiveHTS.Core.Interfaces.Services.Access;
using LiveHTS.Core.Model.Subject;

namespace LiveHTS.Core.Service.Access
{
    public class AuthService:IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User SignIn(string username, string password)
        {
            username = username.ToLower().Trim();

            var user = _userRepository
                .GetAll(x => x.UserName.ToLower() == username)
                .FirstOrDefault();

            if (null == user)
                throw new Exception("wrong User or Password !");

            if (String.CompareOrdinal(password, user.Password) == 0)
                return _userRepository.Get(user.Id);


            throw new Exception("wrong User or Password !");
        }
    }
}