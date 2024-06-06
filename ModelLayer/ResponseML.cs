using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class ResponseML
    {
        public bool Success {  get; set; }
        public string? Message { get; set; }
        public object? Data {  get; set; }
    }
}
