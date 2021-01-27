using System;
using System.Collections.Generic;
using System.Linq;
using WeatherDataLibrary.Models;
using WeatherDataLibrary.WeatherDataAnalysis;

namespace WeatherDataLibrary
{
    public class OpenDoorAnalysis
    {
        public const int okMeasuringSpan = 10;
        public static double AverageTemperatureChangeIndoorEveryMeasuring { get; set; }
        public DateTime Date { get; set; }
        public List<WeatherData> DayData { get; set; }
        public double AverageTemperatureInside { get; set; }
        public double AverageTemperatureOutside { get; set; }
        public double? TemperatureDifferenceInAndOutside { get; set; }
        public double TemperatureChangeToSeeIfDoorOpens { get; set; }
        public List<OpenDoor> TimeDoorIsOpen { get; set; }
        public TimeSpan TotallyOpenTime { get; set; }
        public OpenDoorAnalysis(List<WeatherData> dayData)
        {
            Date = dayData[0].DateAndTime;
            DayData = dayData;
            AverageTemperatureInside = TemperatureAnalysis.AverageTemperature(dayData, 2);
            AverageTemperatureOutside = TemperatureAnalysis.AverageTemperature(dayData, 1);
            TemperatureDifferenceInAndOutside = TemperatureAnalysis.DifferenceInTemperature(AverageTemperatureInside, AverageTemperatureOutside);
            TemperatureChangeIndoorToSeeIfDoorsOpen(AverageTemperatureChangeIndoorEveryMeasuring);
            TimeDoorIsOpen = new List<OpenDoor>();
            FindByAnalysingIndoor(dayData);
            CountTotallyOpenDoor();
        }
        private void TemperatureChangeIndoorToSeeIfDoorsOpen(double average)
        {
            double temperatureChange = average * (double)TemperatureDifferenceInAndOutside;
            TemperatureChangeToSeeIfDoorOpens = temperatureChange+0.1;
        }
        private void CountTotallyOpenDoor()
        {
            for (int i = 0; i < TimeDoorIsOpen.Count; i++)
            {
                TotallyOpenTime += TimeDoorIsOpen[i].OpenTime;
            }
        }
        public static OpenDoorAnalysis CreateOpenDoorAnalysis(List<WeatherData> dayData)
        {
            if (WeatherDatabaseAnalysis.DataExistsBothInAndOutACertainDay(dayData))
            {
                OpenDoorAnalysis data = new OpenDoorAnalysis(dayData);
                return data;
            }
            return null;
        }
        public void FindByAnalysingIndoor(List<WeatherData> dayData)
        {
            var d = dayData
               .Where(d => d.Thermometer == 2)
               .OrderBy(d => d.DateAndTime)
               .ToList();

            OpenDoor open = new OpenDoor();
            bool doorIsOpen = false;
            for (int i = 0; i < d.Count - 1; i++)
            {
                if (!doorIsOpen)
                {
                    if (d[i].Temperature > d[i + 1].Temperature)
                    {
                        if ((d[i].Temperature - d[i + 1].Temperature >= TemperatureChangeToSeeIfDoorOpens) && ((d[i + 1].DateAndTime - d[i].DateAndTime).TotalMinutes <= okMeasuringSpan))
                        {
                            open.DoorOpens = d[i].DateAndTime;
                            open.TempBeforeOpening = d[i].Temperature;
                            open.TempAfterOpening = d[i + 1].Temperature;
                            doorIsOpen = true;
                        }
                    }
                }
                else
                {
                    if ((d[i + 1].DateAndTime - d[i].DateAndTime).TotalMinutes < okMeasuringSpan)
                    {
                        if (d[i].Temperature < d[i + 1].Temperature)
                        {
                            doorIsOpen = false;
                            open.DoorCloses = d[i].DateAndTime;
                            open.TempBeforeClosing = d[i].Temperature;
                            open.TempAfterClosing = d[i + 1].Temperature;
                            open.OpenTime = open.DoorCloses - open.DoorOpens;
                            TimeDoorIsOpen.Add(open);
                            open = new OpenDoor();
                        }
                    }
                    else
                    {
                        doorIsOpen = false;
                        open.DoorCloses = d[i].DateAndTime.AddMinutes(okMeasuringSpan);
                        open.TempBeforeClosing = d[i].Temperature;
                        open.TempAfterClosing = d[i+1].Temperature;
                        open.OpenTime = open.DoorCloses - open.DoorOpens;
                        TimeDoorIsOpen.Add(open);
                        open = new OpenDoor();
                    }
                }
            }
        }
        private static double AverageTemperatureChangeBetweenEachMeasuring(List<double> differences)
        {
            var t = differences
                .Average(t => t);

            return Math.Round(t, 3);
        }
        public static double AverageDifferenceBetweenAllMeasurings(int thermometer)
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .AsEnumerable()
                    .Where(d => d.Thermometer == thermometer)
                    .GroupBy(d => d.DateAndTime.Date)
                    .Select(d => AverageTemperatureChangeBetweenEachMeasuring(TemperatureChanges(d.ToList(), thermometer)))
                    .Average(d => d);

                return Math.Round(d, 3);
            }
        }
        private static List<double> TemperatureChanges(List<WeatherData> dayData, int thermometer)
        {
            List<double> temperatureChanges = new List<double>();

            var d = dayData
                .Where(d => d.Thermometer == thermometer)
                .OrderBy(d => d.DateAndTime)
                .Select(d => d.Temperature)
                .ToList();

            for (int i = 0; i < d.Count; i++)
            {
                if (i > 0)
                {
                    if (d[i] >= d[i - 1])
                    {
                        temperatureChanges.Add(d[i] - d[i - 1]);
                    }
                    else
                    {
                        temperatureChanges.Add(d[i - 1] - d[i]);
                    }
                }
            }
            return temperatureChanges;
        }
    }
}
