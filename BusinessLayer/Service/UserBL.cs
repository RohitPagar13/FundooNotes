using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL userRL;

        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }
        public User RegisterUser(UserModel model)
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
