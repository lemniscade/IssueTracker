using IssueTracker.Business.Logging;
using IssueTracker.Entity;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using IssueTracker.Entity.Models;

namespace IssueTracker.Business.Services
{
    public class JsonExport
    {
        private ExceptionLogging _logging = new ExceptionLogging();
        public void ExportIssuesToJsonAsync<T>(string filePath, List<T> listToExport) where T : class
        {
            bool IsAuthorized=false;

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            try
            {
                string testFilePath = Path.Combine(filePath, Path.GetRandomFileName());

                using (FileStream fs = File.Create(testFilePath, 1, FileOptions.DeleteOnClose)) { }

                IsAuthorized=true;
            }
            catch (UnauthorizedAccessException)
            {
                IsAuthorized = false;
            }
            catch (SecurityException)
            {
                IsAuthorized = false;
            }
            catch (Exception)
            {
                IsAuthorized = false;
            }
            DateTime currentDate = DateTime.Now;
            if (IsAuthorized)
            {
                using var dbContext = new ApplicationDbContext();

                if (listToExport is List<Project> projects)
                {
                    foreach (var element in projects)
                    {
                        if (element.Issues != null && !element.Issues.Any())
                        {
                            element.Issues = null;
                        }
                    }
                }


                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                };
                DateTime now = DateTime.Now;






                string timestamp = now.ToString("yyyy-MM-dd_HH-mm-ss");
                filePath =filePath+"//"+ $"{timestamp}_exported.json";

                



                string jsonString = JsonConvert.SerializeObject(listToExport, settings);
                File.WriteAllText(filePath, jsonString);
            }
            else
            {
                _logging.Create("You do not have permission to write to this file path. Please check your permissions and try again", new Dictionary<string, object> { {"Restricted file path" , filePath} });
                AnsiConsole.MarkupLine("\n[red]You do not have permission to write to this file path. Please check your permissions and try again.[/]");
            }
           
        }
    }
}
