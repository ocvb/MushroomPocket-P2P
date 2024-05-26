using System;

// Name: Kai Jie
// Admin No: 234412H

namespace MushroomPocket
{
    public class MainExtra
    {
        public static void PrintColor(string text, ConsoleColor color = ConsoleColor.White, bool newLine = true)
        {
            Console.ForegroundColor = color;
            if (newLine)
                Console.WriteLine(text);
            else
                Console.Write(text);

            Console.ResetColor();
        }

        public static void ContinueWhenPressed(bool onlyOneKey = false, string customText = "Press any key to continue...")
        {
            if (onlyOneKey)
            {
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.D0 || Console.ReadKey(true).Key == ConsoleKey.NumPad0)
                    {
                        return;
                    }
                }
            }

            PrintColor(customText);
            Console.ReadKey(true);
        }
    }

    public class AsciiArt
    {
        public static void PrintMushroom()
        {
            string[] part1 = @"                                     
 _____         _                     
|     |_ _ ___| |_ ___ ___ ___ _____ 
| | | | | |_ -|   |  _| . | . |     |
|_|_|_|___|___|_|_|_| |___|___|_|_|_|".Split('\n');

            string[] part2 = @"                           
 _____         _       _   
|  _  |___ ___| |_ ___| |_ 
|   __| . |  _| '_| -_|  _|
|__|  |___|___|_,_|___|_|  ".Split('\n');

            int originalTop = Console.CursorTop;
            int originalLeft = Console.CursorLeft;

            for (int i = 0; i < part1.Length; i++)
            {
                Console.SetCursorPosition(originalLeft, originalTop + i);
                MainExtra.PrintColor(part1[i], ConsoleColor.Red, false);
            }

            for (int i = 0; i < part2.Length; i++)
            {
                Console.SetCursorPosition(originalLeft + part1[0].Length, originalTop + i);
                MainExtra.PrintColor(part2[i], ConsoleColor.Yellow, true);
            }
        }
    }

    public class printOwner()
    {
        public static void PrintOwner()
        {
            MainExtra.PrintColor("Name: Kai Jie", ConsoleColor.Blue);
            MainExtra.PrintColor("Admin No: 234412H", ConsoleColor.DarkBlue);
        }
    }
}