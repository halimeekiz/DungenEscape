using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Weapon : Item //Weapon arver fra Item. Det betyder, at Weapon automatisk får alle egenskaber og metoder fra Item-klassen.
    {
        // Angiver om våbnet er et langdistancevåben (true = f.eks. bue, false = f.eks. sværd)
        public bool IsRanged { get; set; }

        // Fortæller hvor mange gange våbnet kan bruges, før det er "slidt op"
        public int UsesLeft { get; set; }

        // Random bruges til at bestemme, om angrebet rammer eller ej (tilfældigt)
        private Random rng = new Random();


        /* Konstruktøren modtager alle nødvendige oplysninger for at oprette et våben.
           Den sender navn, skade, heal og attackPower videre til Item-konstruktøren via 'base(...)',
           og sætter samtidig IsRanged og UsesLeft, som hører til Weapon.*/
        public Weapon(string name, int damage, int heal, int attackPower, bool isRanged, int usesLeft)
            : base(name, damage, heal, attackPower)
        {
            IsRanged = isRanged;
            UsesLeft = usesLeft;
        }


        /* Hit() kaldes når spilleren forsøger at angribe med våbnet.
           Den tjekker først om der er UsesLeft tilbage. Hvis ikke, vises en besked og angrebet afbrydes.
           Hvis der stadig er uses tilbage, trækkes én fra.
           Derefter beregnes der en 75% chance for at ramme (true hvis rammer, false hvis miss).
           Spilleren får besked om resultatet via konsollen.*/
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
