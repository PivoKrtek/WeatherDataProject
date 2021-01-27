using System;
using System.Collections.Generic;
using System.Linq;
using WeatherDataLibrary.Models;
using WeatherDataLibrary.WeatherDataAnalysis;

namespace WeatherDataLibrary
{
    public class WeatherDataPrinting
    {
        public static string PrintShortDate(DateTime date)
        {
            return date.Date.ToString().Substring(0, 10);
        }
        private static string inputDate()
        {
            string dateInput = "";
            bool tryAgain = true;

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("\n\t\t\tMata in ett datum i formatet ÅÅÅÅ-MM-DD, avsluta med ENTER:");
            while (tryAgain)
            {
                tryAgain = false;
                Console.Write("\n\t\t\t-----> ");
                dateInput = Console.ReadLine();
                bool ok = DateTime.TryParse(dateInput, out DateTime date);
                if (!ok || !WeatherDatabaseAnalysis.CheckIfData(date))
                {
                    tryAgain = true;
                    Console.WriteLine("\t\t\tFörsök igen.");
                    Console.WriteLine("\t\t\tDu har gjort en felaktig inmatning eller valt ett datum som ej finns i databasen.");
                }
            }
            return dateInput;
        }
        public static void PrintAverageTemperatureCertainDay()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\n\t\t\tMedelvärde av temperatur valfritt datum mellan {WeatherDatabaseAnalysis.StartDateDatabase()} och {WeatherDatabaseAnalysis.EndDateDatabase()}.");
            Console.WriteLine("\t\t\t-------------------------------------------------------------------------");
            string date = inputDate();
            Console.ForegroundColor = ConsoleColor.Blue;

