using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using static MushroomPocket.MainExtra;


// Name: Kai Jie
// Admin No: 234412H


namespace MushroomPocket;
public class MushroomFunc
{

    // Add Character function

    public static void AddMushroomMaster(List<MushroomHeroes> heroes, Users user)
    {
        using (var context = new MushroomDbContext())
        {
            List<MushroomHeroes> PresetCharacters = [new Daisy(), new Wario(), new Waluigi()];

            PrintColor("Add Mushroom Characters", ConsoleColor.Green);

            PrintColor("This are the only Characters you can choose from: ");
            PrintColor("(Daisy) or ", ConsoleColor.Yellow, false);
            PrintColor("(Wario) or ", ConsoleColor.Yellow, false);
            PrintColor("(Waluigi)", ConsoleColor.Yellow, true);

            PrintColor("Enter Character's Name (Enter 'B' to back): ", newLine: false);
            string name = Convert.ToString(Console.ReadLine());

            if (name == "B" || name == "b")
            {

                throw new Exception("Exiting...");
            }

            if (PresetCharacters.Exists(x => x.Name == name))
            {
                PrintColor($"{name} exists in the preset list!");


                if (heroes.Exists(x => x.Name == name))
                {
                    throw new Exception("Character already exists in your pocket!");
                }
                else
                {
                    // Continue to add the character details to the database
                    PrintColor("Enter Character's HP: ", newLine: false);
                    string hp = Convert.ToString(Console.ReadLine());
                    PrintColor("Enter Character's XP: ", newLine: false);
                    string xp = Convert.ToString(Console.ReadLine());

                    // Check if the character exists in the preset list
                    MushroomHeroes hero = PresetCharacters.Find(x => x.Name == name);
                    PrintColor($"{name} has been added!");
                    PrintColor("");

                    hero.Hp = Convert.ToInt32(hp);
                    hero.Exp = Convert.ToInt32(xp);
                    hero.UserId = user.Id;
                    context.MushroomHeroes.Add(hero);
                    context.SaveChanges();
                }
            }
            else
            {
                throw new Exception($"{name} does not exist in the preset list!");
            }
        }
    }

    // List Character function

    public static void listCharacter(List<MushroomHeroes> heroes, Users user)
    {
        PrintColor("List of Characters in your pocket", ConsoleColor.Green);

        using (var context = new MushroomDbContext())
        {

            var currentUser = context.Users.Where(x => x.Username == user.Username).FirstOrDefault();
            heroes = context.MushroomHeroes.Where(x => x.UserId == currentUser.Id).ToList();

            if (heroes.Count() <= 0)
            {
                throw new Exception("No character in your pocket!");
            }

            heroes.Sort((x, y) => y.Hp.CompareTo(x.Hp));
            foreach (MushroomHeroes hero in heroes)
            {
                PrintColor(string.Concat(Enumerable.Repeat("-", 30)));
                PrintColor($"Name: {hero.Name} \nHP: {hero.Hp} \nEXP: {hero.Exp} \nSkill: {hero.Skill} \nDead: {(hero.isDead ? "Yes" : "No")}");
                PrintColor(string.Concat(Enumerable.Repeat("-", 30)));
            }
            return;
        }
    }

    // Check Transform function

