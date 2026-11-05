using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace InventoryOfEquipmentInAPolyclinic.Services
{
    public static class ConfigService
    {
        private static IConfigurationRoot? _configuration;

        public static string GetConnectionString()
        {
            if (_configuration == null)
            {
                _configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            }

            return _configuration.GetConnectionString("DefaultConnection")
                   ?? throw new Exception("Строка подключения не найдена в appsettings.json");
        }

        public static void UpdateConnectionString(string newConnectionString)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            string json = File.ReadAllText(filePath);

            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json)!;
            jsonObj["ConnectionStrings"]["DefaultConnection"] = newConnectionString;

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, output);
        }
    }
}
