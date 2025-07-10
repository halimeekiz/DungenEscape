using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Item
    {
        public int Damage { get; set; }
        public int Heal { get; set; }
        public int AttackPower { get; set; }
        public string Name { get; set; }
        public ItemType Type { get; set; }

        public Item(string name, int damage, int heal, int attackPower, ItemType type = ItemType.Generic)
        {
            Name = name;
            Damage = damage;
            AttackPower = attackPower;
            Heal = heal;
            Type = type;
        }

        public static void PopulateItems(Dictionary<string, Room> rooms)
        {
            // Opret items
            Item loftNoegle = new Item("Loft-nøgle", 0, 0, 0);
            Item kniv = new Item("Kniv", 10, 0, 5, ItemType.Weapon);
            Item potion = new Item("Potion", 0, 20, 0, ItemType.Potion);
            Item lommelygte = new Item("Lommelygte", damage: 0, heal: 0, attackPower: 0);
            Item hjerte = new Item("Hjerte", damage: 0, heal: 50, attackPower: 0);
            Item giftflaske = new Item("Giftflaske", damage: 30, heal: 50, attackPower: 0);
            Item bombe = new Item("Bombe", damage: 0, heal: 0, attackPower: 50);

            // Opret våben
            Weapon sværd = new Weapon("Sværd", damage: 35, heal: 0, attackPower: 0, isRanged: false, usesLeft: 5);
            Weapon bue = new Weapon("Bue", damage: 20, heal: 0, attackPower: 0, isRanged: true, usesLeft: 15);

            // Læg items i rum
            rooms["Rum7"].Items.AddRange(new[] { loftNoegle }); // Nøgle i soveværelset
            rooms["Rum4"].Items.AddRange(new[] { sværd });      // Sværd i køkkenet
            rooms["Rum9"].Items.AddRange(new[] { bue });        // Bue i børneværelset

            // Du kan senere tilføje flere items i rum her
        }
    }

    public enum ItemType
    {
        Generic,
        Weapon,
        Potion,
        Key
    }
}

