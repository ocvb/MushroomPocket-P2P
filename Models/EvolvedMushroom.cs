using System;

// Name: Kai Jie
// Admin No: 234412H

namespace MushroomPocket
{
    public class EvolvedMushroom
    {
        public string Name { get; set; }
        public int NoToTransform { get; set; }
        public string TransformTo { get; set; }
        public virtual string Skill { get; set; }
        public virtual double Damage { get; set; }

        public EvolvedMushroom()
        {
        }

        public EvolvedMushroom(string name, int noToTransform, string transformTo)
        {
            Name = name;
            NoToTransform = noToTransform;
            TransformTo = transformTo;
        }
    }

    public class Peach : EvolvedMushroom
    {
        public Peach() : base("Daisy", 2, "Peach")
        {
            Skill = "Magic Abilities";
            Damage = 12.5;
        }

        public double Hp { get; set; }

        // Ability
        public void UseSkill()
        {
            Console.WriteLine("Healing...");

            if (Hp > 0)
            {
                double heal = 10;
                Hp = Hp + heal;
                Console.WriteLine($"{Name} Healed {Hp} HP");
                return;
            }
            else
            {
                Console.WriteLine("Cannot heal");
            }
        }
    }

    public class Mario : EvolvedMushroom
    {
        public Mario() : base("Wario", 3, "Mario")
        {
            Skill = "Combat Skills";
            Damage = 11.5;
        }

        // ability
        public double UseSkill(double hp)
        {
            Console.WriteLine("Fireball attack!");

            double fireDamage = Damage * 1.5 / 2;
            Console.WriteLine($"Dealt: {fireDamage} damage");
            return fireDamage;
        }
    }

    public class Luigi : EvolvedMushroom
    {
        public Luigi() : base("Waluigi", 1, "Luigi")
        {
            Skill = "Precision and Accuracy";
            Damage = 11;
        }

        // ability
        public double UseSkill()
        {
            Console.WriteLine("Speedy attack!");
            double speedyDamage = Damage * 1.5;
            Console.WriteLine($"Dealt: {speedyDamage} damage");
            return speedyDamage;
        }
    }
}