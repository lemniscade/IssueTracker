using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Business.Exceptions
{
    public class BusinessException : AppException
    {
        public BusinessException(string message) : base(message)
        {
            Console.WriteLine($"Business Rule Error: {message}");
        }
    }
}
