using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.CustomException
{
    public class UserException:ApplicationException
    {
        public string? ErrorCode { get; }
        public UserException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
