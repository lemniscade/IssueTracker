using IssueTracker.Entity;
using IssueTracker.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;

namespace IssueTracker.Business.Logging
{
    public class ExceptionLogging
    {
        public ExceptionLogging()
        {

        }
        public string LogManualError(string message, Dictionary<string, object> context)
        {
            string user = Environment.UserName;
            string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var stackTraceObj = new StackTrace(true);
            string stackTrace = stackTraceObj.ToString();
            var frame = new StackTrace().GetFrame(1);
            MethodBase method = frame.GetMethod();
            string methodName = method.Name;
            string className = method.DeclaringType?.FullName ?? "Unknown";
            var logObject = new
            {
                timestamp = timestamp,
                message = message,
                user = user,
                location = $"{className}.{methodName}",
                stackTrace = stackTrace,
                context = context
            };
            string json = JsonSerializer.Serialize(logObject, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            return json;
        }
        public bool Create(string message, Dictionary<string, object> context)
        {
            string jsonLog = LogManualError(message, context);
            Log log = new Log
            {
                JsonLog = jsonLog,
                CreatedAt = DateTime.Now
            };
            using (var dbContext = new ApplicationDbContext())
            {
                dbContext.Logs.Add(log);
                return dbContext.SaveChanges() > 0;
            }
        }

        public List<Log> FilterLog(DateTime start, DateTime end, string jsonParam)
        {
            using (var context = new ApplicationDbContext())
            {
                var query = context.Logs.Where(i => i.CreatedAt >= start && i.CreatedAt <= end);

                return query.ToList();
            }
        }
    }
}
