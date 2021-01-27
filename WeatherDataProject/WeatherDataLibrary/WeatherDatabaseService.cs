using System;
using System.Collections.Generic;
using WeatherDataLibrary.Models;

namespace WeatherDataLibrary
{
    public class WeatherDatabaseService
    {
        public static bool LoadAllInformationFromFileToDatabase()
        {
            Console.WriteLine("\n\t\t\tTryck ENTER för att ladda in din fil i databasen.");
            Console.ReadLine();

            List<WeatherData> weatherList = FileReader.GetFileInformation();
            if (weatherList.Count == 0)
            {
                Console.WriteLine("\t\t\tCouldn't load file into database. Check that your file contains information.");
                return false;
            }
            Console.WriteLine("\n\t\t\tStartar att läsa in fil till databasen. . .");
            using (var db = new EFContext())
            {
                try
                {
                    for (int i = 0; i < weatherList.Count; i++)
                    {

                        db.Add(weatherList[i]);
                    }

                    db.SaveChanges();
                }
                catch (Exception)
                {
                    Console.WriteLine("\t\t\tCouldn't load file into database.");
                    return false;
                }

            }
            FileReader.SetDataBaseUploadDone();
            Console.WriteLine("\n\t\t\tFile succesfully loaded into database.");
            return true;
        }

        
    }
}
