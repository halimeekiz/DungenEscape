using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Weapon : Item
    {
        public bool IsRanged { get; set; }
        public int UsesLeft { get; set; }
        private Random rng = new Random();

        public Weapon(string name, int damage, int heal, int attackPower, bool isRanged, int usesLeft)
            : base(name, damage, heal, attackPower)
        {
            IsRanged = isRanged;
            UsesLeft = usesLeft;
        }

        public bool Hit()
        {
            if (UsesLeft <= 0)
            {
                Console.WriteLine($"{Name} kan ikke bruges mere.");
                return false;
            }

            UsesLeft--;

            bool hitSuccess = rng.Next(100) < 75; // 75% chance for hit

            if (hitSuccess)
                Console.WriteLine($"{Name} ramte og gjorde {Damage} skade!");
            else
                Console.WriteLine($"{Name} missede!");

            return hitSuccess;
        }
    }
}
