using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Name: Kai Jie
// Admin No: 234412H

namespace MushroomPocket
{
    public class Users
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<MushroomHeroes> MushroomHeroes { get; set; }

        public Users()
        {
        }

        public Users(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}