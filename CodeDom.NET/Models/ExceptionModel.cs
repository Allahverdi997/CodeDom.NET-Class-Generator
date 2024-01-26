using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDom.NET.Models
{
    public class ExceptionModel 
    {
        public ExceptionModel() 
        {
            IsSucceed = true;
            IsFailed=false;
            Message =string.Empty;
            Exception = null; ;
        }  
        public ExceptionModel(Exception exception) 
        {
            IsSucceed = false;
            IsFailed = true;
            Message = exception.Message;
            Exception = exception;
        }

        public bool IsSucceed { get; set; }
        public bool IsFailed { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
