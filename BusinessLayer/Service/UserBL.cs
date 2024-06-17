using Azure;
using BusinessLayer.Interface;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.CustomException;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL userRL;

        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        public void ForgetPassword(string email)
        {
            try
            {
                userRL.ForgetPassword(email);
            }
            catch
            {
                throw;
            }

        }

        public LoginResponse getUser(string email)
        {
            try
            {
                return userRL.getUser(email);
            }
            catch
            {
                throw;
            }
        }

        public string LoginUser(UserLoginModel loginModel)
        {
            try
            {
                return userRL.LoginUser(loginModel);
            }
            catch
            {
                throw;
            }
        }

        public LoginResponse RegisterUser(UserModel model)
        {
            try
            {
                return userRL.RegisterUser(model);
            }
            catch
            {
                throw;
            }
        }

        public void ResetPassword(string email, string password)
        {
            try
            {
                userRL.ResetPassword(email, password);
            }
            catch
            {
                throw;
            }
        }
    }
}
