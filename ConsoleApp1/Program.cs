using ConsoleApp1;

/* 
Spilbeskrivelse:
Dette program er et simpelt tekstbaseret eventyrspil, hvor spilleren
udforsker et hus og dets omgivelser gennem forskellige rum.
*/

public class Program
{
    private static void Main(string[] args)
    {
        Game.rooms = Room.CreateAllRooms();                // Opret alle rum
        Player.CurrentRoom = Game.rooms["Rum1"];           // Start i baghaven
        Item.PopulateItems(Game.rooms);                    // Læg alle ting og våben i rummene


    }
}