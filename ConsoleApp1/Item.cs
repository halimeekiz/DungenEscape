using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


        /*
           - 'void' betyder, at metoden ikke returnerer noget.
              Den gør noget(lægger ting ind i rummene), men giver ikke et resultat tilbage.
              Vi skal ikke bruge noget svar – vi skal bare have tingene lagt ud i spillets verden.

           -  Derfor bruger vi 'static void' her: 
              Metoden skal kun kaldes én gang i starten af spillet og bruges til at fylde rummene med ting.*/
        public static void PopulateItems(Dictionary<string, Room> rooms)
        {
            // Opretelse af items
            Item loftNoegle = new Item("Loft-nøgle", 0, 0, 0, ItemType.Key);
            Item husNoegle = new Item("Hus-nøgle", 0, 0, 0, ItemType.Key);
            Item kniv = new Item("Kniv", damage: 10, heal: 0, attackPower: 5, ItemType.Weapon);
            Item potion = new Item("Potion", 0, 20, 0, ItemType.Potion);
            Item lommelygte = new Item("Lommelygte", damage: 0, heal: 0, attackPower: 0, ItemType.Generic);
            Item hjerte = new Item("Hjerte", damage: 0, heal: 50, attackPower: 0, ItemType.Potion);
            Item giftflaske = new Item("Giftflaske", damage: 0, heal: 0, attackPower: 35, ItemType.Weapon);
            Item bombe = new Item("Bombe", damage: 0, heal: 0, attackPower: 50, ItemType.Weapon);

            // Opretelse af våben
            Weapon sværd = new Weapon("Sværd", damage: 45, heal: 0, attackPower: 0, isRanged: false, usesLeft: 5);
            Weapon bue = new Weapon("Bue", damage: 20, heal: 0, attackPower: 0, isRanged: true, usesLeft: 15);

            // Lægger items i diverse rum med de korrekte navne
            rooms["Soveværelset"].Items.AddRange(new[] { loftNoegle });                           // Nøgle i soveværelset
            rooms["Køkken"].Items.AddRange(new[] { sværd, kniv });                                // Sværd og kniv i køkkenet
            rooms["Børneværelset"].Items.AddRange(new[] { bue });                                 // Bue i børneværelset
            rooms["Kælder"].Items.AddRange(new[] { potion, lommelygte });                         // Potion og lommelygte i kælderen  
            rooms["Baghaven"].Items.AddRange(new[] { hjerte, bombe, husNoegle });                 // Hjerte, nøgle og bombe i baghaven
            rooms["Værelse"].Items.AddRange(new[] { giftflaske });                                // Giftflaske i det mørke rum
        }
    }

    public enum ItemType // Enum til at kategorisere forskellige typer af items (f.eks. våben, potioner og nøgler).
    {
        Generic,
        Weapon,
        Potion,
        Key
    }
}

