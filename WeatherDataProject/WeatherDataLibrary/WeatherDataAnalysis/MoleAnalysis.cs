using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherDataLibrary.Models;

namespace WeatherDataLibrary.WeatherDataAnalysis
{
    class MoleAnalysis
    {
        public static List<IGrouping<DateTime, WeatherData>> MoleRiskPerDay(int whichThermometer)
        {
            using (var db = new EFContext())
            {
                var d = db.MeasuringData
                    .AsEnumerable()
                    .GroupBy(d => d.DateAndTime.Date)
                    .OrderByDescending(d => MoleRisk(TemperatureAnalysis.AverageTemperature(d.ToList(), whichThermometer), HumidityAnalysis.AverageHumidity(d.ToList(), whichThermometer)))
                    .ToList();

                return d;
            }
        }
        public static double MoleRisk(double temperature, double humidity)
        {
            double molerisk = 0;

            if (humidity < 79)
            { return 0; }
            else
            {
                molerisk = ((humidity - 78) * (temperature / 15)) / 0.22;
                if (molerisk > 100)
                { molerisk = 100; }
                else if (molerisk < 0)
                { molerisk = 0; }
            }
            return Math.Round(molerisk, 1);
        }
    }
}
