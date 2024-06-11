using Microsoft.Data.SqlClient;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using RepositoryLayer.Utilities;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly  FundooContext _db;

        public UserRL(FundooContext db)
        {
            this._db = db;
        }

        public LoginResponse LoginUser(UserLoginModel loginModel)
        {
            try
            {
                var result = _db.users.Where(user=>user.Email.Equals(loginModel.Email)).FirstOrDefault();
                if (result == null)
                {
                    throw new UserException("User not found with given Email", "UserNotFoundException");
                }
                else if (HashPassword.verifyHash(loginModel.Password, result.Password))
                {
                    LoginResponse lr = new LoginResponse();
                    lr.ID = result.Id;
                    lr.FirstName=result.FirstName;
                    lr.LastName=result.LastName;
                    lr.Email=result.Email;
                    lr.Phone=result.Phone;
                    lr.BirthDate=result.BirthDate;
                    return lr;
                }
                else
                {
                    throw new UserException("Please enter correct Password", "IncorrectPasswordException");
                }
            }
            catch (SqlException se)
            {
                Console.WriteLine(se.ToString());
                throw;
            }
        }
    

        public LoginResponse RegisterUser(UserModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (_db.users.Any(user => user.Email.Equals(model.Email) && user.Phone.Equals(model.Phone)))
                    {
                        throw new UserException("Customer with specified Email or Phone Already Exists", "CustomerAlreadyExists");
                    }
                    User user = new User();
                    LoginResponse lr = new LoginResponse();

                    user.FirstName=lr.FirstName = model.FirstName;

                    user.LastName=lr.LastName = model.LastName;

                    user.Email=lr.Email = model.Email;

                    user.Phone=lr.Phone = model.Phone;

                    user.Password = HashPassword.convertToHash(model.Password);

                    user.BirthDate=lr.BirthDate = model.BirthDate;

                    _db.users.Add(user);
                    _db.SaveChanges();
                    lr.ID = user.Id;
                    transaction.Commit();
                    return lr;
                }
                catch (SqlException se)
                {
                    transaction.Rollback();
                    Console.WriteLine(se.ToString());
                    throw;
                }
            }
        }

    }
}
