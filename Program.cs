using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;


using static MushroomPocket.MainExtra;



// Name: Kai Jie
// Admin No: 234412H

namespace MushroomPocket
{
    class Program
    {
        static void Main(string[] args)
        {
            printOwner.PrintOwner();
            Thread.Sleep(800);
            Console.ForegroundColor = ConsoleColor.White;
            //MushroomMaster criteria list for checking character transformation availability.   
            /*************************************************************************
                PLEASE DO NOT CHANGE THE CODES FROM LINE 15-19
            *************************************************************************/
            List<EvolvedMushroom> mushroomMasters = new List<EvolvedMushroom>(){
                new Peach(),
                new Mario(),
                new Luigi()
            };

            //Use "Environment.Exit(0);" if you want to implement an exit of the console program
            //Start your assignment 1 requirements below.


            // Dictionary to store the character transformation abilites.
            Dictionary<string, Dictionary<string, string>> EvolveCharactersSkills = new Dictionary<string, Dictionary<string, string>>();
            foreach (var mushroomMaster in mushroomMasters)
            {
                EvolveCharactersSkills.Add(mushroomMaster.TransformTo, new Dictionary<string, string> { { "NoToTransfrom", mushroomMaster.NoToTransform.ToString() }, { "Skill", mushroomMaster.Skill }, { "Damage", mushroomMaster.Damage.ToString() } });
            };



            // Start the program
            using var context = new MushroomDbContext();


            bool isConnected = context.IsDatabaseConnected();

            if (isConnected) Console.WriteLine("Database connected."); else throw new Exception("Database not connected.");

            Users user = null;

            List<MushroomHeroes> heroes = new List<MushroomHeroes>();
            List<EnemyCharacters> enemyCharacters = [new Goomba(), new Koopa(), new Bowser()];

            loadEnemies(enemyCharacters);

            while (user == null)
            {
                Console.Clear();

                AsciiArt.PrintMushroom();
                PrintColor(string.Concat(Enumerable.Repeat("-", 65)));

                PrintColor("(1) Login");
                PrintColor("(2) Register");
                PrintColor("(Q) Quit");

                PrintColor("Please only enter (1, 2): ", newLine: false);

                var key = Console.ReadKey(false);
                string choice = key.KeyChar.ToString();
                PrintColor("");

                if (choice == "Q" || choice == "q")
                {
                    PrintColor("Goodbye!");
                    Environment.Exit(0);
                }
                else if (choice == "2")
                {
                    PrintColor("\nRegistration");
                    user = Register();
                }
                else if (choice == "1")
                {
                    user = Login();
                }
                else
                {
                    PrintColor("Invalid input! Please only enter (1, 2).");
                }
            }

            Dictionary<string, string> eee = new Dictionary<string, string>() { { "warrior", "mage" } };

            eee.TryGetValue("warrior", out string value);


            while (true)
            {
                Console.Clear();

                AsciiArt.PrintMushroom();
                PrintColor(string.Concat(Enumerable.Repeat("-", 65)));
                PrintColor($"Welcome, ", newLine: false);
                PrintColor($"{user.Username}!", ConsoleColor.DarkGreen);

                PrintColor("(1) Add Mushroom Master");
                PrintColor("(2) List Character(s) in my poacket");
                PrintColor("(3) Check if i can transform my character");
                PrintColor("(4) Transform Character(s)");
                PrintColor("(5) Remove Character(s) from my pocket");
                PrintColor("(6) Attack Enemy Character (Offline PvE)");
                PrintColor("(m) Mutliplayer Mode (P2P Battle)", ConsoleColor.DarkYellow);
                PrintColor("(0) Reset Database (Caution: This will delete all data)", ConsoleColor.Red);
                PrintColor("(Q) Quit");


                PrintColor("Please only enter (1,2,3,4,5,6]): ", newLine: false);

                try
                {
                    var key = Console.ReadKey(false);
                    string choice = key.KeyChar.ToString();
                    Console.WriteLine();

                    switch (choice)
                    {
                        case "0":
                            resetDatabase();
                            loadEnemies(enemyCharacters);
                            break;
                        case "Q" or "q":
                            Console.WriteLine("\nGoodbye!");
                            Environment.Exit(0);
                            break;
                        case "1":
                            Console.Clear();
                            MushroomFunc.AddMushroomMaster(heroes, user);

                            break;
                        case "2":
                            Console.Clear();
                            MushroomFunc.listCharacter(heroes, user);
                            ContinueWhenPressed();
                            break;
                        case "3":
                            Console.Clear();
                            MushroomFunc.checkTransform(heroes, mushroomMasters, EvolveCharactersSkills, user);
                            ContinueWhenPressed();
                            break;
                        case "4":
                            Console.Clear();
                            MushroomFunc.TransformCharacter(heroes, mushroomMasters, EvolveCharactersSkills, user);
                            ContinueWhenPressed();
                            break;
                        case "5":
                            Console.Clear();
                            MushroomFunc.removeCharacter(heroes, user);
                            ContinueWhenPressed();
                            break;
                        case "6":
                            Console.Clear();
                            MushroomFunc.attack(user);
                            ContinueWhenPressed();
                            break;
                        case "m" or "M":
                            Console.Clear();
                            MushroomP2P.Multiplayer(user);
                            break;
                        default:
                            throw new Exception("Invalid input! Please only enter [1,2,3,4,5,6] or Q to quit.");
                    }

                }
                catch (Exception ex)
                {
                    PrintColor(ex.Message);
                    ContinueWhenPressed();
                }

                Console.WriteLine();
            }
        }



