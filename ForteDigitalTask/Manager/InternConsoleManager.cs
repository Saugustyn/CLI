using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForteDigitalTask.Manager
{
    public class InternConsoleManager
    {
        private InternManagerInterface internManager;

        public InternConsoleManager(InternManagerInterface internManager)
        {
            this.internManager = internManager;
        }
        public void Start()
        {
            string arg = Console.ReadLine();
            string[] args = arg.Split(' ');
            if (arg == null)
            {
                Console.WriteLine("Error: No input entered.");
                return;
            }

            if (args.Length < 2)
            {
                Console.WriteLine("Error: Invalid command.");
                return;
            }

            string command = args[0];
            string url = args[1];
            int? ageThreshold = null;
            bool greaterThan = false;

            if (command == "count")
            {
                if (args.Length > 3)
                {
                    string ageOption = args[2];
                    if (ageOption == "--age-gt")
                    {
                        greaterThan = true;
                    }
                    else if (ageOption == "--age-lt")
                    {
                        greaterThan = false;
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid command.");
                        return;
                    }
                    ageThreshold = int.Parse(args[3]);
                }
            }

            switch (command)
            {
                case "count":
                    {
                        int count = internManager.CountInterns(url, ageThreshold, greaterThan);
                        Console.WriteLine(count);
                    }
                    break;
                case "max-age":
                    {
                        int maxAge = internManager.MaxAge(url);
                        Console.WriteLine(maxAge);
                    }
                    break;
                default:
                    Console.WriteLine("Error: Invalid command.");
                    break;
            }
        }
    }
}