            if (WeatherDatabaseAnalysis.CheckIfData(WeatherDatabaseAnalysis.GetDayData(date, WeatherDatabaseAnalysis.outdoorThermometer)))
            {
                Console.WriteLine($"\n\t\t\tMedeltemperatur utomhus {date}:\t{TemperatureAnalysis.AverageTemperatureACertainDay(date, WeatherDatabaseAnalysis.outdoorThermometer)} grader");
            }
            else
            {
                Console.WriteLine("\t\t\tUppgifter om utomhustemperatur saknas valt datum.");
            }
            if (WeatherDatabaseAnalysis.CheckIfData(WeatherDatabaseAnalysis.GetDayData(date, WeatherDatabaseAnalysis.indoorThermometer)))
            {
                Console.WriteLine($"\n\t\t\tMedeltemperatur inomhus {date}:\t{TemperatureAnalysis.AverageTemperatureACertainDay(date, WeatherDatabaseAnalysis.indoorThermometer)} grader");
            }
            else
            {
                Console.WriteLine("\t\t\tUppgifter om inomhustemperatur saknas valt datum.");
            }
            Console.ReadLine();
        }
        public static void PrintAverageTemperaturesAllDays(int numberOfHighest, int numberOfLowest)
        {
            PrintAverageTemperatureAllDaysPerThermometer(numberOfHighest, numberOfLowest, WeatherDatabaseAnalysis.outdoorThermometer);
            PrintAverageTemperatureAllDaysPerThermometer(numberOfHighest, numberOfLowest, WeatherDatabaseAnalysis.indoorThermometer);
            Console.ReadLine();
        }
        private static void PrintAverageTemperatureAllDaysPerThermometer(int numberOfHighest, int numberOfLowest, int whichThermometer)
        {
            if (whichThermometer == 1)
            {
                whichThermometer = WeatherDatabaseAnalysis.outdoorThermometer;
            }
            else
            {
                whichThermometer = WeatherDatabaseAnalysis.indoorThermometer;
            }

            List<IGrouping<DateTime, WeatherData>> data = TemperatureAnalysis.AverageTemperatureEachDay(whichThermometer);
            var highest = data
                  .Take(numberOfHighest);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\n\t\t\tMEDELTEMPERATUR {(whichThermometer == 1 ? "UTOMHUS" : "INOMHUS")}");
            Console.WriteLine("\t\t\t-----------------------");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n\t\t\tDatum:\t\tMedeltemperatur:");
            Console.ForegroundColor = ConsoleColor.Blue;
            foreach (var group in highest)
            {
                Console.WriteLine($"\t\t\t{PrintShortDate(group.Key)}\t{TemperatureAnalysis.AverageTemperature(group.ToList())} grader");
            }
            SteppingOverInfo(numberOfHighest, numberOfLowest, data.Count);

            var lowest = data
                .TakeLast(numberOfLowest);

            foreach (var group in lowest)
            {
                Console.WriteLine($"\t\t\t{PrintShortDate(group.Key)}\t{TemperatureAnalysis.AverageTemperature(group.ToList())} grader");
            }
        }
        public static void PrintAverageHumidityAllDays(int numberOfHighest, int numberOfLowest)
        {
            PrintAverageHumidityAllDaysPerThermometer(numberOfHighest, numberOfLowest, WeatherDatabaseAnalysis.outdoorThermometer);
            PrintAverageHumidityAllDaysPerThermometer(numberOfHighest, numberOfLowest, WeatherDatabaseAnalysis.indoorThermometer);
            Console.ReadLine();
        }
        private static void PrintAverageHumidityAllDaysPerThermometer(int numberOfHighest, int numberOfLowest, int whichThermometer)
        {
            if (whichThermometer == 1)
            {
                whichThermometer = WeatherDatabaseAnalysis.outdoorThermometer;
            }
            else
            {
                whichThermometer = WeatherDatabaseAnalysis.indoorThermometer;
            }
            List<IGrouping<DateTime, WeatherData>> data = HumidityAnalysis.AverageHumidityEachDay(whichThermometer);
            var highest = data
                  .Take(numberOfHighest);
            var lowest = data
              .TakeLast(numberOfLowest);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\n\t\t\tMEDELFUKTIGHET {(whichThermometer == 1 ? "UTOMHUS" : "INOMHUS")}");
            Console.WriteLine("\t\t\t-----------------------");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n\t\t\tDatum:\t\tMedelfuktighet/dag:");
            Console.ForegroundColor = ConsoleColor.Blue;
            PrintHumidityHighestOrLowest(highest);
            SteppingOverInfo(numberOfHighest, numberOfLowest, data.Count);
            PrintHumidityHighestOrLowest(lowest);
        }
        private static void SteppingOverInfo(int numberOfHighest, int numberOfLowest, int dataCount)
        {
            Console.WriteLine($"\t\t\t--------------------------------- \n\t\t\tstepping over data from {dataCount - (numberOfHighest + numberOfLowest)} days\n\t\t\t---------------------------------");
        }
        private static void PrintHumidityHighestOrLowest(IEnumerable<IGrouping<DateTime, WeatherData>> highest)
        {
            foreach (var group in highest)
            {
                Console.WriteLine($"\t\t\t{PrintShortDate(group.Key)}\t{HumidityAnalysis.AverageHumidity(group.ToList())} %");
            }
        }
        public static void PrintAutumnStart()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n\t\t\tHÖSTENS STARTDATUM:");
            Console.WriteLine("\t\t\t-------------------");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\n\t\t\t {(WeatherDatabaseAnalysis.AutumnStart() == default ? "Höststart ej funnen i tillgänglig data." : WeatherDatabaseAnalysis.ShortDate(WeatherDatabaseAnalysis.AutumnStart()))}");
            Console.ReadLine();
        }
        public static void PrintWinterStart()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n\t\t\tVINTERNS STARTDATUM:");
            Console.WriteLine("\t\t\t-------------------");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\n\t\t\t{(WeatherDatabaseAnalysis.WinterStart() == default ? "Vinterstart ej funnen i tillgänglig data." : WeatherDatabaseAnalysis.ShortDate(WeatherDatabaseAnalysis.WinterStart()))}");
            Console.ReadLine();
        }
        public static void PrintAverageThermometerDifference(int numberOfHighest, int numberOfLowest)
        {
            WeatherDataPrinting.PrintAverageInnerAndOuterTemperatureDifference(numberOfHighest, numberOfLowest);
            WeatherDataPrinting.PrintHighestHourInnerAndOuterTemperatureDifference();
            Console.ReadLine();
        }
        public static void PrintHighestHourInnerAndOuterTemperatureDifference()
        {
            IEnumerable<IGrouping<DateTime, WeatherData>> data = TemperatureAnalysis.HighestHourInnerAndOuterTemperatureDifference();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n\t\t\tDatum:\t\tDe 10 högsta medeltemperaturskillnaden under en specifik timme varje dag*:");
            Console.WriteLine("\t\t\t-------------------------------------------------------------------------------------\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            foreach (var group in data)
            {
                Console.WriteLine($"\t\t\t{PrintShortDate(group.Key)}\t{Math.Round(TemperatureAnalysis.HourDifference(group.ToList()), 1)} grader");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n\t\t\t*För varje timme, varje dygn, har medeltemperaturen inne och ute räknats ut för den specifika timmen. \n\t\t\tDen högsta tim-temperaturskillnaden ute och inne, har sparats som värde för varje dygn.");
        }
        public static void PrintAverageInnerAndOuterTemperatureDifference(int numberOfHighest, int numberOfLowest)
        {
            List<IGrouping<DateTime, WeatherData>> data = TemperatureAnalysis.AverageInnerAndOuterTemperatureDifference();
            var highest = data
                    .Take(numberOfHighest);
            var lowest = data
                .TakeLast(numberOfLowest);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n\t\t\tDatum:\t\tMedeltemp ute:\tMedeltemp inne:\tTemperaturskillnad ute och inne:");
            Console.WriteLine("\t\t\t--------------------------------------------------------------\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            PrintAverageInnerAndOuterTemperatureDifferenceHighestAndLowest(highest);
            SteppingOverInfo(numberOfHighest, numberOfLowest, data.Count);
            PrintAverageInnerAndOuterTemperatureDifferenceHighestAndLowest(lowest);
        }

        public static void print()
        {
            Console.WriteLine(OpenDoorAnalysis.AverageDifferenceBetweenAllMeasurings(WeatherDatabaseAnalysis.indoorThermometer));
        }
        private static void PrintAverageInnerAndOuterTemperatureDifferenceHighestAndLowest(IEnumerable<IGrouping<DateTime, WeatherData>> highest)
        {
            foreach (var group in highest)
            {
                Console.WriteLine($"\t\t\t{PrintShortDate(group.Key)}\t{TemperatureAnalysis.AverageTemperature(group.ToList(), WeatherDatabaseAnalysis.outdoorThermometer)} grader\t{TemperatureAnalysis.AverageTemperature(group.ToList(), WeatherDatabaseAnalysis.indoorThermometer)} grader \t{TemperatureAnalysis.DifferenceInTemperature(TemperatureAnalysis.AverageTemperature(group.ToList(), WeatherDatabaseAnalysis.indoorThermometer), TemperatureAnalysis.AverageTemperature(group.ToList(), WeatherDatabaseAnalysis.outdoorThermometer))} grader");
            }
        }
        public static void PrintMoleRiskInAndOut(int highdata, int lowdata)
        {
            PrintMoleRisk(highdata, lowdata, WeatherDatabaseAnalysis.outdoorThermometer);
            PrintMoleRisk(highdata, lowdata, WeatherDatabaseAnalysis.indoorThermometer);
            Console.ReadLine();
        }
        private static void PrintMoleRisk(int numberOfHighest, int numberOfLowest, int thermometer)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\n\t\t\tMÖGELRISK {(thermometer == 1 ? "UTOMHUS" : "INOMHUS")}");
            Console.WriteLine("\t\t\t-----------------");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n\t\t\tDatum:\t\tMedeltemperatur:\tMedelfuktighet:\tRisk för mögel:");
            Console.WriteLine("\t\t\t---------------------------------------------------------------------\n");

            List<IGrouping<DateTime, WeatherData>> data = MoleAnalysis.MoleRiskPerDay(thermometer);
            var highest = data
                    .Take(numberOfHighest);
            var lowest = data
                            .TakeLast(numberOfLowest);

            Console.ForegroundColor = ConsoleColor.Blue;
            PrintMoleRiskHighOrLow(thermometer, highest);
            SteppingOverInfo(numberOfHighest, numberOfLowest, data.Count);
            PrintMoleRiskHighOrLow(thermometer, lowest);
        }
        private static void PrintMoleRiskHighOrLow(int thermometer, IEnumerable<IGrouping<DateTime, WeatherData>> highest)
        {
            foreach (var group in highest)
            {
                double temp = TemperatureAnalysis.AverageTemperature(group.ToList(), thermometer);
                double hum = HumidityAnalysis.AverageHumidity(group.ToList(), thermometer);

                Console.WriteLine($"\t\t\t{PrintShortDate(group.Key)}\t{temp} grader\t\t{hum} %\t\t{MoleAnalysis.MoleRisk(temp, hum)} %");
            }
        }
        public static void PrintOpenDoorInfoAllDays(int howMany)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n\t\t\tBERÄKNING AV HUR LÄNGE BALKONGDÖRREN VARIT ÖPPEN/DAG:");
            Console.WriteLine("\t\t\t-----------------------------------------------------\n");
            
            List<OpenDoorAnalysis> lista = WeatherDatabaseAnalysis.OpenDoorList();
            
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\t\t\tHÖGSTA VÄRDENA:");
            Console.WriteLine("\t\t\t---------------\n");
            Console.WriteLine($"\t\t\tDatum:\t\t\tÖppen tid sammanlagt:\n");
            Console.ForegroundColor = ConsoleColor.Blue;

            for (int i = 0; i < howMany; i++)
            {
                Console.WriteLine($"\t\t\t{i + 1}. {PrintShortDate(lista[i].Date)}\t\t{lista[i].TotallyOpenTime.Hours} h, {lista[i].TotallyOpenTime.Minutes} min");
            }
            Console.ReadLine();
        }
        public static void PrintOpenDoorInfoCertainDay()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\n\t\t\tINFORMATION OM ÖPPEN DÖRR, valbara datum mellan {WeatherDatabaseAnalysis.StartDateDatabase()} och {WeatherDatabaseAnalysis.EndDateDatabase()}.");
            Console.WriteLine("\t\t\t--------------------------------------------------------------------------");
            string date = inputDate();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n\tINFORMATION OM ÖPPEN DÖRR " + date);
            Console.WriteLine("\t------------------------------------\n");

            List<OpenDoor> lista = WeatherDatabaseAnalysis.ListOfOpenDoorACertainDay(date);

            if (lista.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("\t\t\tInga tydliga indikationer för öppnande av balkongdörren funna detta datum.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                OpenDoorAnalysis door = WeatherDatabaseAnalysis.OpenDoorAnalysisASpecialDate(date);
                Console.Write($"\tMedeltemperatur ute under dagen: ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{door.AverageTemperatureOutside} grader");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write($"\tSkillnad i temp inomhus och utomhus: ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{door.TemperatureDifferenceInAndOutside} grader");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write($"\tTemperaturförändring som minst för dörren antas öppnas: ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{Math.Round(door.TemperatureChangeToSeeIfDoorOpens, 1)} grader");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write($"\tTidsintervall mellan mätningarna som är ok: ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{OpenDoorAnalysis.okMeasuringSpan} min\n");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"\tÖPPNAS TID\tTEMP INNAN --> EFTER ÖPPNANDE\tSTÄNGS TID\tTEMP INNAN --> EFTER STÄNGNING\tTOTAL TID ÖPPEN DÖRR\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                foreach (var opening in lista)
                {
                    Console.WriteLine($"\t{opening.DoorOpens.TimeOfDay}\t{String.Format("{0:0.0}", opening.TempBeforeOpening)} C --> {String.Format("{0:0.0}", opening.TempAfterOpening)} C\t\t{opening.DoorCloses.TimeOfDay}\t{String.Format("{0:0.0}", opening.TempBeforeClosing)} C --> {String.Format("{0:0.0}", opening.TempAfterClosing)} C\t\t{opening.OpenTime.Hours} h, {opening.OpenTime.Minutes} min");
                }
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"\t\t\t\t\t\t\t\t\t\t\t\t\t-----------");
                Console.WriteLine($"\t\t\t\t\t\t\t\t\t\t\t\t\t{door.TotallyOpenTime.Hours} h, {door.TotallyOpenTime.Minutes} min");
            }
            Console.ReadLine();
        }
    }
}