    public static void checkTransform(List<MushroomHeroes> heroes, List<EvolvedMushroom> mushroomMasters, Dictionary<string, Dictionary<string, string>> EvolveCharactersSkills, Users user)
    {
        PrintColor("Check Transform Characters", ConsoleColor.Green);

        HashSet<string> uniqueNames = new HashSet<string>();

        var dbHeroes = new List<MushroomHeroes>();

        using (var context = new MushroomDbContext())
        {
            var currentUser = context.Users.FirstOrDefault(x => x.Username == user.Username);
            dbHeroes = context.MushroomHeroes.Where(x => x.UserId == currentUser.Id).ToList();

            if (!dbHeroes.Any())
            {
                throw new Exception("No character in your pocket!");
            }

            var heroCounts = dbHeroes.GroupBy(x => x.Name).ToDictionary(g => g.Key, g => g.Count());

            foreach (MushroomHeroes hero in dbHeroes)
            {

                if (!EvolveCharactersSkills.ContainsKey(hero.Name))
                {
                    EvolvedMushroom mushroomMaster = mushroomMasters.Find(x => x.Name == hero.Name) ?? throw new Exception("Character does not exist in the preset list!");

                    if (heroCounts[hero.Name] >= mushroomMaster.NoToTransform)
                    {
                        if (uniqueNames.Add(hero.Name)) // Only print the message if the name is not already in the HashSet
                        {
                            Console.WriteLine($"{hero.Name} --> {mushroomMaster.TransformTo}!");
                        }
                    }
                    else
                    {
                        if (uniqueNames.Add(hero.Name)) // Only print the message if the name is not already in the HashSet
                        {
                            Console.WriteLine($"{hero.Name} cannot transform to {mushroomMaster.TransformTo}!");
                        }
                    }
                }

            }
        }

    }

    // Transform Character function

