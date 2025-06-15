using IssueTracker.Entity;
using IssueTracker.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Business.Logging
{
    public class ExceptionLogging
    {
        public bool Create(string message,string stackTrace,string source,string innerException)
        {
            using (var context = new ApplicationDbContext())
            {
                var exceptionLog = new Log
                {
                    Message = message,
                    StackTrace = stackTrace,
                    Source = source,
                    InnerException = innerException,
                    CreatedAt = DateTime.Now
                };
                context.Logs.Add(exceptionLog);
                return context.SaveChanges() > 0;
            }
        }

        public List<Log> FilterLog(DateTime start,DateTime end)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Logs.Where(i=>i.CreatedAt<end && i.CreatedAt>start).ToList();
            }
        }
    }
}
