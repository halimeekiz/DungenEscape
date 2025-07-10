using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Room // Step 1 - Klassen beskriver et rum i spillet. Den indeholder information om rummet og hvad det kan (fx vise info og holde styr på retninger og ting).

    {
        // Step 2 - Inde i klassen definerer vi egenskaber (kaldet properties), som beskriver rummets data.

        // Unikt Id for rummet
        public string Id { get; set; }
        public bool IsLocked { get; set; } = false;
        public string RequiredKey { get; set; } = null;


        // Beskrivelse af rummet
        public string Description { get; set; }

        // Retninger (peger på ID’er til andre rum)
        public string North { get; set; }
        public string South { get; set; }
        public string East { get; set; }
        public string West { get; set; }


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
            Console.WriteLine($"Rum-ID: {Id}");
            Console.WriteLine($"Beskrivelse: {Description}");

            // Hvis rummet er låst, vis en advarsel
            if (IsLocked)
            {
                Console.WriteLine("⚠️  Døren til dette rum er låst.");
                Console.WriteLine($"   Kræver nøgle: {RequiredKey}");
            }

            Console.WriteLine($"Ting i rummet:");

            if (Items.Count == 0)
            {
                Console.WriteLine(" - Ingen ting i rummet");
            }
            else
            {
                foreach (var item in Items)
                {
                    Console.WriteLine($" - {item.Name}");
                }
            }
        }

        public static Dictionary<string, Room> CreateAllRooms()
        {
            var rooms = new Dictionary<string, Room>();

            Room rum1 = new Room("Rum1", "Du står i baghaven. En dør i huset står åben mod øst.");
            Room rum2 = new Room("Rum2", "Et mørkt og fugtigt kælderrum.");
            Room rum3 = new Room("Rum3", "Du står i entreen. En dør fører længere ind i huset.");
            Room rum4 = new Room("Rum4", "Et lille køkken med en lugt af brændt toast.");
            Room rum5 = new Room("Rum5", "Entré med en trappe op til første sal");
            Room rum6 = new Room("Rum6", "Førstesal – en gang med adgang til flere værelser");
            Room rum7 = new Room("Rum7", "Du er i soveværelset");
            Room rum8 = new Room("Rum8", "Et mørkt rum hvor noget bevæger sig i skyggerne.");
            Room rum9 = new Room("Rum9", "Et rodet børneværelse med en åben dør mod øst.");
            Room rum10 = new Room("Rum10", "Et støvet loft med en enkelt glødepære i loftet.");

            // Tilføj til ordbog
            rooms.Add(rum1.Id, rum1);
            rooms.Add(rum2.Id, rum2);
            rooms.Add(rum3.Id, rum3);
            rooms.Add(rum4.Id, rum4);
            rooms.Add(rum5.Id, rum5);
            rooms.Add(rum6.Id, rum6);
            rooms.Add(rum7.Id, rum7);
            rooms.Add(rum8.Id, rum8);
            rooms.Add(rum9.Id, rum9);
            rooms.Add(rum10.Id, rum10);

            // Forbind retninger
            rum1.East = "Rum3";
            rum3.West = "Rum1";

            rum3.East = "Rum4";
            rum4.West = "Rum3";

            rum3.North = "Rum5";
            rum5.South = "Rum3";

            rum5.North = "Rum6";
            rum6.South = "Rum5";

            rum6.East = "Rum9";
            rum9.West = "Rum6";

            rum6.West = "Rum7";
            rum7.East = "Rum6";

            rum6.North = "Rum10";
            rum10.South = "Rum6";

            rum10.North = "Rum8";
            rum8.South = "Rum10";

            // Lås loftet og kræv nøgle
            rum10.IsLocked = true;
            rum10.RequiredKey = "Loft-nøgle";

            return rooms;
        }


    }
}
