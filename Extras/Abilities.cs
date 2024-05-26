// using System;
// using System.Collections.Generic;
// using static MushroomPocket.MainExtra;

// namespace MushroomPocket
// {
//     public class Abilities
//     {
//         List<string> abilities = new List<string>() { "Leadership", "Strength", "Agility", "Magic Abilities", "Combat Skills", "Precision and Accuracy" };

//         public void Ability(string skill, double hp, double charCurrentDamage, double charGetHitDamage = 0)
//         {
//             if (abilities.Contains(name) == false)
//             {
//                 PrintColor("Invalid Ability", ConsoleColor.Red);
//                 return;
//             }


//             if (skill == "Leadership")
//             {
//                 // Leadership : Daisy
//                 Daisy(hp, charGetHitDamage);

//             }
//             else if (skill == "Strength")
//             {
//                 // Strength : Wario
//             }
//             else if (skill == "Agility")
//             {
//                 // Agility : Waluigi

//             }
//             else if (skill == "Magic Abilities")
//             {
//                 // Magic Abilities : Peach

//             }
//             else if (skill == "Combat Skills")
//             {
//                 // Combat Skills : Mario

//             }
//             else if (skill == "Precision and Accuracy")
//             {
//                 // Precision and Accuracy : Luigi

//             }
//         }

//         protected static void Daisy(double Hp, double damaged)
//         {
//             Console.WriteLine("Defending...");

//             if (Hp > 0)
//             {
//                 double defend = 1.3;
//                 Hp -= damaged / defend;
//                 Console.WriteLine($"{Name} Defended {Hp} HP");
//                 return;
//             }
//             else
//             {
//                 Console.WriteLine("Cannot defend");
//             }
//         }
//     }
// }