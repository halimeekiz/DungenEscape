using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class Room // Step 1 - Klassen beskriver et rum i spillet. Den indeholder information om rummet og hvad det kan (fx vise info og holde styr på retninger og ting).

    {
        // Step 2 - Inde i klassen definerer vi egenskaber (kaldet properties), som beskriver rummets data.

        // Unikt Id for rummet
        public string Id { get; set; }

        // Flag der fortæller om rummet (døren ind til rummet) er låst
        public bool IsLocked { get; set; } = false;

        // Hvilken nøgle kræves for at låse dette rum op (null = ingen nøgle nødvendig)
        public string? RequiredKey { get; set; } = null;

        // Beskrivelse af rummet
        public string Description { get; set; }

        // Retninger (peger på ID’er til andre rum)
        public string? North { get; set; }
        public string? South { get; set; }
        public string? East { get; set; }
        public string? West { get; set; }

        // En liste med items der ligger i rummet
        public List<Item> Items { get; set; } = new List<Item>();

        // Konstruktor: Denne metode kaldes, når vi opretter et nyt rum. Den kræver et ID og en beskrivelse og sætter dem på rummet.
        public Room(string id, string description)
        {
            Id = id; // Sætter rummets unikke ID
            Description = description; // Sætter beskrivelsen af rummet
        }

        // Step 3 - Laver metoder (funktioner) - Dvs. bestemmer hvad der skal ske. 
        // Metode til at udskrive information om rummet til konsollen
        public void ShowInfo()
        {
            // Viser rummets id (bruges også i navigationen)
            Console.WriteLine($"Rum-ID: {Id}");

            // Maks bredde til beskrivelse (venstre side – vi reserverer ca. 60 kolonner)
            int maxWidth;
            try { maxWidth = Math.Max(30, Math.Min(60, Console.WindowWidth - 60)); }
            catch { maxWidth = 60; }

            // Wrap beskrivelsen pænt over 1–2 linjer så teksten ikke flyder ud
            var (l1, l2) = Wrap2Lines(Description, maxWidth);

            string label = "Beskrivelse: ";
            Console.WriteLine(label + l1);
            if (!string.IsNullOrEmpty(l2))
                Console.WriteLine(new string(' ', label.Length) + l2);

            // Hvis rummet er låst, informer om det – og hvilken nøgle der kræves
            if (IsLocked)
            {
                Console.WriteLine("⚠️  Døren til dette rum er låst.");
                Console.WriteLine($"   Kræver nøgle: {RequiredKey}");
            }

            // Vis hvad der ligger i rummet lige nu
            Console.WriteLine("Ting i rummet:");
            if (Items.Count == 0) Console.WriteLine(" - Ingen ting i rummet");
            else foreach (var item in Items) Console.WriteLine($" - {item.Name}");
        }

        // Hjælper til at bryde en tekst i maks to linjer med ellipsis, så UI'et forbliver pænt
        private static (string line1, string? line2) Wrap2Lines(string text, int width)
        {
            if (string.IsNullOrWhiteSpace(text)) return ("", "");
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var lines = new List<string>();
            var sb = new StringBuilder();

            int i = 0;
            while (i < words.Length && lines.Count < 2)
            {
                string w = words[i];
                int nextLen = sb.Length == 0 ? w.Length : sb.Length + 1 + w.Length;

                // Læg ord på den aktuelle linje hvis de kan være der, ellers “luk” linjen
                if (nextLen <= width) { if (sb.Length > 0) sb.Append(' '); sb.Append(w); i++; }
                else
                {
                    // luk linjen
                    lines.Add(sb.ToString());
                    sb.Clear();
                }
            }

            // Eventuelle rester fra buffer
            if (sb.Length > 0 && lines.Count < 2) lines.Add(sb.ToString());

            // Hvis der stadig er ord tilbage – tilføj ellipsis på linje 2
            if (i < words.Length)
            {
                string l2 = lines.Count >= 2 ? lines[1] : "";
                string ellipsis = "...";
                if (l2.Length + ellipsis.Length > width)
                    l2 = l2[..Math.Max(0, width - ellipsis.Length)];
                if (lines.Count == 0) lines.Add(""); // safety
                if (lines.Count == 1) lines.Add(l2 + ellipsis);
                else lines[1] = l2 + ellipsis;
            }

            string first = lines.Count > 0 ? lines[0] : "";
            string second = lines.Count > 1 ? lines[1] : "";
            return (first, second);
        }

        // Fabriksmetode der opretter ALLE rum i spillet, kobler dem sammen,
        // og sætter låse/nøgler hvor det giver mening.
        public static Dictionary<string, Room> CreateAllRooms()
        {
            var rooms = new Dictionary<string, Room>();

            // Opretter rummene med beskrivende navne
            // Baghaven - Startpunktet
            Room bagHaven = new Room("Baghaven", "Du står i baghaven. Bagdøren mod øst er låst, og du har brug for en nøgle for at slippe ind. Du har en fornemmelse af, at noget venter på den anden side.");
            Room entre = new Room("Entré", "Du står i entreen. En dør fører længere ind i huset.");                        // Entré
            Room koekken = new Room("Køkken", "Et lille køkken med en lugt af brændt toast.");                             // Køkken
            Room entreMedTrappe = new Room("Entré udgangen", "Med trappe til første sal");                                 // Entré med trappe
            Room foersteSal = new Room("Førstesal", "Førstesal – en gang med adgang til flere værelser");                  // Førstesal
            Room sovevaerelse = new Room("Soveværelset", "Du er i soveværelset");                                          // Soveværelse    
            Room moerktRum = new Room("Værelse", "Et mørkt rum hvor noget bevæger sig i skyggerne.");                      // Mørkt rum
            Room boernevaerelse = new Room("Børneværelset", "Et rodet børneværelse med en åben dør mod øst.");             // Børneværelse
            Room loft = new Room("Loftet", "Et støvet loft med en enkelt glødepære i loftet.");                            // Loft

            // Tilføjer rummene til ordbogen (så de kan slås op på deres Id)
            rooms.Add(bagHaven.Id, bagHaven);
            rooms.Add(entre.Id, entre);
            rooms.Add(koekken.Id, koekken);
            rooms.Add(entreMedTrappe.Id, entreMedTrappe);
            rooms.Add(foersteSal.Id, foersteSal);
            rooms.Add(sovevaerelse.Id, sovevaerelse);
            rooms.Add(moerktRum.Id, moerktRum);
            rooms.Add(boernevaerelse.Id, boernevaerelse);
            rooms.Add(loft.Id, loft);

            // Forbinder retninger mellem rummene ved at bruge de beskrivende navne
            bagHaven.East = "Entré";
            entre.West = "Baghaven";

            entre.East = "Køkken";
            koekken.West = "Entré";

            entre.North = "Entré udgangen";
            entreMedTrappe.South = "Entré";

            entreMedTrappe.North = "Førstesal";
            foersteSal.South = "Entré udgangen";

            foersteSal.East = "Børneværelset";
            boernevaerelse.West = "Førstesal";

            foersteSal.West = "Soveværelset";
            sovevaerelse.East = "Førstesal";

            foersteSal.North = "Loftet";
            loft.South = "Førstesal";

            loft.North = "Værelse";
            moerktRum.South = "Loftet";

            // låser hoveddøren og kræver nøgle
            entre.IsLocked = true;
            entre.RequiredKey = "Hus-nøgle";

            // Låser loftet og kræver nøgle
            loft.IsLocked = true;
            loft.RequiredKey = "Loft-nøgle";

            // Returnér hele kortet så resten af spillet kan bruge det
            return rooms;
        }
    }
}
