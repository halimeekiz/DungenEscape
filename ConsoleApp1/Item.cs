using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    // Item repræsenterer en ting i spillet (fx en nøgle).
    // Lige nu bruger jeg kun typen Key, men klassen kan udvides senere.
    public class Item
    {
        // Navn som vises i spillet (fx "Loft-nøgle")
        public string Name { get; set; }

        // Hvilken type ting det er (her: Key)
        public ItemType Type { get; set; }

        // Simpel konstruktør: jeg sætter navn og type ved oprettelse
        public Item(string name, ItemType type)
        {
            Name = name;
            Type = type;
        }

        // Lægger mine start-items ud i de relevante rum.
        // Her placerer jeg "Loft-nøgle" i Soveværelset og "Hus-nøgle" i Baghaven.
        public static void PopulateItems(Dictionary<string, Room> rooms)
        {
            var loftNoegle = new Item("Loft-nøgle", ItemType.Key);
            var husNoegle = new Item("Hus-nøgle", ItemType.Key);

            rooms["Soveværelset"].Items.Add(loftNoegle);
            rooms["Baghaven"].Items.Add(husNoegle);
        }
    }

    // Typer til mine items. Jeg holder det enkelt og bruger kun Key i dette spil.
    public enum ItemType
    {
        Key
    }
}
