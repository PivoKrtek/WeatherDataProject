using System;
using System.Collections.Generic;
using System.Linq;
using WeatherDataLibrary.Models;
using WeatherDataLibrary.WeatherDataAnalysis;

namespace WeatherDataLibrary
{
    public class WeatherDatabaseAnalysis
    {
        public const int indoorThermometer = 2;
        public const int outdoorThermometer = 1;
        public static void SetAverageTemperatureChangeIndoor()
        {
            OpenDoorAnalysis.AverageTemperatureChangeIndoorEveryMeasuring = OpenDoorAnalysis.AverageDifferenceBetweenAllMeasurings(WeatherDatabaseAnalysis.indoorThermometer);
        }
        public static string ShortDate(DateTime date)
        {
            return date.Date.ToString().Substring(0, 10);
        }
        public static string StartDateDatabase()
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .OrderBy(d => d.DateAndTime)
                    .Select(d => d.DateAndTime)
                    .ToList();

                return ShortDate(d[0]);
            }
        }
        public static string EndDateDatabase()
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .OrderByDescending(d => d.DateAndTime)
                    .Select(d => d.DateAndTime)
                    .ToList();

                return ShortDate(d[0]);
            }
        }
        public static List<WeatherData> GetDayData(string date, int thermometer)
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .Where(d => d.DateAndTime.Date == DateTime.Parse(date) && d.Thermometer == thermometer)
                    .ToList();
                return d;
            }
        }
        public static bool CheckIfData(DateTime date)
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .Where(d => d.DateAndTime.Date == date)
                    .ToList();
                if (d.Count == 0)
                {
                    return false;
                }
                return true;
            }
        }
        public static bool CheckIfData(List<WeatherData> list)
        {
            if (list.Count == 0)
            { return false; }
            else
            {
                return true;
            }
        }
        public static DateTime AutumnStart()
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .AsEnumerable()
                    .Where(d => d.Thermometer == 1 && d.DateAndTime.Month > 7)
                    .GroupBy(d => d.DateAndTime.Date)
                    .OrderBy(d => d.Key);

                int countDays = 0;
                DateTime autumnStart = default;

                foreach (var group in d)
                {
                    if (TemperatureAnalysis.AverageTemperature(group.ToList()) < 10.0)
                    {
                        countDays++;
                        if (countDays == 1)
                        {
                            autumnStart = group.Key;
                        }
                        if (countDays == 5)
                        {
                            return autumnStart;
                        }
                    }
                    else { countDays = 0; }
                }
                return default;
            }
        }
        public static DateTime WinterStart()
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .AsEnumerable()
                    .Where(d => d.Thermometer == outdoorThermometer)
                    .GroupBy(d => d.DateAndTime.Date)
                    .OrderBy(d => d.Key);

                int countDays = 0;
                DateTime winterStart = default;

                foreach (var group in d)
                {
                    if (TemperatureAnalysis.AverageTemperature(group.ToList()) < 0.0)
                    {
                        countDays++;
                        if (countDays == 1)
                        {
                            winterStart = group.Key;
                        }
                        if (countDays == 5)
                        {
                            return winterStart;
                        }
                    }
                    else { countDays = 0; }
                }
                return default;
            }
        }
        public static bool DataExistsBothInAndOutACertainHour(List<WeatherData> dayData)
        {
            List<int> hourList = new List<int>();

            for (int i = 0; i < dayData.Count; i++)
            {
                if (dayData[i].Thermometer == outdoorThermometer)
                { hourList.Add(dayData[i].DateAndTime.Hour); }
            }
            for (int i = 0; i < dayData.Count; i++)
            {
                if (dayData[i].Thermometer == indoorThermometer)
                {
                    for (int j = 0; j < hourList.Count; j++)
                    {
                        if (hourList[j] == dayData[i].DateAndTime.Hour)
                        {

                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static bool DataExistsBothInAndOutACertainDay(List<WeatherData> dayData)
        {
            bool indoor = false;
            bool outdoor = false;
            for (int i = 0; i < dayData.Count; i++)
            {
                if (dayData[i].Thermometer == outdoorThermometer)
                { outdoor = true; }
                else if (dayData[i].Thermometer == indoorThermometer)
                { indoor = true; }
                if (indoor && outdoor)
                { return true; }
            }
            return false;
        }
        public static List<OpenDoorAnalysis> OpenDoorList()
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .AsEnumerable()
                    .GroupBy(d => d.DateAndTime.Date)
                    .Where(d => DataExistsBothInAndOutACertainDay(d.ToList()))
                    .Select(d => new OpenDoorAnalysis(d.ToList()))
                    .OrderByDescending(d => d.TotallyOpenTime)
                    .ToList();

                return d;
            }
        }
        public static OpenDoorAnalysis OpenDoorAnalysisASpecialDate(string date)
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .AsEnumerable()
                    .Where(d => d.DateAndTime.Date == DateTime.Parse(date).Date)
                    .ToList();
                if(DataExistsBothInAndOutACertainDay(d))
                { return new OpenDoorAnalysis(d); }
                
            }
            
                return null;
        }
        public static List<OpenDoor> ListOfOpenDoorACertainDay(string date)
        {
            List<OpenDoorAnalysis> lista = OpenDoorList();

            var l = lista
                .Where(l => l.Date.Date == DateTime.Parse(date).Date)
                .Select(l => l.TimeDoorIsOpen)
                .ToList();

            return l[0];
        }
    }
}
