using Microsoft.VisualBasic;
using Spectre.Console;
using System.Collections;
using System.Security.Cryptography;

namespace Assignment_03
{
    public class Program
    {
        static void Main(string[] args)
        {
            bool endApp = false;

            var crcListOfObject = new List<object>();
            var crcObjectDictionary = new Dictionary<string, List<object>>();
            var crcArrayList = new ArrayList();

            while (!endApp)
            {
                AnsiConsole.Write(new FigletText($"Collections").Centered().Color(Color.Aqua));

                Console.WriteLine("\n");

                // Start of the program                
                var crcInput = AnsiConsole.Ask<string>("[yellow]Input any type of objects[/]:");

                // Parse input to accept any type of object
                var (key, value) = GetAnyInput(crcInput);

                // Save to dictionary to sort depending on which type
                if (crcObjectDictionary.ContainsKey(key))
                {
                    crcObjectDictionary[key].Add(value);
                }
                else
                {
                    crcObjectDictionary.Add(key, new List<object> { value });
                }

                // Add any object value to an arrayList
                crcArrayList.Add(value);

                Console.WriteLine("\n");

                var crcObjectLisSelection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(6)
                        .Title("[yellow]Select list of object to view[/]")
                        .MoreChoicesText("[grey](Move up and down to choose what to do)[/]")
                        .AddChoices(new[]
                        {
                            $"1 - List of String",
                            "2 - List of Int32",
                            "3 - List of Bool",
                            "4 - List of Char",
                            "5 - List of Double",
                            "6 - NonGeneric ArrayList"
                        }));

                var crcListOptionName = crcObjectLisSelection.GetOption();
                Console.WriteLine($"Selected list: {crcListOptionName}");
                Console.WriteLine("\n");

                var crcTypeName = string.Empty;                

                switch (crcListOptionName)
                {
                    case 1:
                        crcTypeName = typeof(string).Name;
                        AnsiConsole.WriteLine($"Display list of {crcTypeName}:");
                        crcObjectDictionary.TryGetValue(crcTypeName, out crcListOfObject);

                        var crcListOfString = crcListOfObject?.Select(s => s.ToString()).ToList();
                        AnsiConsole.WriteLine(crcListOfString != null ? string.Join("\n", crcListOfString) : "No item in the string list." );
                        break;

                    case 2:
                        crcTypeName = typeof(int).Name;
                        AnsiConsole.WriteLine($"Display list of {crcTypeName}:");
                        crcObjectDictionary.TryGetValue(crcTypeName, out crcListOfObject);

                        var crcListOfInt = crcListOfObject?.Select(i => Convert.ToInt32(i)).ToList();
                        AnsiConsole.WriteLine(crcListOfInt != null ? string.Join("\n", crcListOfInt) : "No item in the int list.");
                        break;

                    case 3:
                        crcTypeName = typeof(bool).Name;
                        AnsiConsole.WriteLine($"Display list of {crcTypeName}:");                        
                        crcObjectDictionary.TryGetValue(crcTypeName, out crcListOfObject);

                        var crcListOfBool = crcListOfObject?.Select(b => Convert.ToBoolean(b)).ToList();
                        Console.WriteLine(crcListOfBool != null ? string.Join("\n", crcListOfBool) : "No item in the bool list.");
                        break;

                    case 4:
                        crcTypeName = typeof(char).Name;
                        AnsiConsole.WriteLine($"Display list of {crcTypeName}:");
                        crcObjectDictionary.TryGetValue(crcTypeName, out crcListOfObject);

                        var crcListOfChar = crcListOfObject?.Select(c => Convert.ToChar(c)).ToList();
                        AnsiConsole.WriteLine(crcListOfChar != null ? string.Join("\n", crcListOfChar) : "No item in the char list.");
                        break;

                    case 5:
                        crcTypeName = typeof(double).Name;
                        Console.WriteLine($"Display list of {crcTypeName}:");
                        crcObjectDictionary.TryGetValue(crcTypeName, out crcListOfObject);

                        var crcListOfDouble = crcListOfObject?.Select(d => Convert.ToDouble(d)).ToList();
                        Console.WriteLine(crcListOfDouble != null ? string.Join("\n", crcListOfDouble) : "No item in the double list.");
                        break;

                    case 6:
                        Console.WriteLine("Display an arrayList:");
                        Console.WriteLine(crcArrayList != null ? string.Join("\n", crcArrayList.ToArray()) : "No item in the array list.");
                        break;

                    default:
                        throw new InvalidOperationException("Invalid object type, please input valid types only.");
                }                

                Console.WriteLine("\n");
                Console.WriteLine("\n\n--------------------------------------------------\n");

                // Wait for the user to respond before closing.                
                if (AnsiConsole.Confirm("Do you want to end the application?")) endApp = true;

                Console.WriteLine("\n");
            }
        }

        public static (string, object) GetAnyInput(string crcInput) 
        {                                    
            if (bool.TryParse(crcInput, out var crcBoolResult))
            {
                return (typeof(bool).Name, crcBoolResult);                      
            }
            else if (char.TryParse(crcInput, out var crcCharResult))
            {
                return (typeof(char).Name, crcCharResult);
            }
            else if (int.TryParse(crcInput, out var crcIntResult))
            {
                return (typeof(int).Name, crcIntResult);
            }
            else if (double.TryParse(crcInput, out var crcDoubleResult))
            {
                return (typeof(double).Name, crcDoubleResult);
            }
            else
            {
                return (typeof(string).Name, crcInput);
            }
        }
    }

    public static class ObjectExtensions
    {
        public static int GetOption(this string crcOptionName)
        {
            var crcOption = crcOptionName.Split('-').First().Trim();

            return Convert.ToInt32(crcOption);
        }
    }
}