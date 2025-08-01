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

            // Opretter rummene med beskrivende navne
            Room bagHaven = new Room("Baghaven", "Du står i baghaven. En dør i huset står åben mod øst.");                 // Baghaven
            Room kaelder = new Room("Kælder", "Et mørkt og fugtigt kælderrum.");                                           // Kælder
            Room entre = new Room("Entré", "Du står i entreen. En dør fører længere ind i huset.");                        // Entré
            Room koekken = new Room("Køkken", "Et lille køkken med en lugt af brændt toast.");                             // Køkken
            Room entreMedTrappe = new Room("Entré udgangen", "Med trappe til første sal");                                 // Entré med trappe
            Room foersteSal = new Room("Førstesal", "Førstesal – en gang med adgang til flere værelser");                  // Førstesal
            Room sovevaerelse = new Room("Soveværelset", "Du er i soveværelset");                                          // Soveværelse    
            Room moerktRum = new Room("Værelse", "Et mørkt rum hvor noget bevæger sig i skyggerne.");                      // Mørkt rum
            Room boernevaerelse = new Room("Børneværelset", "Et rodet børneværelse med en åben dør mod øst.");             // Børneværelse
            Room loft = new Room("Loftet", "Et støvet loft med en enkelt glødepære i loftet.");                            // Loft

            // Tilføj rummene til ordbogen
            rooms.Add(bagHaven.Id, bagHaven);
            rooms.Add(kaelder.Id, kaelder);
            rooms.Add(entre.Id, entre);
            rooms.Add(koekken.Id, koekken);
            rooms.Add(entreMedTrappe.Id, entreMedTrappe);
            rooms.Add(foersteSal.Id, foersteSal);
            rooms.Add(sovevaerelse.Id, sovevaerelse);
            rooms.Add(moerktRum.Id, moerktRum);
            rooms.Add(boernevaerelse.Id, boernevaerelse);
            rooms.Add(loft.Id, loft);

            // Forbind retninger mellem rummene ved at bruge de beskrivende navne
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

            // lås hoveddøren og kræv nøgle
            entre.IsLocked = true;
            entre.RequiredKey = "Hus - nøgle";

            // Lås loftet og kræv nøgle
            loft.IsLocked = true;
            loft.RequiredKey = "Loft-nøgle";

            return rooms;
        }
    }
}
