using System;
using System.Collections.Generic;
using System.Linq;
using WeatherDataLibrary;

namespace WeatherDataProject
{
    public class Program
    {
        public const int showDataHighest = 5;
        public const int showDataLowest = 5;
        static void Main(string[] args)
        {
            Console.SetWindowSize(140, 45);
            
            string startDate = "";
            string endDate = "";
            Console.WriteLine("\n\n\t\t\tStartar . . .");

            bool runProgram = FileReader.DatabaseInfoExists();

            if(!runProgram)
            {
                if (FileReader.FileExists())
                {
                    runProgram = WeatherDatabaseService.LoadAllInformationFromFileToDatabase();
                }
                else
                {
                    Console.WriteLine("\t\t\tDu behöver lägga in filen på rätt plats för att läsa information till databasen.");
                }
                Console.ReadLine();
            }

            if (runProgram)
            {
                WeatherDatabaseAnalysis.SetAverageTemperatureChangeIndoor();
                startDate = WeatherDatabaseAnalysis.StartDateDatabase();
                endDate = WeatherDatabaseAnalysis.EndDateDatabase();
            }

            while (runProgram)
            {
                Console.Clear();
                Menu.Intro();
                Menu.InformationAccess(startDate, endDate);
                Menu.ShowMenu();
                int choiceOfInfo = Menu.InputNumber(9);
                Console.Clear();

                switch (choiceOfInfo)
                {
                    case 0:
                        Console.WriteLine("\n\t\t\tTack för den här gången. Välkommen åter!");
                        runProgram = false;
                        break;
                    case 1:
                        WeatherDataPrinting.PrintAverageTemperatureCertainDay();
                        break;
                    case 2:
                        WeatherDataPrinting.PrintAverageTemperaturesAllDays(showDataHighest, showDataLowest);
                        break;
                    case 3:
                        WeatherDataPrinting.PrintAverageHumidityAllDays(showDataHighest, showDataLowest);
                        break;
                    case 4:
                        WeatherDataPrinting.PrintMoleRiskInAndOut(showDataHighest, showDataLowest);
                        break;
                    case 5:
                        WeatherDataPrinting.PrintAutumnStart();
                        break;
                    case 6:
                        WeatherDataPrinting.PrintWinterStart();
                        break;
                    case 7:
                        WeatherDataPrinting.PrintAverageThermometerDifference(showDataHighest, showDataLowest);
                        break;
                    case 8:
                        WeatherDataPrinting.PrintOpenDoorInfoAllDays(10);
                        break;
                    case 9:
                        WeatherDataPrinting.PrintOpenDoorInfoCertainDay();
                        break;
                }
            }
            Console.ReadLine();
        }
    }
}
