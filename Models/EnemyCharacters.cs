// Name: Kai Jie
// Admin No: 234412H

namespace MushroomPocket
{
    public class EnemyCharacters
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Hp { get; set; }
        public string Skill { get; set; }
        public bool isDead { get; set; }
    }

    // Enemy Characters
    public class Goomba : EnemyCharacters
    {
        public Goomba()
        {
            Name = "Goomba";
            Hp = 50;
            Skill = "Stomp";
            isDead = false;
        }
    }

    public class Koopa : EnemyCharacters
    {
        public Koopa()
        {
            Name = "Koopa";
            Hp = 75;
            Skill = "Spike Ball";
            isDead = false;
        }
    }

    public class Bowser : EnemyCharacters
    {
        public Bowser()
        {
            Name = "Bowser";
            Hp = 100;
            Skill = "Fire Breath";
            isDead = false;
        }
    }
}