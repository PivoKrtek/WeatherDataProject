using System;
using System.Collections.Generic;
using System.IO;
using WeatherDataLibrary.Models;

namespace WeatherDataLibrary
{
    public class FileReader
    {
        public static bool DatabaseInfoExists()
        {
            if (File.Exists("uploadedDatabase.txt"))
            {
                string text = File.ReadAllText("uploadedDatabase.txt");
                return bool.Parse(text);
            }
            return false;
        }
        public static bool FileExists()
        {
            return File.Exists("TemperaturData.csv");
        }
        public static void SetDataBaseUploadDone()
        {
            StreamWriter sw = File.CreateText("uploadedDatabase.txt");
            sw.WriteLine("true");
            sw.Close();
        }
        public static List<WeatherData> GetFileInformation()
        {
            List<WeatherData> weatherList = new List<WeatherData>();

            string[] lines = File.ReadAllLines("TemperaturData.csv");

            for (int i = 0; i < lines.Length; i++)
            {
                string[] dataInfo = lines[i].Split(',');

                try
                {
                    WeatherData weatherData = new WeatherData(GetThermometer(dataInfo[1]), GetDateAndTime(dataInfo[0]), GetTemperature(dataInfo[2]), GetHumidity(dataInfo[3]));
                    weatherList.Add(weatherData);
                }
                catch (Exception)
                {
                    Console.WriteLine("Couldn't apply file info to database. Wrong format.");
                    break;
                }
            }
            return weatherList;
        }
        private static DateTime GetDateAndTime(string info)
        {
            return DateTime.Parse(info);
        }
        private static int GetThermometer(string info)
        {
            if (info == "Ute")
            { return 1; }
            else { return 2; }
        }
        private static double GetTemperature(string info)
        {
            string[] splittedNumber = info.Split(".");
            string newNumber = splittedNumber[0] + "," + splittedNumber[1];
            return Math.Round(double.Parse(newNumber), 1);
        }
        private static int GetHumidity(string info)
        {
            return int.Parse(info);
        }
    }
}