        static string PasswordCensored()
        {
            PrintColor("Enter your password: ", newLine: false);
            string password = "";
            while (true)
            {
                var key = Console.ReadKey(true);
                // Break the loop if Enter key is pressed
                if (key.Key == ConsoleKey.Enter) break;

                // If Backspace is pressed, remove the last char from password
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        password = password[..^1]; // remove last char from password
                        PrintColor("\b \b", newLine: false); // remove last asterisk from console
                    }
                }
                else
                {
                    password += key.KeyChar;
                    PrintColor("*", newLine: false);
                }
            }
            if (string.IsNullOrEmpty(password))
            {
                PrintColor("\nPassword cannot be empty!");
                return PasswordCensored();
            }
            return password;
        }

        public static Users Login()
        {
            using (var context = new MushroomDbContext())
            {
                while (true)
                {
                    PrintColor("Enter 'B' to go back.");
                    PrintColor(string.Concat(Enumerable.Repeat("-", 30)));
                    PrintColor("Enter your username: ", newLine: false);
                    string username = Console.ReadLine();

                    if (username == "b" || username == "B")
                    {
                        return null;
                    }
                    if (string.IsNullOrEmpty(username))
                    {
                        PrintColor("Username cannot be empty!");
                        continue;
                    }

                    var password = PasswordCensored();
                    PrintColor("");

                    var user = context.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

                    if (user != null)
                    {

                        PrintColor("Login successful!");

                        return user;
                    }
                    else
                    {
                        PrintColor("Invalid username or password.");
                        continue;
                    }
                }
            }
        }

        public static Users Register()
        {
            using (var context = new MushroomDbContext())
            {
                while (true)
                {
                    PrintColor("Enter 'B' to go back.");
                    PrintColor(string.Concat(Enumerable.Repeat("-", 30)));
                    PrintColor("Enter your username: ", newLine: false);
                    string username = Console.ReadLine();
                    if (username == "b" || username == "B")
                    {
                        return null;
                    }
                    if (string.IsNullOrEmpty(username))
                    {
                        PrintColor("Username cannot be empty!");
                        continue;
                    }

                    var user = context.Users.FirstOrDefault(x => x.Username == username);
                    // Console.WriteLine($"Exist? {user}");

                    if (user != null)
                    {
                        PrintColor("Username already exists!");
                        continue;
                    }
                    else if (string.IsNullOrEmpty(username))
                    {
                        PrintColor("Username cannot be empty!");
                        continue;
                    }
                    else
                    {
                        var password = PasswordCensored();
                        PrintColor("", newLine: true);

                        var newUser = new Users(username, password);
                        context.Users.Add(newUser);
                        context.SaveChanges();
                        PrintColor("Registration successful!");

                        ContinueWhenPressed();

                        return newUser;
                    }
                }
            }
        }

        static void resetDatabase()
        {
            Console.WriteLine("Are you sure you want to reset the database? (Y/N): ", ConsoleColor.Red);
            if (Console.ReadKey().Key != ConsoleKey.Y)
            {
                Console.WriteLine("Database reset cancelled.");
                ContinueWhenPressed();
                return;
            }

            using (var context = new MushroomDbContext())
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();

            }

            Console.WriteLine("Database reset successful!", ConsoleColor.Green);
            ContinueWhenPressed();
            Environment.Exit(0);
        }

        static void loadEnemies(List<EnemyCharacters> enemyCharacters)
        {
            using (var context = new MushroomDbContext())
            {

                if (context.EnemyCharacters.Count() == 0)
                {
                    foreach (EnemyCharacters enemy in enemyCharacters)
                    {
                        context.EnemyCharacters.Add(enemy);
                        context.SaveChanges();
                    }
                }
                else
                {
                    return;
                }
            }
            return;
        }
    }
}