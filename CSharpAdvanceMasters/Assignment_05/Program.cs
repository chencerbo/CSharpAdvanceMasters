using System.Net.Http.Headers;
using Newtonsoft.Json;
using Spectre.Console;
using System.Net;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;

namespace Assignment_05
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            AnsiConsole.Write(new FigletText($"Async Programming").Centered().Color(Color.Aqua));

            var taskItems = new Task[] {
                GetResponseFromApi("https://official-joke-api.appspot.com/random_joke"),
                LoopEverySeconds(), // --- Check Output tab for logs
                CalculateNumbers(5, 5),
                UpdatePrivateValue("John Doe", 27),
                AddItemsToCollection(new List<string>() { "Value1", "Value2", "Value3" }),
                DownloadFile() // --- Check Output tab for logs
            };

            await AnsiConsole.Progress()
                .StartAsync(async ctx =>
                {
                    // Define tasks
                    var task1 = ctx.AddTask("[green]Getting all tasks[/]", autoStart: true, maxValue: 100);

                    while (!ctx.IsFinished)
                    {
                        await Task.Delay(250);

                        // Simulate some work
                        await Task.WhenAll(taskItems);

                        // Increment
                        task1.Increment(1.5);
                    }
                });
        }

        // Return response from API
        public static async Task GetResponseFromApi(string requestUrl)
        {
            HttpClient client = new HttpClient();

            // Add an accept header for json format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Response
            HttpResponseMessage response = client.GetAsync(requestUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var jsonObject = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Joke>(jsonObject);

                Console.WriteLine($"Q: {data.Setup} \nA: {data.Punchline}");
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            // Dispose once all HttpClient calls are complete
            client.Dispose();
        }

        public class Joke
        {
           public string Type { get; set; }
           public string Setup { get; set; }
           public string Punchline { get; set; }  
           public int Id { get; set; }
        }

        // Loop every second
        public static async Task LoopEverySeconds() 
        {
            await Task.Run(() =>
            {
                var count = 1;
                Timer timer = new Timer(x => { callTimerMethod(count++);});
                timer.Change(1, 20);
            });
        }
        private static void callTimerMethod(int count)  
        {
            System.Diagnostics.Debug.Write(string.Format("\n-----> Timer Executed {0} times.", count));
        }  

        // Calculate sum of two numbers
        public static async Task CalculateNumbers(int num1, int num2) 
        {
            await Task.Run(() => Console.WriteLine($"Sum of two numbers: {num1 + num2}"));
        }

        // Update private variable
        public static async Task UpdatePrivateValue(string name, int age) 
        {
            await Task.Run(() => 
            {
                var result = new Person(name, age);
                Console.WriteLine($"Updated value for:\nName: {result.Name}, Age: {result.Age}");
            });
        }

        public class Person
        {
            public string Name { get; private set; }

            public int Age { get; private set; }
            public Person(string Name, int Age)
            {
                this.Name = Name;
                this.Age = Age;
            }
        }

        // Add values to any collection
        public static async Task AddItemsToCollection(List<string> collection)
        {
            await Task.Run(() => 
            {
                var newCollection = new List<string>();

                foreach (var oldItem in collection) 
                {
                    newCollection.Add(oldItem);
                }

                foreach (var item in newCollection)
                {
                    Console.Write($"{item} ");
                }
            });
        }

        public static async Task DownloadFile()
        {
            HttpClient client = new HttpClient();

            using (var stream = await client.GetStreamAsync("https://via.placeholder.com/300.png"))
            {
                using (var fileStream = new FileStream("testfile.png", FileMode.OpenOrCreate))
                {
                    // kindly check file into debug folder
                    await stream.CopyToAsync(fileStream).ConfigureAwait(false);
                }
            }

            System.Diagnostics.Debug.Write("\n=========***FILE DOWNLOADED***============");
        }
    }
}