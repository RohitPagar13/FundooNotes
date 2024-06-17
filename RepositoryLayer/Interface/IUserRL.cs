using ModelLayer;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        public LoginResponse RegisterUser(UserModel model);

        public string LoginUser(UserLoginModel loginModel);

        public LoginResponse getUser(string email);

        public void ForgetPassword(string email);
        public void ResetPassword(string email, string password);
    }
}
