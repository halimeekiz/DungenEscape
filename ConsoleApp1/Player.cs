using Konsolspil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    // Player-klassen indeholder spillerens tilstand (hvilket rum, inventar)
    // samt hovedloopen der håndterer input, bevægelse og simple kommandoer.
    public static class Player
    {
        // Peger på det rum spilleren står i lige nu.
        public static Room CurrentRoom = null!;

        // Simpelt inventar. Ting (Item) tilføjes/fjernes via Actions.TakeItem/UseItem.
        public static List<Item> Inventory = new();

        // Hovedloop for spillet: vis rum, læs input, håndter kommandoer/bevægelse.
        public static void Move()
        {
            while (true)
            {
                // 1) Ryd skærm og vis info for det rum jeg står i.
                Console.Clear();
                CurrentRoom.ShowInfo();

                // 2) Tegn kommandomenu og minikort side om side (menu venstre, kort højre).
                int menuTop = Console.CursorTop;
                int usedLines = Actions.ShowCommandMenu(CurrentRoom); // venstre kolonne

                Console.SetCursorPosition(0, menuTop);
                Maps.DrawMiniMap(Game.rooms, CurrentRoom, leftPadding: 45); // højre kolonne

                // 3) Placér cursor under menuen og bed om input.
                Console.SetCursorPosition(0, menuTop + usedLines);
                Console.Write(">");

                // 4) Læs brugerens input (kommando eller retning).
                string input = (Console.ReadLine() ?? "").ToLower().Trim();

                // --- Kommandoer -----------------------------------------------------

                // inventory -> vis hvad jeg har i tasken
                if (input == "inventory")
                {
                    Actions.ShowInventory();
                    Actions.Pause();
                    continue;
                }

                // use [navn] -> brug en ting fra inventaret (f.eks. nøgle)
                if (input.StartsWith("use "))
                {
                    string itemName = input.Substring(4).Trim();
                    Actions.UseItem(itemName);
                    Actions.Pause();
                    continue;
                }

                // take [navn] -> saml en ting op fra rummet
                if (input.StartsWith("take "))
                {
                    string itemName = input.Substring(5).Trim();
                    Actions.TakeItem(itemName);
                    Actions.Pause();
                    continue;
                }

                // restart -> nulstil spillet (rum, items, startposition)
                if (input == "restart")
                {
                    Inventory.Clear();
                    Game.rooms = Room.CreateAllRooms();
                    Item.PopulateItems(Game.rooms);
                    CurrentRoom = Game.rooms["Baghaven"];

                    Console.WriteLine("Spillet er genstartet!");
                    Actions.Pause();
                    continue;
                }

                // quit/exit -> afslut programmet
                if (input == "quit" || input == "exit")
                {
                    Console.WriteLine("Farvel! 👋");
                    Environment.Exit(0); // Afslutter hele programmet
                }

                // --- Retningsinput (bevægelser) -------------------------------------

                // Oversæt tekstinput til id’et på det næste rum (hvis der er en udgang).
                string? nextRoomId = null;

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
                        Actions.Pause();
                        continue;
                }

                // Ingen udgang i den retning
                if (nextRoomId == null)
                {
                    Console.WriteLine("Du kan ikke gå den vej. Tryk en tast for at prøve igen...");
                    Actions.Pause();
                    continue;
                }

                // Slå det næste rum op i spillets room-liste.
                Room nextRoom = Game.rooms[nextRoomId];

                // Hvis døren er låst, tjek om jeg har den rigtige nøgle i inventaret.
                if (nextRoom.IsLocked)
                {
                    // Robust sammenligning af nøgle-navne (Normalize fjerner mellemrum/case/æøå-varianter).
                    bool hasKey = Inventory.Any(item => Actions.Normalize(item.Name) == Actions.Normalize(nextRoom.RequiredKey ?? ""));

                    if (hasKey)
                    {
                        Console.WriteLine($"Du bruger {nextRoom.RequiredKey} og låser døren op.");
                        nextRoom.IsLocked = false; // lås op
                        CurrentRoom = nextRoom;    // gå ind
                        // Intet break; vi lader flowet fortsætte så rummet vises nedenfor.
                    }
                    else
                    {
                        Console.WriteLine("Døren er låst! Du mangler den rigtige nøgle.");
                        Console.WriteLine("Tryk en tast for at prøve en anden retning...");
                        Actions.Pause();
                        continue;
                    }
                }
                else
                {
                    // Døren er ikke låst — gå ind i rummet.
                    CurrentRoom = nextRoom;
                }

                // 5) Når jeg er kommet ind, viser jeg rummet igen
                //    og håndterer 2D-array-hændelser (fælder, nøgle, udgang) via Labyrinth.
                Console.Clear();
                CurrentRoom.ShowInfo();

                // (Her kaldes hændelser fra mit char[,] Grid – fx fælder/udgang – for det nye rum)
                // ntrig 2D-array-hændelser (fælder) først når vi ER i rummet
                Labyrinth.OnEnterRoom(Player.CurrentRoom.Id);

                // 6) Lille pause så teksten kan nå at blive læst.
                Actions.Pause();
            }
        }
    }
}
