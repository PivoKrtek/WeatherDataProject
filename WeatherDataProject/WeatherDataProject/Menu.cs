using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDataProject
{
    class Menu
    {
        public static void ShowMenu()
        {
            int speedMenu = 150;
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t\t\tMENY över information");
            Console.WriteLine("\n\t\t\t[1] --> Medeltemperatur för valbart datum");
            System.Threading.Thread.Sleep(speedMenu);
            Console.WriteLine("\t\t\t[2] --> Sortering av varmast till kallaste dagen, baserat på medeltemperatur (ute/inne)");
            System.Threading.Thread.Sleep(speedMenu);
            Console.WriteLine("\t\t\t[3] --> Sortering av torrast till fuktigaste dagen, baserat på medelfuktighet (ute/inne)");
            System.Threading.Thread.Sleep(speedMenu);
            Console.WriteLine("\t\t\t[4] --> Sortering av minst till störst risk för mögel (ute/inne)");
            System.Threading.Thread.Sleep(speedMenu);
            Console.WriteLine("\t\t\t[5] --> Information/datum om inträdandet av meteorologisk höst");
            System.Threading.Thread.Sleep(speedMenu);
            Console.WriteLine("\t\t\t[6] --> Information/datum om inträdandet av meteorologisk vinter");
            System.Threading.Thread.Sleep(speedMenu);
            Console.WriteLine("\t\t\t[7] --> Sortering av då ytter- och innertemperatur skiljt sig som mest");
            System.Threading.Thread.Sleep(speedMenu);
            Console.WriteLine("\t\t\t[8] --> Simulering/beräkning av hur lång tid balkongdörren varit öppen/dag alla dagar");
            System.Threading.Thread.Sleep(speedMenu);
            Console.WriteLine("\t\t\t[9] --> Detaljerad information om öppen balkongdörr för valbart datum\n");
            System.Threading.Thread.Sleep(speedMenu);
            Console.WriteLine("\t\t\t[0] --> EXIT\n");
            Console.WriteLine("\t\t\tTryck siffra samt ENTER motsvarande önskat val.");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Intro()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\t\t\t __   __  _||_||_  ______   _______  ______    ______   _______  _______  _______ ");
            Console.WriteLine("\t\t\t|  | |  ||   _   ||      | |       ||    _ |  |      | |   _   ||       ||   _   |");
            Console.WriteLine("\t\t\t|  |_|  ||  |_|  ||  _    ||    ___||   | ||  |  _    ||  |_|  ||_     _||  |_|  |");
            Console.WriteLine("\t\t\t|       ||       || | |   ||   |___ |   |_||_ | | |   ||       |  |   |  |       |");
            Console.WriteLine("\t\t\t|       ||       || |_|   ||    ___||    __  || |_|   ||       |  |   |  |       |");
            Console.WriteLine("\t\t\t |     | |   _   ||       ||   |___ |   |  | ||       ||   _   |  |   |  |   _   |");
            Console.WriteLine("\t\t\t  |___|  |__| |__||______| |_______||___|  |_||______| |__| |__|  |___|  |__| |__|\n");
            Console.WriteLine("\t\t--------------------------------------------------------------------------------------------------\n");
            System.Threading.Thread.Sleep(1000);

        }
        public static void InformationAccess(string startDate, string endDate)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\t\t\tInformation tillgänglig ifrån urval av dagar under perioden: {startDate} - {endDate}\n");
            Console.WriteLine("\t\t\tTillgängliga mätpunkter:    1. Termometer utomhus (balkong)");
            Console.WriteLine("\t\t\t\t\t\t    2. Termometer inomhus (vardagsrum)\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\t\t--------------------------------------------------------------------------------------------------\n");
            System.Threading.Thread.Sleep(150);
        }
        public static int InputNumber(int maximumChoice)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            int number = 0;
            bool ok = false;
            while (!ok)
            {
                Console.Write("\n\t\t\t-----> ");
                ok = int.TryParse(Console.ReadLine(), out number);
                if (!ok || number > maximumChoice || number < 0)
                { Console.WriteLine("\t\t\tFörsök igen.");
                    ok = false;
                }
            }
            return number;
        }
    }
}
