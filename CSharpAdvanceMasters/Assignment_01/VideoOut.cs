﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_01
{
    public class RentalEventArgs : EventArgs
    {
        public Video RentedMovie { get; set; }
    }

    public class RentalExtendingEventArgs
    {
        private event EventHandler<RentalEventArgs> OnRental;
        public static void RentalEvent(string crcMovieTitle, string crcMovieYear) 
        {
            var rent = new RentalExtendingEventArgs();

            rent.OnRental += VideoOut.RemoveRentedMovieFromList;
            rent.OnRental += VideoOut.SendEmail;
            rent.OnRental += VideoOut.DisplayMessage;

            rent.CreateDetails(crcMovieTitle, crcMovieYear);
        }

        public void CreateDetails(string crcMovieTitle, string crcMovieYear) 
        {            
            //3. raise an event and a new instance of the OrderEventArgs class
            if (OnRental != null)
            {
                OnRental(this, new RentalEventArgs { RentedMovie = new Video { Title = crcMovieTitle, Year = crcMovieYear } });
            }
        }
    }
    public class VideoOut
    {
        public static void RemoveRentedMovieFromList(object sender, RentalEventArgs e) 
        {
            Console.WriteLine($"Removing from list with movie details: {e.RentedMovie.Title}, {e.RentedMovie.Year}");
            // TODO: Call another method/class that will remove rented movie details from json file.
        }

        public static void SendEmail(object sender, RentalEventArgs e)
        {
            Console.WriteLine($"Sending an email with movie details: {e.RentedMovie.Title}, {e.RentedMovie.Year}");
            // Call MailService.cs to send email with details of rented movie
            EmailExtendingEventArgs.EmailEvent("chen.cerbo@gmail.com", "cerbochen@gmail.com", "Test Movie Rental", $"You've rented a movie with details: {e.RentedMovie.Title}, {e.RentedMovie.Year}");
        }

        public static void DisplayMessage(object sender, RentalEventArgs e)
        {
            Console.WriteLine($"Displaying message to console with movie details: {e.RentedMovie.Title}, {e.RentedMovie.Year}");
            // Call MessageService.cs to displat message with details of rented movie
            MessageExtendingEventArgs.MessageEvent($"{e.RentedMovie.Title}, {e.RentedMovie.Year}");
        }
    }
}
