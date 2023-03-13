using Assignment_04.DAL;
using Assignment_04.Events;
using Assignment_04.Model;
using Assignment_04.Service;
using Assignment_04.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Assignment_04
{
    public class Program
    {   
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configBuilder) =>
                {
                    configBuilder.SetBasePath(Directory.GetCurrentDirectory());
                    configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    var connectionString = hostingContext.Configuration.GetConnectionString("DefaultConnection");
                    services.AddDbContext<VideoContext>(v => v.UseSqlServer(connectionString));

                    services.TryAddScoped<IVideoDAL, VideoDAL>();
                    services.TryAddScoped<IVideoService, VideoService>();
                    services.TryAddTransient<MyProgram>();
                    services.AddLogging(l => l.ClearProviders());

                    var serviceProvider = services.BuildServiceProvider();

                    // entry to run app
                    var program = serviceProvider.GetService<MyProgram>();

                    bool endApp = false;

                    while (!endApp)
                    {
                        program?.Run();

                        // Wait for the user to respond before closing.                
                        if (AnsiConsole.Confirm("Do you want to end the application?")) endApp = true;

                        Console.WriteLine("\n");
                    }

                    return;
                });

        public class MyProgram
        {
            private readonly IVideoService _videoService;

            public MyProgram(IVideoService videoService)
            {
                _videoService = videoService;
            }
            public void Run()
            {
                AnsiConsole.Write(
                    new FigletText($"MARVEL UNIVERSE")
                        .Centered()
                        .Color(Color.Red));

                AnsiConsole.Write(
                    new FigletText($"Video Rental Store")
                        .Centered()
                        .Color(Color.Aqua));

                Console.WriteLine("\n");

                var crcToDo = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()    // change according to what event
                        .PageSize(5)
                        .MoreChoicesText("[grey](Move up and down to choose what to do)[/]")
                        .AddChoices(new[]
                        {
                                "1 - Display ALL Videos",
                                "2 - Get Specific Video (Order By Year)",
                                "3 - Add Video",
                                "4 - Rent a Video",
                                "5 - Delete a Video"
                        }));

                AnsiConsole.WriteLine(crcToDo);

                var crcToDoOptionNum = crcToDo.GetNumericOption();
                Console.WriteLine($"Selection: {crcToDoOptionNum}");
                Console.WriteLine("\n");

                var crcMarvelVideos = _videoService.GetAllVideos().ToList();

                switch (crcToDoOptionNum)
                {
                    case 1:
                        AnsiConsole.WriteLine($"LIST OF AVAILABLE MOVIES");
                        AnsiConsole.WriteLine("=====================================================\n");

                        crcMarvelVideos.ForEach(item =>
                        {
                            AnsiConsole.Markup($"[darkcyan]Id: {item.Id}\nTitle: {item.Title}\nYear: {item.Year}\n[/]");
                            AnsiConsole.WriteLine("=====================================================\n");
                        });
                        break;

                    case 2:
                        AnsiConsole.WriteLine($"GET MOVIE BY YEAR");
                        AnsiConsole.WriteLine("=====================================================\n");

                        var listedYear = AnsiConsole.Ask<int>("Enter a year of [green]movie[/]?");
                        var crcAvailableMovieByYear = _videoService.GetVideoByYear(listedYear).ToList();

                        crcAvailableMovieByYear.ForEach(item =>
                        {
                            AnsiConsole.Markup($"[darkcyan]Id: {item.Id}\nTitle: {item.Title}\nYear: {item.Year}\n[/]");
                            AnsiConsole.WriteLine("=====================================================\n");
                        });
                        break;

                    case 3:
                        var title = AnsiConsole.Ask<string>("Enter title of [green]movie[/]?");
                        var year = AnsiConsole.Ask<int>("Enter year of [green]movie[/]?");

                        if (_videoService.AddVideo(title, year)) 
                        {
                            Console.WriteLine("New movie was added.");
                        }
                        break;

                    case 4:
                        var crcRentMovie = GetSelectedMovie(crcMarvelVideos, "rent");

                        // Call event to rent for a movie
                        RentalExtendingEventArgs.RentalEvent(crcRentMovie.Item1?.Title, crcRentMovie.Item1?.Year);
                        if (_videoService.RentVideo(crcRentMovie.Item2, isRented: true)) 
                        {
                            Console.WriteLine("Movie was rented.");
                        }
                        break;

                    case 5:
                        Console.WriteLine($"\nReady for delete...");
                        var crcDeleteMovie = GetSelectedMovie(crcMarvelVideos, "delete");

                        if (_videoService.DeleteVideo(crcDeleteMovie.Item2, isDeleted: true))
                        {
                            Console.WriteLine("Movie was deleted.");
                        }
                        break;

                    default:
                        throw new InvalidOperationException("Invalid option, please select from the available options.");

                }

                Console.WriteLine("-----------------------------------------------------\n");
            }

            public (Video?, int?) GetSelectedMovie(List<Video> crcMarvelVideos, string crcDisplay) 
            {
                // Displays selection of movies
                var crcMovieSelection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()    // change according to what event
                   .PageSize(10)
                   .MoreChoicesText($"[blue](Move up and down to choose a movie to {crcDisplay})[/]")
                   .AddChoices(crcMarvelVideos.Select(m => $"{m.Id} - {m.Title} ({m.Year})")));

                AnsiConsole.WriteLine(crcMovieSelection);
                var crcMovieOptionNum = crcMovieSelection.GetNumericOption();
                Console.WriteLine($"Selection: {crcMovieOptionNum}");

                var crcMovie = crcMarvelVideos.Where(m => m.Id == crcMovieOptionNum).FirstOrDefault();

                return (crcMovie, crcMovieOptionNum);
            }
        }
    }
    public static class ObjectExtensions
    {
        public static int GetNumericOption(this string crcOptionName)
        {
            var crcOptionNum = crcOptionName.Split('-').First();

            return Convert.ToInt32(crcOptionNum);
        }
    }
}