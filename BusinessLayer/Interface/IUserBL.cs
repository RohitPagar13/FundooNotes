using ModelLayer;
using RepositoryLayer.Entities;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public LoginResponse RegisterUser(UserModel model);
        public string LoginUser(UserLoginModel loginModel);
    }
}
