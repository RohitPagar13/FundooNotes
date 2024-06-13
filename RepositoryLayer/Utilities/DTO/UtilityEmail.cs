using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Utilities.emailModel
{
    public class UtilityEmail
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
    }
}
