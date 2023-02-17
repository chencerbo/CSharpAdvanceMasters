using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace Assignment_01
{
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }
    }

    public class MessageExtendingEventArgs 
    {
        //2.define the event using the EventHandler with the OrderEventArgs type
        private event EventHandler<MessageEventArgs> OnDisplay;

        public static void MessageEvent(string msg)
        {
            var msgService = new MessageExtendingEventArgs();

            msgService.OnDisplay += MessageService.DisplayMessage;

            msgService.CreateDisplay(msg);
        }

        private void CreateDisplay(string msg)
        {
            //Raising an Event
            if (OnDisplay != null)
            {
                OnDisplay(this, new MessageEventArgs{ Message = msg});
            }
        }
    }

    public class MessageService
    {
        public static void DisplayMessage(object sender, MessageEventArgs e)
        {
            AnsiConsole.MarkupLine("You've rented movie with details: [yellow]{0}[/]", e.Message);
        }
    }
}
