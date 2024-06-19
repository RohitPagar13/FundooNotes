using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class LabelRL : ILabelRL
    {
        private readonly FundooContext _db;


        public LabelRL(FundooContext db)
        {
            _db = db;
        }
        public Label createLabel(string labelName)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Label label = new Label();

                    label.LabelName = labelName;
                    _db.labels.Add(label);
                    _db.SaveChanges();
                    transaction.Commit();
                    return label;
                }
                catch (SqlException se)
                {
                    transaction.Rollback();
                    Console.WriteLine(se.ToString());
                    throw;
                }
            }
        }

        public Label UpdateLabel(int id, string newLabelName)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                        Label l = _db.labels.Find(id);
                        if (l == null)
                        {
                            throw new UserException("Label you are trying to update is not exists", "LabelNotExistsException");
                        }
                        l.LabelName = newLabelName;
                        _db.labels.Update(l);
                        _db.SaveChanges();

                        return l;
                }
                catch (SqlException se)
                {
                    transaction.Rollback();
                    Console.WriteLine(se.ToString());
                    throw;
                }
            }
        }

        public IEnumerable<Label> GetLabels()
        {
            try
            {
                return _db.labels.ToList();
            }
            catch (SqlException se)
            {
                Console.WriteLine(se.ToString());
                throw;
            }
        }   

        public Label GetLabelById(int id)
        {
            try
            {
                var result=  _db.labels.Where(Label => Label.Id == id).FirstOrDefault();
                if(result == null)
                {
                    throw new UserException("label does not exists", "LabelNotExistsException");
                }
                return result;
            }
            catch (SqlException se)
            {
                Console.WriteLine(se.ToString());
                throw;
            }
        }

        public void removeLabel(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                        Label? l = _db.labels.Find(id);
                        if (l == null)
                        {
                            throw new UserException("Label you are trying to delete is not exists", "LabelNotExistsException");
                        }
                        _db.labels.Remove(l);
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
