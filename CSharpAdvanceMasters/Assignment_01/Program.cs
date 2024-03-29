﻿using System.Text.Json;
using System;
using Spectre.Console;
using read_json_file.Extensions;
using System.Collections.Generic;

namespace Assignment_01
{
    public class Program
    {
        delegate string StatementDelegate(string m);
        static void Main()
        {
            bool endApp = false;
            
            while (!endApp)
            {
                string crcFileName = @"./Data/Videos.json";
                string crcVideoFile = File.ReadAllText(crcFileName);
                var crcMarvelVideos = crcVideoFile.DeserializeObject<IList<Video>>(); // try to turn to an extension method

                AnsiConsole.Write(
                    new FigletText($"MARVEL UNIVERSE")
                        .Centered()
                        .Color(Color.Red));

                AnsiConsole.Write(
                    new FigletText($"Video Rental Store")
                        .Centered()
                        .Color(Color.Aqua));
               
                GetAppDetails();
                Console.WriteLine("\n");

                var crcToDo = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()    // change according to what event
                        .PageSize(5)
                        .MoreChoicesText("[grey](Move up and down to choose what to do)[/]")
                        .AddChoices(new[]
                        {
                            "1 - Rent a Video",
                            "2 - Display Video List"
                        }));

                AnsiConsole.WriteLine(crcToDo);

                var crcToDoOptionNum = crcToDo.GetNumericOption();
                Console.WriteLine($"Selection: {crcToDoOptionNum}");
                Console.WriteLine("\n");

                switch (crcToDoOptionNum)
                {
                    case 1:
                        // Displays selection of movies
                        var crcMovieSelection = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()    // change according to what event
                           .PageSize(10)
                           .MoreChoicesText("[blue](Move up and down to choose a movie to rent)[/]")
                           .AddChoices(crcMarvelVideos.Select(m => $"{m.Id} - {m.Title} ({m.Year})")));

                        AnsiConsole.WriteLine(crcMovieSelection);
                        var crcMovieOptionNum = crcMovieSelection.GetNumericOption();
                        Console.WriteLine($"Selection: {crcMovieOptionNum}");
                        Console.WriteLine("\n");

                        Console.WriteLine($"Ready for rental...");

                        var crcMovie = crcMarvelVideos.Where(m => m.Id == crcMovieOptionNum).FirstOrDefault();

                        // Call event to rent for a movie
                        RentalExtendingEventArgs.RentalEvent(crcMovie.Title, crcMovie.Year);

                        crcMarvelVideos.Remove(crcMovie);
                        File.WriteAllText(crcFileName, crcMarvelVideos.ToJsonString());

                        break;

                    case 2:
                        Console.WriteLine($"--- List of Available Movies --- ");
                        Console.WriteLine("=====================================\n");

                        foreach (var crcVideo in crcMarvelVideos)
                        {                            
                            Console.WriteLine($"Title: {crcVideo.Title}");
                            Console.WriteLine($"Year: {crcVideo.Year}");
                            Console.WriteLine("=====================================\n");
                        }
                        break;
                    default:
                        throw new InvalidOperationException("Invalid option, please select from the available options.");

                }

                Console.WriteLine("--------------------------------------------------\n");

                // Wait for the user to respond before closing.                
                if (AnsiConsole.Confirm("Do you want to end the application?")) endApp = true;

                Console.WriteLine("\n");
            }
            return;
        }

        public static void GetAppDetails() 
        {
            var table = new Table().Centered();

            AnsiConsole.Live(table)
                .Start(ctx =>
                {
                    table.AddColumn("Assignment No.");
                    ctx.Refresh();
                    Thread.Sleep(1000);

                    table.AddColumn("Author");
                    ctx.Refresh();
                    Thread.Sleep(1000);

                    table.AddColumn("Date");
                    ctx.Refresh();
                    Thread.Sleep(1000);

                    table.AddColumn("Description");
                    ctx.Refresh();
                    Thread.Sleep(1000);

                    table.AddRow("1 & 2", "Chen Cerbo", "02/17/23", "Console app that monitors movie rental store");
                    ctx.Refresh();
                    Thread.Sleep(1000);
                });
        }
    }
}