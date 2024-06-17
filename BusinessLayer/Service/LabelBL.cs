using BusinessLayer.Interface;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class LabelBL : ILabelBL
    {
        private readonly ILabelRL labelRL;

        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }


        public Label createLabel(string labelName)
        {
            try
            {
                return labelRL.createLabel(labelName);
            }
            catch
            {
                throw;
            }
        }

        public Label GetLabelById(int id)
        {
            try
            {
                return labelRL.GetLabelById(id);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Label> GetLabels()
        {
            try
            {
                return labelRL.GetLabels();
            }
            catch
            {
                throw;
            }
        }

        public void removeLabel(int id)
        {
            try
            {
                labelRL.removeLabel(id);
            }
            catch
            {
                throw;
            }
        }

        public Label UpdateLabel(int id, string newlabelName)
        {
            try
            {
                return labelRL.UpdateLabel(id, newlabelName);
            }
            catch
            {
                throw;
            }
        }
    }
}
