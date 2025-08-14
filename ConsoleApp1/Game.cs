using System.Collections.Generic;

namespace ConsoleApp1
{
    // Game fungerer som en lille "central" for spillets verden.
    // Her ligger den fælles liste (dictionary) over alle rum i spillet.
    public class Game
    {
        /* rooms er min fælles (static) ordbog med alle spillets rum.
           Key  = rummets id/navn (string), fx "Baghaven" eller "Loftet".
           Value = selve Room-objektet for det id.
           
           Static betyder, at jeg kan tilgå den overalt i projektet som Game.rooms
           uden at skulle oprette en Game-instans. */
        public static Dictionary<string, Room> rooms = new Dictionary<string, Room>();
    }

    /*
      Kort fortalt:
      - Game er stedet, hvor jeg gemmer alle rum ét sted.
      - rooms gør, at jeg hurtigt kan slå et rum op på id (string) -> Room.
      - Fordi rooms er static, kan Player, Maps og andre klasser skrive Game.rooms[...] direkte.
      - Jeg opretter/udfylder dictionary'en i Program.cs (start) og bruger den,
        når spilleren flytter sig mellem rum.
    */
}
