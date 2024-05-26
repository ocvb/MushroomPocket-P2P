using System;

// Name: Kai Jie
// Admin No: 234412H

namespace MushroomPocket
{
    /// <summary>
    /// Represents a hero in the Mushroom Heroes game.
    /// </summary>
    public class MushroomHeroes
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private double hp;
        public double Hp
        {
            get { return hp; }
            set { hp = Math.Round(value, 2); }
        }
        public int Exp { get; set; }
        public string Skill { get; set; }
        public double damage { get; set; }
        public bool isDead { get; set; }
        public int UserId { get; set; }
        public Users User { get; set; }

        public MushroomHeroes()
        {
        }

        public MushroomHeroes(string name, int hp, int exp, string skill, bool isDead)
        {
            Name = name;
            Hp = hp;
            Exp = exp;
            Skill = skill;
            isDead = false;
        }

        internal void UseSkill(int v)
        {


        }
    }

    public class Daisy : MushroomHeroes
    {
        public Daisy()
        {
            Name = "Daisy";
            Hp = 100;
            Exp = 0;
            Skill = "Leadership";
            damage = 11;
            isDead = false;
        }

        // Ability
        public void UseSkill(double damaged)
        {
            Console.WriteLine("Defending...");

            if (Hp > 0)
            {
                double defend = 1.3;
                Hp -= damaged / defend;
                Console.WriteLine($"{Name} Defended {Hp} HP");
                return;
            }
            else
            {
                Console.WriteLine("Cannot defend");
            }
        }
    }

    public class Wario : MushroomHeroes
    {
        public Wario()
        {
            Name = "Wario";
            Hp = 100;
            Exp = 0;
            Skill = "Strength";
            damage = 10.5;
            isDead = false;
        }

        // Ability
        public double UseSkill()
        {
            Console.WriteLine("Increasing Strength...");

            double increase = 1.5;
            damage = damage * increase;
            Console.WriteLine($"{Name} Increased Strength to {damage}");
            return damage;
        }
    }

    public class Waluigi : MushroomHeroes
    {
        public Waluigi()
        {
            Name = "Waluigi";
            Hp = 100;
            Exp = 0;
            Skill = "Agility";
            damage = 9.5;
            isDead = false;
        }

        // Ability
        public void UseSkill()
        {
            // Console.WriteLine("Dodging...");

            // Random random = new Random();
            // double dodgeChance = random.Next(0, 1) ;




            // Console.WriteLine($"{Name} Dodged to {damage}");
            return;
        }
    }

}