using ConsoleApp1;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

namespace Konsolspil
{
    public static class Actions
    {
        // Normaliser navne/strenge så sammenligninger bliver robuste:
        // - trim + lowercase
        // - æ/ø/å -> ae/oe/aa
        // - fjern mellemrum, '-' og '_'
        // Det gør, at "Loft nøgle", "loft-nøgle" og "LOFTNØGLE" matches ens.
        public static string Normalize(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            s = s.Trim().ToLowerInvariant();
            s = s.Replace("æ", "ae").Replace("ø", "oe").Replace("å", "aa");
            var sb = new StringBuilder(s.Length);
            foreach (var ch in s)
                if (!char.IsWhiteSpace(ch) && ch != '-' && ch != '_')
                    sb.Append(ch);
            return sb.ToString();
        }

        // Lille helper til at sammenligne to strenge med min normalisering
        private static bool Matches(string a, string b) => Normalize(a) == Normalize(b);

        // Vis alt, hvad spilleren har samlet op indtil videre
        public static void ShowInventory()
        {
            Console.WriteLine("Dit inventar:");
            if (Player.Inventory.Count == 0)
            {
                Console.WriteLine("Din taske er tom.");
                return;
            }

            foreach (var item in Player.Inventory)
                Console.WriteLine($"- {item.Name} ({item.Type})");
        }

        // Saml en ting op fra det aktuelle rum, hvis navnet matcher
        public static void TakeItem(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
            {
                Console.WriteLine("Tag hvad?");
                return;
            }

            // Find første item i rummet der matcher det, spilleren skrev
            var item = Player.CurrentRoom.Items.FirstOrDefault(i => Matches(i.Name, itemName));

            if (item == null)
            {
                Console.WriteLine($"Jeg kan ikke se '{itemName}' her.");
                return;
            }

            // Flyt tingen fra rummet til spillerens inventar
            Player.Inventory.Add(item);
            Player.CurrentRoom.Items.Remove(item);
            Console.WriteLine($"{item.Name} er blevet tilføjet til dit inventar.");
        }

        // Brug en ting fra inventaret (jeg bruger kun nøgler aktivt her)
        public static void UseItem(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
            {
                Console.WriteLine("Brug hvad?");
                return;
            }

            // Tjek at spilleren faktisk har tingen
            var item = Player.Inventory.FirstOrDefault(i => Matches(i.Name, itemName));

            if (item == null)
            {
                Console.WriteLine($"Du har ikke '{itemName}' i dit inventar.");
                return;
            }

            // Hvis det er en nøgle: prøv at låse låste nabo-rum op, der kræver samme nøgle
            if (item.Type == ItemType.Key)
            {
                var room = Player.CurrentRoom;
                var unlockedAny = false;

                // Lokal funktion der prøver at låse et naboom op, hvis id'et gives
                void TryUnlock(string? nextId)
                {
                    if (string.IsNullOrWhiteSpace(nextId)) return;
                    if (!Game.rooms.TryGetValue(nextId, out var r)) return;
                    if (!r.IsLocked) return;

                    // Sammenlign RequiredKey med navnet på nøglen, robust
                    if (Matches(r.RequiredKey ?? "", item.Name))
                    {
                        r.IsLocked = false;
                        Console.WriteLine($"Du låser døren til {nextId} op med {item.Name}.");
                        unlockedAny = true;
                    }
                }

                // Forsøg på alle fire retninger fra det aktuelle rum
                TryUnlock(room.North);
                TryUnlock(room.South);
                TryUnlock(room.East);
                TryUnlock(room.West);

                if (!unlockedAny)
                    Console.WriteLine($"Der sker ikke noget, når du bruger {item.Name} her.");

                return;
            }

            // Hvis det ikke er en nøgle, sker der intet særligt (kan udvides senere)
            Console.WriteLine($"Du bruger {item.Name}, men intet særligt sker.");
        }

        // Tegn min lille “kommando-boks” i venstre side og returnér højden (antal linjer) den brugte
        public static int ShowCommandMenu(Room room)
        {
            var dirs = new List<string>();
            if (!string.IsNullOrWhiteSpace(room.North)) dirs.Add("go north");
            if (!string.IsNullOrWhiteSpace(room.South)) dirs.Add("go south");
            if (!string.IsNullOrWhiteSpace(room.East)) dirs.Add("go east");
            if (!string.IsNullOrWhiteSpace(room.West)) dirs.Add("go west");

            const int innerWidth = 40;
            string top = "╔" + new string('═', innerWidth) + "╗";
            string sep = "╠" + new string('═', innerWidth) + "╣";
            string bottom = "╚" + new string('═', innerWidth) + "╝";

            int startTop = Console.CursorTop;

            Console.WriteLine();
            Console.WriteLine(top);
            WriteCentered("Kommandoer", innerWidth);
            Console.WriteLine(sep);

            // Vis mulige retninger først
            if (dirs.Count == 0) WriteRow("Ingen udgange her.", innerWidth);
            else foreach (var d in dirs) WriteRow(d, innerWidth);

            // Så de generelle kommandoer
            WriteRow("", innerWidth);
            WriteRow("Take            - Tag et item", innerWidth);
            WriteRow("Inventory       - Vis dine ting", innerWidth);
            WriteRow("Use [item]      - Brug en ting", innerWidth);
            WriteRow("Restart         - Genstart spillet", innerWidth);
            WriteRow("Exit            - Afslut spillet", innerWidth);

            Console.WriteLine(bottom);
            Console.WriteLine("Skriv en af retningerne ovenfor og tryk Enter.");

            int endTop = Console.CursorTop;
            return endTop - startTop;

            // Udskriv en enkelt forekomst af tekst inden i en “ramme”-linje
            static void WriteRow(string text, int width)
            {
                if (text.Length > width) text = text[..width];
                Console.WriteLine("║ " + text.PadRight(width - 2) + " ║");
            }

            // Centreret overskrift i boksen
            static void WriteCentered(string text, int width)
            {
                if (text.Length > width) text = text[..width];
                int left = (width - text.Length) / 2;
                WriteRow(new string(' ', left) + text, width);
            }
        }

        // Lille pause-helper som jeg bruger flere steder efter en besked,
        // så spilleren kan nå at læse den inden skærmen ryddes
        public static void Pause()
        {
            Console.WriteLine("\nTryk på en tast for at fortsætte...");
            Console.ReadKey();
        }
    }
}
