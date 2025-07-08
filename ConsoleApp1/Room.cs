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
            Console.WriteLine($"Ting i rummet:");

            if (Items.Count == 0)
            {
                Console.WriteLine(" - Ingen ting i rummet");
            }
            else
            {
                foreach (var item in Items)
                {
                    Console.WriteLine($" -{item.Name}");
                }
            }
        }
    }
}
