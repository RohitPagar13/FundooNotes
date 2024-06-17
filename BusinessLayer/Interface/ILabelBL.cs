using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ILabelBL
    {
        public Label createLabel(string labelName);

        public Label UpdateLabel(int id, string newLabelName);

        public IEnumerable<Label> GetLabels();

        public Label GetLabelById(int id);

        public void removeLabel(int id);
    }
}
