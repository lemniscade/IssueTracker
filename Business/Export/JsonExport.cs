using IssueTracker.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IssueTracker.Business.Services
{
    public class JsonExport
    {
        public async Task ExportIssuesToJsonAsync<T>(string filePath, List<T> listToExport) where T : class
        {
            using var dbContext = new ApplicationDbContext();

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(listToExport, jsonOptions);
            await File.WriteAllTextAsync(filePath, jsonString);
        }
    }
}
