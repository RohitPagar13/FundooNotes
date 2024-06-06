using Microsoft.Data.SqlClient;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System.ComponentModel.DataAnnotations;

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
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (_db.users.Any(user => user.Email.Equals(model.Email) && user.Phone.Equals(model.Phone)))
                    {
                        throw new UserException("Customer with specified Email or Phone Already Exists", "CustomerAlreadyExists");
                    }
                    User customer = new User();

                    customer.FirstName = model.FirstName;

                    customer.LastName = model.LastName;

                    customer.Email = model.Email;

                    customer.Phone = model.Phone;

                    customer.Password=model.Password;

                    customer.BirthDate = model.BirthDate;

                    _db.users.Add(customer);
                    _db.SaveChanges();
                    transaction.Commit();
                    return customer;
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
