using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherDataLibrary.Models;

namespace WeatherDataLibrary.WeatherDataAnalysis
{
    public class TemperatureAnalysis
    {
        public const int indoorThermometer = 2;
        public const int outdoorThermometer = 1;
        public static List<IGrouping<DateTime, WeatherData>> AverageInnerAndOuterTemperatureDifference()
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .AsEnumerable()
                    .GroupBy(d => d.DateAndTime.Date)
                    .Where(d => WeatherDatabaseAnalysis.DataExistsBothInAndOutACertainDay(d.ToList()))
                    .OrderByDescending(d => DifferenceInTemperature(AverageTemperature(d.ToList(), indoorThermometer), AverageTemperature(d.ToList(), outdoorThermometer)))
                    .ToList();

                return d;
            }
        }
        public static double AverageTemperatureACertainDay(string date, int thermometer)
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .Where(d => d.Thermometer == thermometer)
                    .Where(d => d.DateAndTime.Date == DateTime.Parse(date))
                    .Average(d => d.Temperature);

                return Math.Round(d, 1);
            }
        }
        
        public static double AverageTemperature(List<WeatherData> list, int thermomter)
        {
            var l = list
                .Where(l => l.Thermometer == thermomter)
                .Average(l => l.Temperature);

            return Math.Round(l, 1);
        }
        public static double DifferenceInTemperature(double inDoor, double outdoor)
        {
            if (inDoor >= outdoor)
                return Math.Round(inDoor - outdoor, 3);
            else
                return Math.Round(outdoor - inDoor, 3);

        }
        public static double AverageTemperature(List<WeatherData> list)
        {
            var l = list
                .Average(l => l.Temperature);

            return Math.Round(l, 1);
        }
        public static List<IGrouping<DateTime, WeatherData>> AverageTemperatureEachDay(int whichTermometer)
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .AsEnumerable()
                    .Where(d => d.Thermometer == whichTermometer)
                    .GroupBy(d => d.DateAndTime.Date)
                    .OrderByDescending(d => AverageTemperature(d.ToList()))
                    .ToList();
                return d;
            }
        }
        public static IEnumerable<IGrouping<DateTime, WeatherData>> HighestHourInnerAndOuterTemperatureDifference()
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .AsEnumerable()
                    .GroupBy(d => d.DateAndTime.Date)
                    .Where(d => WeatherDatabaseAnalysis.DataExistsBothInAndOutACertainHour(d.ToList()))
                    .OrderByDescending(d => HourDifference(d.ToList()))
                    .Take(10)
                    .ToList();

                return d;
            }
        }
        public static double HourDifference(List<WeatherData> dayData)
        {
            List<int> hourList = new List<int>();
            List<int> hourListSame = new List<int>();
            List<double> differences = new List<double>();

            for (int i = 0; i < dayData.Count; i++)
            {
                if (dayData[i].Thermometer == outdoorThermometer)
                {
                    bool exists = false;
                    for (int j = 0; j < hourList.Count; j++)
                    {
                        if (dayData[i].DateAndTime.Hour == hourList[j])
                        {
                            exists = true;
                            break;
                        }

                    }
                    if (!exists) { hourList.Add(dayData[i].DateAndTime.Hour); }
                }
            }
            for (int i = 0; i < dayData.Count; i++)
            {
                if (dayData[i].Thermometer == indoorThermometer)
                {
                    for (int j = 0; j < hourList.Count; j++)
                    {
                        if (hourList[j] == dayData[i].DateAndTime.Hour)
                        {
                            bool exists = false;
                            for (int k = 0; k < hourListSame.Count; k++)
                            {
                                if (dayData[i].DateAndTime.Hour == hourListSame[k])
                                {
                                    exists = true;
                                    break;
                                }
                            }
                            if (!exists) { hourListSame.Add(dayData[i].DateAndTime.Hour); }
                        }
                    }
                }
            }
            for (int i = 0; i < hourListSame.Count; i++)
            {
                var d = dayData
                    .Where(d => d.Thermometer == indoorThermometer && d.DateAndTime.Hour == hourListSame[i])
                    .Average(d => d.Temperature);

                var e = dayData
                    .Where(e => e.Thermometer == outdoorThermometer && e.DateAndTime.Hour == hourListSame[i])
                    .Average(d => d.Temperature);

                differences.Add(DifferenceInTemperature(d, e));
            }

            var dif = differences
                .OrderByDescending(dif => dif)
                .ToList();

            return dif[0];
        }
    }
}
