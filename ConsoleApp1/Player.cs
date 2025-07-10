using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class Player // Static = hører til klassen, ikke til et objekt - Hvis det kun en player static, hvis flere player så kan det ikke være static
    {
        public static int Health = 100; // Player starter med 100% energi liv
        public static int life = 3; // Player har 3 liv
        public static Room CurrentRoom;
        public static List<Item> Inventory = new List<Item>(); // vi laver en liste da opgaven kræver vi samler flere ting og ikke en enkelt ting op. 


        public static void Move() // Husk Void returnerer ikke nogen værdi!
        {

            while (true)
            {
                Console.Clear();
                CurrentRoom.ShowInfo(); // Viser information om det nuværende rum

                Console.WriteLine("╔════════════════════════════╗");
                Console.WriteLine("║ Du hører fodtrin bag dig!  ║");
                Console.WriteLine("╠════════════════════════════╣");
                Console.WriteLine("║  go north                  ║");
                Console.WriteLine("║  go south                  ║");
                Console.WriteLine("║  go west                   ║");
                Console.WriteLine("║  go east                   ║");
                Console.WriteLine("║                            ║");
                Console.WriteLine("║  Skriv en retning ovenfor  ║");
                Console.WriteLine("║  og tryk Enter.            ║");
                Console.WriteLine("╚════════════════════════════╝");
                Console.Write(">");

                string input = Console.ReadLine().ToLower().Trim();

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
                        Console.WriteLine("Ugyldig retning. Prøv igen.");
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