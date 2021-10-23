using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TaskLiner
{
    /// <summary>
    /// Класс Helper, загружает конфигурацию БД из .env файла 
    /// </summary>
    public static class ConfigLoader
    {
        /// <summary> Строка подключения к MongoDb </summary>
        public static string MySqlURL => _mysqlURL ??= ConfigureURLMySqlDBFromEnviroment();
        /// <summary> Строка подключения к MariaDB </summary>


        private static string _mysqlURL;

        /// <summary>
        /// Загружает данные из файла .env, добавляет все данные в переменные окружения
        /// Данные должны быть в формате key = value
        /// </summary>
        /// <param name="filename"></param>
        public static void Load(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine(".env file does not exists");
                return;
            }

            string[] lines = File.ReadAllLines(filename);

            foreach (var line in lines)
            {
                // Отделяем ключ и значение
                var parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);
                // Добавляем полученные значения в переменные окружения
                if (parts.Length == 2) Environment.SetEnvironmentVariable(parts[0], parts[1]);
                else throw new NotImplementedException("Uncorrect env variables, check .env file");
            }

            // Строим конфиг
            new ConfigurationBuilder().AddEnvironmentVariables().Build();
        }


        private static string ConfigureURLMySqlDBFromEnviroment()
        {
            var address = Environment.GetEnvironmentVariable("MYSQL_HOSTNAME");
            var port = Environment.GetEnvironmentVariable("MYSQL_PORT");
            var userId = Environment.GetEnvironmentVariable("MYSQL_USER");
            var password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
            var databaseName = Environment.GetEnvironmentVariable("MYSQL_DATABASE");

            var result = $@"Server={address};Port={port};Database={databaseName};Uid={userId};Pwd={password};";

            return result;
        }
    }
}
