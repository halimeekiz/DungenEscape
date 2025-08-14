using ConsoleApp1;
using System.Text;

/* 
Spilbeskrivelse:
Dette program er et simpelt tekstbaseret eventyrspil, hvor spilleren
udforsker et hus og dets omgivelser gennem forskellige rum.
*/

internal class Program
{
    private static void Main(string[] args)
    {
        // Sørger for at konsollen kan vise/skrive danske tegn (æøå) og emojis
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        // Ryd skærmen og tegn en lille velkomstskærm
        Console.Clear();

        Console.WriteLine("╔══════════════════════════════════╗");
        Console.WriteLine("║            Velkommen             ║");
        Console.WriteLine("║              til                 ║");
        Console.WriteLine("║        Dungeon Escape            ║");
        Console.WriteLine("╠══════════════════════════════════╣");
        Console.WriteLine("║ Du skal udforske et hus og       ║");
        Console.WriteLine("║ dets omgivelser.                 ║");
        Console.WriteLine("║                                  ║");
        Console.WriteLine("║ Brug kommandoer til at bevæge    ║");
        Console.WriteLine("║ dig og interagere med ting.      ║");
        Console.WriteLine("║                                  ║");
        Console.WriteLine("║ Tryk på en tast for at starte.   ║");
        Console.WriteLine("╚══════════════════════════════════╝");

        // Vent på at spilleren trykker en tast før vi går i gang
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Escape)                       // Hvis spilleren trykker Escape her, afslut straks
            return;                                             // Lukker programmet uden at starte spillet

        // === Initialisering af spillet ===

        Game.rooms = Room.CreateAllRooms();                     // Opretter alle rum og deres forbindelser (dictionary)
        Player.CurrentRoom = Game.rooms["Baghaven"];            // Sætter startpositionen til Baghaven
        Item.PopulateItems(Game.rooms);                         // Lægger start-items (nøgler m.m.) ud i rummene

        // Start hovedloopen (input/bevægelse/hændelser styres i Player.Move)
        Player.Move();                                          // Kører indtil programmet afsluttes indefra
    }
}
