using Pets;
using Spectre.Console;
using System.ComponentModel;
using System.Reflection;

namespace Assignment_02
{
    public class Program
    {
        static void Main(string[] args)
        {
            bool endApp = false;

            while (!endApp)
            {
                AnsiConsole.Write(new FigletText($"Pets and Abilities").Centered().Color(Color.DarkSeaGreen));

                //GetAppDetails();
                Console.WriteLine("\n");

                var crcFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pets.dll");
                Assembly crcAssembly = Assembly.LoadFrom(crcFilePath);

                var crcPetSelection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>() 
                        .PageSize(5)
                        .Title("[yellow]Select a pet[/]")
                        .MoreChoicesText("[grey](Move up and down to choose what to do)[/]")
                        .AddChoices(new[]
                        {
                            $"1 - Dog",
                            "2 - Bird",
                            "3 - Cat",
                            "4 - Fish"
                        }));

                var crcPetOptionName = crcPetSelection.GetOption();
                Console.WriteLine($"Selected pet: {crcPetOptionName}");
                Console.WriteLine("\n");

                //Parameter can be (namespace.class) via assembly qualified name
                Type? T = crcAssembly.GetType($"Pets.{crcPetOptionName}");
                object? crcPetInstance = Activator.CreateInstance(T, null);

                // Display class name
                var className = T.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                    .FirstOrDefault() as DisplayNameAttribute;

                Console.WriteLine($"Class: {className?.DisplayName}");

                var abilities = new List<string>();

                // Display properties and abilities according to type of pet
                switch (crcPetInstance)
                {
                    case Dog d:
                        Console.WriteLine($"Name: {d.Name}");
                        Console.WriteLine($"Age: {d.Age}");
                        abilities = ShowAbilities(typeof(Dog));
                        break;

                    case Cat c:
                        Console.WriteLine($"Name: {c.Name}");
                        Console.WriteLine($"Age: {c.Age}");
                        abilities = ShowAbilities(typeof(Cat));
                        break;

                    case Bird b:
                        Console.WriteLine($"Name: {b.Name}");
                        Console.WriteLine($"Age: {b.Age}");
                        abilities = ShowAbilities(typeof(Bird));
                        break;

                    case Fish f:
                        Console.WriteLine($"Name: {f.Name}");
                        Console.WriteLine($"Age: {f.Age}");
                        abilities = ShowAbilities(typeof(Fish));
                        break;

                    default:
                        throw new InvalidOperationException("Invalid option, please select from the available options.");
                }

                Console.WriteLine("\n");

                GetSelectedPetAbilities(abilities, T, crcPetInstance);

                Console.WriteLine("\n\n--------------------------------------------------\n");

                // Wait for the user to respond before closing.                
                if (AnsiConsole.Confirm("Do you want to end the application?")) endApp = true;

                Console.WriteLine("\n");
            }
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

                    table.AddRow("3 & 4", "Chen Cerbo", "02/28/23", "Console app that list animals with properties and their abilities");
                    ctx.Refresh();
                    Thread.Sleep(1000);
                });
        }

        public static List<string> ShowAbilities(Type crcType) 
        {
            int ctr = 1;
            var listOfAbilities = new List<string>();

            Console.WriteLine("Abilities");

            MethodInfo[] methods = crcType.GetMethods();
            foreach (MethodInfo method in methods)
            {
                if (method.ReturnType == typeof(void))
                {
                    string methodOptionName = $"{ctr++}-{method.Name}";
                    Console.WriteLine(methodOptionName);
                    listOfAbilities.Add(methodOptionName);
                }
            }

            return listOfAbilities;
        }

        public static void GetSelectedPetAbilities(List<string> abilities, Type type, object obj) 
        {
            var crcAbilitySelection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(5)
                        .Title("[yellow]Select ability[/]")
                        .MoreChoicesText("[grey](Move up and down to choose what to do)[/]")
                        .AddChoices(abilities.ToArray()));

            //AnsiConsole.WriteLine(crcAbilitySelection);

            var crcAbilityOptionName = crcAbilitySelection.GetOption();
            Console.WriteLine($"Selected ability of pet: {crcAbilityOptionName}");

            if (isPetAbility(type.GetMethods(), crcAbilityOptionName))
            {
                MethodInfo? methodInfo = type.GetMethod(crcAbilityOptionName);
                _ = methodInfo.Invoke(obj, null);
            }
        }

        public static bool isPetAbility(MethodInfo[] methods, string selected) => 
            methods.Where(m => m.ReturnType == typeof(void)).Any(m => m.Name.Equals(selected));
    }

    public static class ObjectExtensions 
    {
        public static string GetOption(this string crcOptionName)
        {
            var crcOption = crcOptionName.Split('-').Last().Trim();

            return crcOption;
        }
    }
}