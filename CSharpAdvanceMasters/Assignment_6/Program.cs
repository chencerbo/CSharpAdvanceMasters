using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment_06
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            bool endApp = false;

            while (!endApp)
            {
                AnsiConsole.Write(new FigletText($"Threading").Centered().Color(Color.Aqua));

                var crcObjectLisSelection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(6)
                        .Title("[yellow]Select list of object to view[/]")
                        .MoreChoicesText("[grey](Move up and down to choose what to do)[/]")
                        .AddChoices(new[]
                        {
                            $"1 - Sugar",
                             "2 - Dakota",
                             "3 - Chief",
                             "4 - Ruby",
                             "5 - Pegasus"
                        }));

                var crcUserOptionBet= crcObjectLisSelection.GetOption();
                Console.WriteLine($"Selected list: {crcUserOptionBet.Item2}");
                Console.WriteLine("\n");

                // horse and race info
                var randomSpeeds = await HorseRandomRaceSpeed();
                var horseNames = HorseNames.ToList();
                var raceInfo = GetHorseRaceInfo(randomSpeeds.ToList(), horseNames);

                var winnerHorse = string.Empty;
                var slowestHorse = string.Empty;

                // thread
                foreach (KeyValuePair<int, string> kvp in raceInfo)
                {
                    Thread horse = new(() => CountDown(kvp.Key, kvp.Value));
                    horse.Name = kvp.Value;
                    horse.Priority = Enum.Parse<ThreadPriority>(kvp.Key.ToString());
                    horse.IsBackground = true;

                    horse.Start();

                    if (kvp.Key == (int)ThreadPriority.Highest) 
                    {
                        winnerHorse = kvp.Value;
                    }

                    if (kvp.Key == (int)ThreadPriority.Lowest) 
                    {
                        slowestHorse = kvp.Value;
                    }
                }

                // display progress bar
                ShowProgressBar(raceInfo);

                // user's bet and winner and slowest horse
                if (string.Compare(crcUserOptionBet.Item2, winnerHorse, StringComparison.OrdinalIgnoreCase) == 0) 
                {
                    Console.WriteLine($"Way to go, you bet on {winnerHorse}, the winner horse!");
                }
                else
                {
                    Console.WriteLine($"Unforunately, you bet on {slowestHorse} and didn't win.");
                }

                Console.WriteLine("\n\n--------------------------------------------------\n");

                // Wait for the user to respond before closing.                
                if (AnsiConsole.Confirm("Do you want to end the application?")) endApp = true;

                Console.WriteLine("\n");
            }
        }

        public class Horse 
        {
            public string Name { get; set; }
            public string Color { get; set; }
            public int Speed { get; set; }

            public Horse(string name, string color, int speed) 
            {
                Name = name;
                Color = color;
                Speed = speed;
            }
        }

        public static string[] HorseNames = { "Sugar", "Dakota", "Chief", "Ruby", "Pegasus"};

        public static async Task<IEnumerable<int>> HorseRandomRaceSpeed() 
        {
            Random r = new Random();
            var horseSpeedValues = new List<int>(new int[] 
            { 
                (int)ThreadPriority.Lowest, 
                (int)ThreadPriority.BelowNormal,
                (int)ThreadPriority.Normal,
                (int)ThreadPriority.AboveNormal,
                (int)ThreadPriority.Highest
            });

            var randomInterval = await Task.Run(() => horseSpeedValues.OrderBy(x => r.Next()).Take(5));

            return randomInterval;
        }

        public static Dictionary<int, string> GetHorseRaceInfo(List<int> raceSpeed, List<string> horseName) 
        {
            return raceSpeed.Zip(horseName, (k, v) => new { k, v })
              .ToDictionary(x => x.k, x => x.v);
        }

        public static void CountDown(int speed, string name) 
        {
            Console.WriteLine($"The priority of {name} is: {Enum.Parse<ThreadPriority>(speed.ToString())}");
            Console.WriteLine("Value = {0}, Key = {1}", speed, name);
        }

        public static async void ShowProgressBar(Dictionary<int, string> raceInfo) 
        {
            await AnsiConsole.Progress()
               .StartAsync(async ctx =>
               {
                   var horseName = raceInfo.Values.ToList();
                   var task1 = ctx.AddTask($"[green]{horseName[0]} running[/]");
                   var task2 = ctx.AddTask($"[yellow]{horseName[1]} running[/]");
                   var task3 = ctx.AddTask($"[dodgerblue1]{horseName[2]} running[/]");
                   var task4 = ctx.AddTask($"[orange3]{horseName[3]} running[/]");
                   var task5 = ctx.AddTask($"[violet]{horseName[4]} running[/]");

                   while (!ctx.IsFinished)
                   {
                       // Simulate some work
                       Thread.Sleep(250);
        
                       var randomRaceSpeed = raceInfo.Keys
                       .Select(i => (double)i).ToList();

                       int index = randomRaceSpeed.FindIndex(s => s == 0);
                       // Need to check if priority is zero, no value for progress bar
                       if (index != -1) randomRaceSpeed[index] = 0.50;

                       // Increment
                       task1.Increment(randomRaceSpeed[0]);
                       task2.Increment(randomRaceSpeed[1]);
                       task3.Increment(randomRaceSpeed[2]);
                       task4.Increment(randomRaceSpeed[3]);
                       task5.Increment(randomRaceSpeed[4]);

                   }
               });
        }
    }

    public static class ObjectExtensions
    {
        public static (int, string) GetOption(this string crcOptionName)
        {
            var crcOptionFirst = crcOptionName.Split('-').First().Trim();
            var crcOptionSecond = crcOptionName.Split('-').Last().Trim();

            return (Convert.ToInt32(crcOptionFirst), crcOptionSecond);
        }
    }
}