    public static void TransformCharacter(List<MushroomHeroes> heroes, List<EvolvedMushroom> mushroomMasters, Dictionary<string, Dictionary<string, string>> EvolveCharactersSkills, Users user)
    {
        PrintColor("Transform Character", ConsoleColor.Green);

        const int TRANSFORMED_HP = 100;
        const int TRANSFORMED_EXP = 0;

        // Dictionary<string, string> EvolveCharactersSkills = new Dictionary<string, string> { { "Luigi", "Precision and Accuracy" }, { "Peach", "Magic Abilities" }, { "Mario", "Combat Skills" } };

        HashSet<string> uniqueNames = new HashSet<string>();


        using (var context = new MushroomDbContext())
        {
            var currentUser = context.Users.FirstOrDefault(x => x.Username == user.Username);
            heroes = context.MushroomHeroes.Where(x => x.UserId == currentUser.Id).ToList();

            if (!heroes.Any())
            {
                Console.WriteLine("No character in your pocket!");
                return;
            }

            int countBeforeContinue = 0;

            Console.WriteLine(string.Concat(Enumerable.Repeat("-", 30)));
            foreach (MushroomHeroes hero in heroes)
            {
                if (!EvolveCharactersSkills.ContainsKey(hero.Name))
                {
                    EvolvedMushroom mushroomMaster = mushroomMasters.Find(x => x.Name == hero.Name) ?? throw new Exception("Character does not exist in the preset list!");
                    // Checks specific character's count in order to transform
                    if (heroes.Count(x => x.Name == hero.Name) >= mushroomMaster.NoToTransform)
                    {
                        if (uniqueNames.Add(hero.Name))
                        {
                            Console.WriteLine($"ID: ({hero.Id}), Name: {hero.Name}, HP: {hero.Hp} (x{heroes.Count(x => x.Name == hero.Name)}). Ready to transform!");
                        }
                    }
                    else
                    {
                        if (uniqueNames.Add(hero.Name))
                        {
                            Console.WriteLine($"ID: ({hero.Id}), Name: {hero.Name} (x{heroes.Count(x => x.Name == hero.Name)}). Unable to transform, does not meet transformation amount.");
                        }
                    }
                }
                else
                {
                    countBeforeContinue += 1;

                    // if (uniqueNames.Add(hero.Name))
                    // {
                    //     Console.WriteLine($"Name: {hero.Name}, HP: {hero.Hp} (x{heroes.Count(x => x.Name == hero.Name)}) already transformed!");
                    // }
                }

            }
            Console.WriteLine(string.Concat(Enumerable.Repeat("-", 30)));

            if (countBeforeContinue == heroes.Count)
            {
                throw new Exception("No character(s) can be transformed!");
            }

            Console.Write("Enter Character's ID (Enter 'B' to back): ");

            var retrieveInput = Console.ReadLine();
            if (retrieveInput == "b" || retrieveInput == "B")
            {
                throw new Exception("Exiting...");
            }

            if (!int.TryParse(retrieveInput, out int selectedID))
            {
                throw new Exception("Invalid ID!");
            }

            try
            {
                if (heroes.Any(x => x.Id == selectedID))
                {
                    MushroomHeroes hero = heroes.Find(x => x.Id == selectedID);
                    EvolvedMushroom mushroomMaster = mushroomMasters.Find(x => x.Name == hero.Name) ?? throw new Exception("Character does not exist in the preset list!");
                    EvolveCharactersSkills.TryGetValue(mushroomMaster.TransformTo, out Dictionary<string, string> characterDetails);


                    if (hero.Name == mushroomMaster.TransformTo)
                    {
                        throw new Exception($"{hero.Name} is already transformed to {mushroomMaster.TransformTo}!");
                    }

                    if (heroes.Count(x => x.Name == hero.Name) >= mushroomMaster.NoToTransform)
                    {

                        // Remove the characters that are going to be transformed
                        var heroesToRemove = heroes.Where(x => x.Name == hero.Name).Take(mushroomMaster.NoToTransform).ToList();
                        context.MushroomHeroes.RemoveRange(heroesToRemove);


                        // Add the transformed hero
                        var transformedHero = new MushroomHeroes
                        {
                            Name = mushroomMaster.TransformTo,
                            Hp = TRANSFORMED_HP,
                            Exp = TRANSFORMED_EXP,
                            Skill = characterDetails.TryGetValue("Skill", out string skill) ? skill : "No Skill",
                            damage = Convert.ToDouble(characterDetails.TryGetValue("Damage", out string damage) ? damage : "0"),
                            UserId = currentUser.Id
                        };

                        context.MushroomHeroes.Add(transformedHero);

                        Console.WriteLine($"{transformedHero.Name} has been transformed to {mushroomMaster.TransformTo}!");

                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception($"{hero.Name} cannot transform to {mushroomMaster.TransformTo}!");
                    }
                }
                else
                {
                    throw new Exception("Character does not exist in your pocket!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            ContinueWhenPressed();
        }
    }

    // Remove Character function

    public static void removeCharacter(List<MushroomHeroes> heroes, Users user)
    {
        using (var context = new MushroomDbContext())
        {
            while (true)
            {
                PrintColor("Remove Character", ConsoleColor.Red);

                var currentUser = context.Users.FirstOrDefault(x => x.Username == user.Username);
                heroes = context.MushroomHeroes.Where(x => x.UserId == currentUser.Id).ToList();

                if (heroes.Count() <= 0)
                {
                    throw new Exception("No character in your pocket!");
                }

                Console.WriteLine(string.Concat(Enumerable.Repeat("-", 30)));
                foreach (MushroomHeroes h in heroes)
                {
                    PrintColor($"ID: ({h.Id}), Name: {h.Name}, HP: {h.Hp}, EXP: {h.Exp}, SKill: {h.Skill}");
                }
                PrintColor(string.Concat(Enumerable.Repeat("-", 30)));

                PrintColor("Enter Character's ID (Enter 'B' to back): ", newLine: false);
                var CharaID = Console.ReadLine();

                if (CharaID == "B" || CharaID == "b")
                {
                    PrintColor("Exiting...");
                    return;
                }

                if (!int.TryParse(CharaID, out int id))
                {
                    PrintColor("Invalid ID!");
                    continue;

                }

                if (!heroes.Exists(x => x.Id == id))
                {
                    PrintColor("Character does not exist in your pocket!");
                    continue;
                }

                var hero = heroes.Find(x => x.Id == id);
                context.Remove(hero);
                context.SaveChanges();

                PrintColor($"{hero.Name} has been removed!", ConsoleColor.Green);
            }

        }
    }

    // Attack function for PvE

    public static void attack(Users user)
    {
        using (var context = new MushroomDbContext())
        {
            PrintColor("Offline (PvE Mode)", ConsoleColor.Green);
            while (true)
            {

                var currentUser = context.Users.FirstOrDefault(x => x.Username == user.Username);
                var heroes = context.MushroomHeroes.Where(x => x.UserId == currentUser.Id).ToList();
                var enemies = context.EnemyCharacters.ToList();

                if (heroes.Count() <= 0)
                {
                    throw new Exception("No character in your pocket!");
                }

                // Print my list of characters
                PrintColor(string.Concat(Enumerable.Repeat("-", 30)));
                foreach (MushroomHeroes h in heroes)
                {
                    if (!h.isDead)
                    {
                        PrintColor($"ID: ({h.Id}), Name: {h.Name}, HP: {h.Hp}");
                    }
                }
                PrintColor(string.Concat(Enumerable.Repeat("-", 30)));


                PrintColor("Choose your character ID to attack (Enter 'B' to back): ", newLine: false);
                var retrieveInput = Console.ReadLine();

                if (retrieveInput == "B" || retrieveInput == "b")
                {
                    PrintColor("Exiting...");
                    return;
                }

                if (!int.TryParse(retrieveInput, out int selectedID))
                {
                    PrintColor("Invalid ID!");
                    continue;
                }

                var hero = heroes.Find(x => x.Id == selectedID);

                if (!heroes.Exists(x => x.Id == selectedID))
                {
                    PrintColor("Character does not exist in your pocket!");
                    continue;
                }


                // Print the list of enemies
                PrintColor(string.Concat(Enumerable.Repeat("-", 30)));
                foreach (EnemyCharacters h in enemies)
                {
                    if (!h.isDead)
                    {
                        PrintColor($"ID: ({h.Id}), Name: {h.Name}, HP: {h.Hp}");
                    }
                }
                PrintColor(string.Concat(Enumerable.Repeat("-", 30)));


                PrintColor("Choose an Enemy ID to attack (Enter 'b' to back): ", newLine: false);
                var targetName = Console.ReadLine();

                if (targetName == "B" || targetName == "b")
                {
                    PrintColor("Exiting...");
                    return;
                }

                if (!int.TryParse(targetName, out int targetID))
                {
                    PrintColor("Invalid ID!");
                    continue;
                }

                var target = context.EnemyCharacters.ToList().Find(x => x.Id == targetID);

                if (!enemies.Exists(x => x.Id == targetID))
                {
                    PrintColor("Enemy does not exist!");
                    continue;
                }

                Random random = new Random();

                PrintColor("Battle Begins!", ConsoleColor.Yellow);
                Thread.Sleep(1000);

                if (hero.Hp > 0 && target.Hp > 0)
                {
                    var heroDamage = random.Next(1, 10);
                    //Hero Attack Enemy
                    target.Hp -= heroDamage;

                    PrintColor($"{hero.Name} attacked {target.Name}! with {heroDamage} damage.", ConsoleColor.Green);
                    PrintColor($"{target.Name} HP: {target.Hp}");

                    Thread.Sleep(1000);

                    var enemyDamage = random.Next(1, 10);

                    //Enemy Attack Hero
                    hero.Hp -= enemyDamage;

                    PrintColor($"{target.Name} attacked {hero.Name}! with {enemyDamage} damage.", ConsoleColor.Red);
                    PrintColor($"{hero.Name} HP: {hero.Hp}");

                    // Update the database
                    context.EnemyCharacters.Update(target);
                    context.MushroomHeroes.Update(hero);
                    context.SaveChanges();


                    if (hero.Hp <= 0)
                    {
                        if (hero.isDead)
                        {
                            throw new Exception($"{hero.Name} has died!");
                        }

                        var e = hero;
                        e.isDead = true;
                        context.MushroomHeroes.Update(e);
                        context.SaveChanges();
                    }

                    if (target.Hp <= 0)
                    {
                        if (target.isDead)
                        {
                            throw new Exception($"{target.Name} is Dead!");
                        }

                        int randomEXP = random.Next(1, 3);
                        hero.Exp += randomEXP;

                        PrintColor($"{hero.Name} gained {randomEXP} EXP!", ConsoleColor.Green);
                        PrintColor($"You have slayn {target.Name}!", ConsoleColor.Green);

                        var e = target;
                        e.isDead = true;
                        context.EnemyCharacters.Update(e);
                        context.SaveChanges();
                    }

                    Thread.Sleep(1000);
                }
                else
                {
                    throw new Exception($"Character is dead!");
                }
            }
        }
    }
}
