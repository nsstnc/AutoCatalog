using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; // Если вы используете JSON

namespace AutoCatalog
{
    public class AppConfig
    {
        public string DatabaseConnectionString { get; set; }
    }

    public class ConfigManager
    {
        public static AppConfig LoadConfig()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\appSettings.json";


            if (File.Exists(path))
            {
                // Чтение данных из файла конфигурации
                string configText = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\appSettings.json");


                // Для JSON
                AppConfig config = JsonConvert.DeserializeObject<AppConfig>(configText);

                return config;
            }
            else
            {
                // Если файл конфигурации не найден, можно выбрасывать исключение или возвращать значения по умолчанию
                throw new FileNotFoundException("Config file not found");
            }
        }
    }
}



