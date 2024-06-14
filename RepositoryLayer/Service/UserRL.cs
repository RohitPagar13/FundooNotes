using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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


        public IConfiguration _configuration;

        public UserRL(FundooContext db, IConfiguration configuration)
        {
            this._db = db;
            _configuration = configuration;
        }

        public string LoginUser(UserLoginModel loginModel)
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
                    return JWTTokenGenerator.generateToken(result.Id, result.Email, _configuration);
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
                    transaction.Commit();
                    lr.ID = user.Id;
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

        public void ForgetPassword(string email)
        {

            var result = _db.users.Where(s => s.Email == email).FirstOrDefault();

            if (result == null)
            {
                throw new UserException("No such user found", "UserNotFoundException");
            }
            var jwtToken = JWTTokenGenerator.generateToken(result.Id,result.Email, _configuration);

            EmailModel model = new EmailModel();
            model.To = result.Email;
            model.Subject = "Reset Password";
            model.Body = "http://localhost:5264/api/user/reset-password/jwttoken?" + jwtToken;

            EmailSender.SendEmail(model, _configuration);
        }


        public void ResetPassword(string email, string password)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var user = _db.users.FirstOrDefault(s => s.Email == email);
                    if (user == null)
                    {
                        throw new UserException("Invalid Credentials", "UserNotFoundException");
                    }

                    user.Password = HashPassword.convertToHash(password);

                    _db.users.Update(user);
                    _db.SaveChanges();
                    transaction.Commit();
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
