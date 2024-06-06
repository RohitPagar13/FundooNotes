using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly  FundooContext _db;

        public UserRL(FundooContext db)
        {
            this._db = db;
        }
        public User RegisterUser(UserModel model)
        {
            
        }
    }
}
