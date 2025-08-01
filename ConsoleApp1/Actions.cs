using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsolspil
{
    public static class Actions
    {
        // Brugen af et item (som potion)
        public static void UseItem(string itemName)
        {
            var item = Player.Inventory.FirstOrDefault(i => i.Name.ToLower() == itemName.ToLower());

            if (item == null)
            {
                Console.WriteLine("Du har ikke dette item i dit inventar.");
                return;
            }

            // ➤ Brug potion
            if (item.Type == ItemType.Potion)
            {
                Console.WriteLine($"Du bruger {item.Name} og helbreder {item.Heal} liv.");
                Player.Health += item.Heal;
                Player.Inventory.Remove(item);
            }
            // ➤ Brug lommelygte
            else if (item.Name.ToLower() == "lommelygte")
            {
                Console.WriteLine("Du tænder lommelygten. Rummet bliver oplyst.");
            }
            // ➤ Brug giftflaske
            else if (item.Name.ToLower() == "giftflaske")
            {
                Console.WriteLine("Du drikker gift... det var en dårlig idé!");
                Player.Health -= 30;
                Player.Inventory.Remove(item);
            }
            else
            {
                Console.WriteLine("Det item kan ikke bruges på denne måde.");
            }
        }


        // Angriber et væsen eller monster med et våben
        public static void AttackWithWeapon(string weaponName)
        {
            var weapon = Player.Inventory.OfType<Weapon>().FirstOrDefault(w => w.Name.ToLower() == weaponName.ToLower());

            if (weapon == null)
            {
                Console.WriteLine("Du har ikke dette våben i dit inventar.");
                return;
            }

            // Logik for at angribe med våben
            if (weapon.IsRanged)
            {
                Console.WriteLine($"Du bruger {weapon.Name} til at angribe på afstand!");
            }
            else
            {
                Console.WriteLine($"Du bruger {weapon.Name} til at angribe nært!");
            }

            // Tjeker om angrebet lykkes
            bool hit = weapon.Hit();

            if (hit)
            {
                Console.WriteLine($"Angrebet med {weapon.Name} lykkedes og gjorde {weapon.Damage} skade.");
            }
            else
            {
                Console.WriteLine("Angrebet mislykkedes.");
            }
        }

        // Viser spillerens inventar
        public static void ShowInventory()
        {
            Console.WriteLine("Dit inventar:");
            if (Player.Inventory.Count == 0)
            {
                Console.WriteLine("Dit inventar er tomt.");
                return;
            }

            foreach (var item in Player.Inventory)
            {
                Console.WriteLine($"- {item.Name} ({item.Type})");
            }
        }

        public static void TakeItem(string itemName)
        {
            var item = Player.CurrentRoom.Items.FirstOrDefault(i => i.Name.ToLower() == itemName.ToLower());

            if (item == null)
            {
                Console.WriteLine("Der findes ikke sådan et item i rummet.");
                return;
            }

            Player.Inventory.Add(item);
            Player.CurrentRoom.Items.Remove(item);
            Console.WriteLine($"{item.Name} er blevet tilføjet til dit inventar.");
        }

    }
}
