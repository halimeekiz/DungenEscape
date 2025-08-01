using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    // Klassen Game fungerer som en slags "kontrolcentral" for spillets verden.
    // Den holder styr på alle rummene i spillet og gør dem tilgængelige for andre klasser.
    public class Game
    {
        /* En offentlig statisk dictionary, som gemmer alle rum i spillet.
           Key (nøglen) er en streng (fx "Rum1"), og value (værdien) er et Room-objekt.
           Statisk betyder, at denne ordbog er fælles og tilgængelig overalt i programmet, uden at man skal oprette et Game-objekt.*/
        public static Dictionary<string, Room> rooms = new Dictionary<string, Room>();

        // En offentlig statisk property til at holde styr på spillerens nuværende rum
        public static Room CurrentRoom { get; set; }
    }

    /*
      - Klassen Game er som en spilkontrol, der gemmer en liste over alle rum.

      - Rooms er en opslagsbog (dictionary), hvor man kan finde et bestemt rum ud fra dets navn/id – fx "Rum1".

      - Static gør, at du kan skrive Game.rooms alle steder i koden uden at lave en ny instans af Game.

      - Det bruges bl.a.i Player.cs og Program.cs, når der skal findes et rum, eller når spilleren flytter sig. */
}

