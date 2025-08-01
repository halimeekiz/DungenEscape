using Konsolspil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class Player                                                    // Static = hører til klassen, ikke til et objekt
                                                                                  // - Hvis det kun en player static, hvis flere player så kan det ikke være static
    {
        public static int Health = 100;                                           // Player starter med 100% energi liv
        public static int life = 3;                                               // Player har 3 liv
        public static Room CurrentRoom;
        public static List<Item> Inventory = new List<Item>();                    // vi laver en liste da opgaven kræver vi samler flere ting og ikke en enkelt ting op. 


        public static void Move()                                                 // Void returnerer ikke nogen værdi!
        {

            while (true)
            {
                Console.Clear();
                CurrentRoom.ShowInfo(); // Viser information om det nuværende rum

                Console.WriteLine(" ");
                Console.WriteLine(" ");
                // Viser kun de gyldige retninger baseret på hvilket rum spilleren er i
                // Visning af gyldige retninger dynamisk
                Console.WriteLine("╔══════════════════════════════╗");
                Console.WriteLine("║ Du hører fodtrin bag dig!    ║");
                Console.WriteLine("╠══════════════════════════════╣");

                // Check hvilke retninger der er gyldige
                if (!string.IsNullOrEmpty(CurrentRoom.North))
                    Console.WriteLine("║  go north                    ║");

                if (!string.IsNullOrEmpty(CurrentRoom.South))
                    Console.WriteLine("║  go south                    ║");

                if (!string.IsNullOrEmpty(CurrentRoom.East))
                    Console.WriteLine("║  go east                     ║");

                if (!string.IsNullOrEmpty(CurrentRoom.West))
                    Console.WriteLine("║  go west                     ║");

                Console.WriteLine("║                              ║");
                Console.WriteLine("║ Brug nedenstående kommando:  ║");
                Console.WriteLine("║ Take                         ║");
                Console.WriteLine("║ Inventory (Viser inventory   ║");
                Console.WriteLine("║ Use Item                     ║");
                Console.WriteLine("║ Attack enemy                 ║");
                Console.WriteLine("║                              ║");
                Console.WriteLine("║ Skriv en retning ovenfor     ║");
                Console.WriteLine("║ eller en kommando.           ║");
                Console.WriteLine("║     Tryk Enter.              ║");
                Console.WriteLine("╚══════════════════════════════╝");
                Console.Write(">");

                // Read user input for direction eller kommando
                string input = Console.ReadLine().ToLower().Trim();

                // Inventory
                if (input == "inventory")
                {
                    Actions.ShowInventory();
                    Console.WriteLine("\nTryk på en tast for at fortsætte...");
                    Console.ReadKey();
                    continue;
                }

                // Use [item]
                if (input.StartsWith("use "))
                {
                    string itemName = input.Substring(4).Trim();
                    Actions.UseItem(itemName);
                    Console.WriteLine("\nTryk på en tast for at fortsætte...");
                    Console.ReadKey();
                    continue;
                }

                // Attack [våben]
                if (input.StartsWith("attack "))
                {
                    string weaponName = input.Substring(7).Trim();
                    Actions.AttackWithWeapon(weaponName);
                    Console.WriteLine("\nTryk på en tast for at fortsætte...");
                    Console.ReadKey();
                    continue;
                }

                // Take
                if (input.StartsWith("take "))
                {
                    string itemName = input.Substring(5).Trim();
                    Actions.TakeItem(itemName);
                    Console.WriteLine("\nTryk på en tast for at fortsætte...");
                    Console.ReadKey();
                    continue;
                }


                // Hvis ikke en af ovenstående – fortsæt med at tjekke retning
                string nextRoomId = null;

                switch (input)
                {
                    case "go north":
                        nextRoomId = CurrentRoom.North;
                        break;
                    case "go south":
                        nextRoomId = CurrentRoom.South;
                        break;
                    case "go east":
                        nextRoomId = CurrentRoom.East;
                        break;
                    case "go west":
                        nextRoomId = CurrentRoom.West;
                        break;
                    default:
                        Console.WriteLine("Ugyldigt. Prøv igen.");
                        Console.ReadKey();
                        continue;
                }


                if (nextRoomId == null)
                {
                    Console.WriteLine("Du kan ikke gå den vej. Tryk en tast for at prøve igen...");
                    Console.ReadKey();
                    continue;
                }

                Room nextRoom = Game.rooms[nextRoomId];

                if (nextRoom.IsLocked)
                {
                    // Tjek om spilleren har den nødvendige nøgle
                    bool hasKey = Inventory.Any(item => item.Name.ToLower() == nextRoom.RequiredKey?.ToLower());

                    if (hasKey)
                    {
                        Console.WriteLine($"Du bruger {nextRoom.RequiredKey} og låser døren op.");
                        nextRoom.IsLocked = false; // Lås op
                        CurrentRoom = nextRoom;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Døren er låst! Du mangler den rigtige nøgle.");
                        Console.WriteLine("Tryk en tast for at prøve en anden retning...");
                        Console.ReadKey();
                        continue;
                    }
                }
                else
                {
                    CurrentRoom = nextRoom;
                }

                // Her vises info om det nye rum, efter spilleren har bevæget sig.
                Console.Clear();
                CurrentRoom.ShowInfo();
                Console.WriteLine("\nTryk på en tast for at fortsætte...");
                Console.ReadKey();
            }
        }
    }
}