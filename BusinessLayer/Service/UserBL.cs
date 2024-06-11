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

        public IConfiguration _configuration;

        public UserBL(IUserRL userRL, IConfiguration configuration)
        {
            this.userRL = userRL;

            _configuration = configuration;
        }

        public string LoginUser(UserLoginModel loginModel)
        {
            try
            {
                var user =  userRL.LoginUser(loginModel);
                if (user != null)
                {
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim("Id", user.ID.ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("Email", user.Email),
                    new Claim("Phone", user.Phone),
                    new Claim("BirthDate", user.BirthDate)
                };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signIn);
                    var jwttoken = new JwtSecurityTokenHandler().WriteToken(token);
                    return jwttoken;
                }
                else { throw new UserException("User not found", "UserNotFoundException"); }
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
    }
}
