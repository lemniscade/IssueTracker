using IssueTracker.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IssueTracker.Business.Services
{
    public class JsonExport
    {
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

                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                DateTime now = DateTime.Now;

                string timestamp = now.ToString("yyyy-MM-dd_HH-mm-ss");
                filePath =filePath+"//"+ $"{timestamp}_exported.json";
                string jsonString = JsonSerializer.Serialize(listToExport, jsonOptions);
                File.WriteAllText(filePath, jsonString);
            }
            else
            {
                Console.WriteLine("You do not have permission to write to this file path. Please check your permissions and try again.");
            }
           
        }
    }
}
