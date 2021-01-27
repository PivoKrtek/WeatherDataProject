using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherDataLibrary.Models;

namespace WeatherDataLibrary.WeatherDataAnalysis
{
    public class HumidityAnalysis
    {
        public static double AverageHumidity(List<WeatherData> list)
        {
            var l = list
                .Average(l => (double)l.Humidity);

            return Math.Round(l, 1);
        }
        public static double AverageHumidity(List<WeatherData> list, int whichThermometer)
        {
            var l = list
                .Where(d => d.Thermometer == whichThermometer)
                .Average(l => (double)l.Humidity);

            return Math.Round(l, 1);
        }
        public static List<IGrouping<DateTime, WeatherData>> AverageHumidityEachDay(int whichTermometer)
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .AsEnumerable()
                    .Where(d => d.Thermometer == whichTermometer)
                    .GroupBy(d => d.DateAndTime.Date)
                    .OrderBy(d => AverageHumidity(d.ToList()))
                .ToList();

                return d;
            }
        }
    }
}
