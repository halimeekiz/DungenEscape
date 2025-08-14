using System.Collections.Generic;
using System.Linq;
using System;
using Konsolspil;

namespace ConsoleApp1
{
    // Maps tegner mit lille ASCII-minikort og holder en simpel positionsoversigt
    // for hvert rum (så jeg kan placere boksene korrekt i et grid).
    public static class Maps
    {
        // (x,y)-placering for hvert rum på et lille grid (bruges KUN til rendering af minikortet)
        private static readonly Dictionary<string, (int x, int y)> pos = new()
        {
            ["Baghaven"] = (0, 2),
            ["Entré"] = (1, 2),
            ["Køkken"] = (2, 2),
            ["Entré udgangen"] = (1, 1),
            ["Førstesal"] = (1, 0),
            ["Soveværelset"] = (0, 0),
            ["Børneværelset"] = (2, 0),
            ["Loftet"] = (1, -1),
            ["Værelse"] = (1, -2),
        };

        // Tegner et minikort af rummene som små bokse med forbindelser mellem
        // naboer. currentRoom markeres, låste naboer markeres med 'X' på kanten.
        public static void DrawMiniMap(Dictionary<string, Room> rooms, Room currentRoom, int leftPadding = 0)
        {
            // Jeg beregner cellestørrelser dynamisk, så lange navne ikke knækker rammen.
            int longest = pos.Keys.Max(k => k.Length) + 2;
            int cellW = Math.Max(13, longest + 4);
            int cellH = 3;
            int gapW = 3;
            int gapH = 1;

            // Find grænser for grid’et (så jeg kan dimensionere canvas korrekt)
            int minY = pos.Values.Min(p => p.y);
            int maxY = pos.Values.Max(p => p.y);
            int minX = pos.Values.Min(p => p.x);
            int maxX = pos.Values.Max(p => p.x);

            int cols = maxX - minX + 1;
            int rows = maxY - minY + 1;

            int canvasW = cols * cellW + (cols - 1) * gapW;
            int canvasH = rows * cellH + (rows - 1) * gapH;

            // Selve “pixel”-canvas’et (char-matrix) som jeg skriver tegn ind i
            char[,] c = new char[canvasH, canvasW];
            for (int y = 0; y < canvasH; y++)
                for (int x = 0; x < canvasW; x++)
                    c[y, x] = ' ';

            // Her gemmer jeg bbox for den aktuelle celle, så jeg kan farve den grøn ved udskrivning
            int curCx = -1, curCy = -1;

            // Lille helper til at tegne en boks + centrere rum-navnet i den
            void WriteCellText(int cx, int cy, string text, bool isCurrent, bool isLocked)
            {
                if (text.Length > cellW - 2) text = text[..(cellW - 2)];

                // Kant-tegn (ASCII-boks)
                for (int x = 0; x < cellW; x++) { c[cy, cx + x] = '-'; c[cy + cellH - 1, cx + x] = '-'; }
                for (int y = 0; y < cellH; y++) { c[cy + y, cx] = '|'; c[cy + y, cx + cellW - 1] = '|'; }
                c[cy, cx] = c[cy, cx + cellW - 1] = c[cy + cellH - 1, cx] = c[cy + cellH - 1, cx + cellW - 1] = '+';

                // Låsemarkør (X) på toppen af cellen hvis rummet er låst
                if (isLocked)
                {
                    int lockX = cx + cellW / 2;
                    c[cy, lockX] = 'X';
                }

                // Marker det aktuelle rum med [navn]
                string display = isCurrent ? $"[{text}]" : text;
                if (display.Length > cellW - 2) display = display[..(cellW - 2)];
                int left = cx + (cellW - display.Length) / 2;
                int top = cy + cellH / 2;
                for (int i = 0; i < display.Length; i++) c[top, left + i] = display[i];

                // Gem placering af den aktuelle celle til farvelægning senere
                if (isCurrent) { curCx = cx; curCy = cy; }
            }

            // Tegn “ledninger” (forbindelser) mellem naborum i øst/vest og nord/syd
            foreach (var kv in pos)
            {
                string id = kv.Key;
                if (!rooms.TryGetValue(id, out var r)) continue;

                var (gx, gy) = kv.Value;
                int col = gx - minX;
                int row = maxY - gy; // vend y-aksen, så højere y ligger længere nede

                int cx = col * (cellW + gapW);
                int cy = row * (cellH + gapH);

                // Forbindelse mod øst (vandret linje). Låste naboer tegnes med 'x'
                if (!string.IsNullOrWhiteSpace(r.East) && pos.TryGetValue(r.East, out var eastPos))
                {
                    int col2 = eastPos.x - minX;
                    int cx2 = col2 * (cellW + gapW);
                    int yLine = cy + cellH / 2;
                    bool neighborLocked = rooms.TryGetValue(r.East, out var rEast) && rEast.IsLocked;
                    char wire = neighborLocked ? 'x' : '-';
                    for (int x = cx + cellW; x < cx2; x++) c[yLine, x] = wire;
                }

                // Forbindelse mod nord (lodret linje). Låste naboer tegnes med 'x'
                if (!string.IsNullOrWhiteSpace(r.North) && pos.TryGetValue(r.North, out var northPos))
                {
                    int row2 = maxY - northPos.y;
                    int cy2 = row2 * (cellH + gapH);
                    int xLine = cx + cellW / 2;
                    bool neighborLocked = rooms.TryGetValue(r.North, out var rNorth) && rNorth.IsLocked;
                    char wire = neighborLocked ? 'x' : '|';
                    for (int y = cy2 + cellH; y < cy; y++) c[y, xLine] = wire;
                }
            }

            // Skriv hver rum-celle efter forbindelserne, så bokse ligger “øverst”
            foreach (var kv in pos)
            {
                string id = kv.Key;
                if (!rooms.TryGetValue(id, out var r)) continue;

                var (gx, gy) = kv.Value;
                int col = gx - minX;
                int row = maxY - gy;

                int cx = col * (cellW + gapW);
                int cy = row * (cellH + gapH);

                bool isCurrent = ReferenceEquals(r, currentRoom);
                WriteCellText(cx, cy, id, isCurrent, r.IsLocked);
            }

            // Udskriv canvas til konsollen. Aktuel celle = grøn, 'X' = rød
            var normal = Console.ForegroundColor;
            Console.WriteLine();
            for (int y = 0; y < canvasH; y++)
            {
                if (leftPadding > 0) Console.SetCursorPosition(leftPadding, Console.CursorTop);

                for (int x = 0; x < canvasW; x++)
                {
                    bool inCurrent =
                        curCx >= 0 &&
                        x >= curCx && x < curCx + cellW &&
                        y >= curCy && y < curCy + cellH;

                    char ch = c[y, x];

                    if (inCurrent)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else if (ch == 'X')
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = normal;

                    Console.Write(ch);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = normal;

            // Lille forklaring under kortet
            if (leftPadding > 0) Console.SetCursorPosition(leftPadding, Console.CursorTop);
            Console.WriteLine("Rum = nuværende rum (grøn)   X på kant = låst rum ");
        }
    }

    // Labyrinth er min “hændelses-motor” der bruger et simpelt 2D char-array som struktur:
    // '.', 'F', '#', 'K', 'U' styrer om et felt er tomt, fælde, væg, nøgle eller udgang.
    public static class Labyrinth
    {
        // Symboler (char-kravet)
        public const char Tom = '.';
        public const char Faelde = 'F';
        public const char Vaeg = '#';
        public const char Noegle = 'K';  // nøglefelt i grid'et
        public const char Udgang = 'U';  // udgangsfelt i grid'et

        // 5x5-grid jeg bruger som “sandheden” for felt-typer (krav: 2D-array)
        public static readonly char[,] Grid =
        {
            { Vaeg, Vaeg, Vaeg, Vaeg, Vaeg },
            { Vaeg, Tom , Tom , Tom , Vaeg },
            { Vaeg, Tom , Tom , Tom , Vaeg },
            { Vaeg, Tom , Tom , Tom , Vaeg },
            { Vaeg, Vaeg, Vaeg, Vaeg, Vaeg }
        };

        // Sammenknytning mellem rum-id og “relative” (x,y) så jeg kan finde deres placering i Grid
        private static readonly Dictionary<string, (int x, int y)> RoomPos = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Baghaven"] = (0, 2),
            ["Entré"] = (1, 2),
            ["Køkken"] = (2, 2),
            ["Entré udgangen"] = (1, 1),
            ["Førstesal"] = (1, 0),
            ["Soveværelset"] = (0, 0),
            ["Børneværelset"] = (2, 0),
            ["Loftet"] = (1, -1),
            ["Værelse"] = (1, -2),
        };

        // Sæt af rum som skal være fælder i grid'et (kan udvides)
        private static readonly HashSet<string> TrapRooms = new(StringComparer.OrdinalIgnoreCase)
        {
            "Køkken", "Værelse"
        };

        // (x,y) -> [row,col] i Grid. Jeg bruger Center=2 så midten af 5x5 svarer til (0,0)
        private const int Center = 2; // (row=2,col=2) er midten af 5x5
        private static bool TryGetIndex(string roomId, out int row, out int col)
        {
            row = col = -1;
            if (!RoomPos.TryGetValue(roomId, out var p)) return false;
            // Array-rækker går nedad (stigende row), derfor inverterer jeg y
            col = Center + p.x;
            row = Center - p.y;
            // Sikkerhed: tjek bounds
            return row >= 0 && row < Grid.GetLength(0) && col >= 0 && col < Grid.GetLength(1);
        }

        // Statisk ctor: jeg “brænder” fælder, nøgle og udgang ind i selve Grid,
        // så 2D-arrayet er kilden til hændelserne.
        static Labyrinth()
        {
            foreach (var room in TrapRooms)
            {
                if (TryGetIndex(room, out int r, out int c))
                    Grid[r, c] = Faelde;
            }

            // Nøglen placeres i Soveværelset
            if (TryGetIndex("Soveværelset", out int nr, out int nc))
                Grid[nr, nc] = Noegle;

            // Udgangen placeres på Loftet
            if (TryGetIndex("Loftet", out int ur, out int uc))
                Grid[ur, uc] = Udgang;
        }

        // Hoved-entry når jeg går ind i et rum: kig i Grid og afgør om der er nøgle, udgang eller fælde.
        public static void OnEnterRoom(string roomId)
        {
            if (!TryGetIndex(roomId, out int r, out int c))
                return;

            char felt = Grid[r, c];

            // Vis hvad grid’et siger om feltet (gør det tydeligt at char[,] bruges aktivt)
            Console.WriteLine($"(Symbol på kortet: '{felt}')");

            // Nøglefelt: giv nøgle (kun hvis jeg ikke allerede har den) og fjern symbolet efterfølgende
            if (felt == Noegle)
            {
                Console.WriteLine("Du har fundet nøglen!");
                bool harAllerede = Player.Inventory.Any(i => Actions.Normalize(i.Name) == Actions.Normalize("Loft-nøgle"));
                if (!harAllerede)
                    Player.Inventory.Add(new Item("Loft-nøgle", ItemType.Key));

                Grid[r, c] = Tom; // fjern nøgle fra grid'et så den ikke samles igen
                return;
            }

            // Udgang: kræver nøglen. Hvis jeg har den, vises slutscene; ellers venlig besked.
            if (felt == Udgang)
            {
                bool hasKey = Player.Inventory.Any(i => Actions.Normalize(i.Name) == Actions.Normalize("Loft-nøgle"));
                if (hasKey)
                {
                    ShowWinSceneAndExit();
                }
                else
                {
                    Console.WriteLine("Du ser stjernerne derude... men døren er låst. Du mangler nøglen.");
                }
                return;
            }

            // Fælde: teleportér tilbage til start (Baghaven) og ryd fælden fra grid'et
            if (felt == Faelde)
            {
                Console.WriteLine("Pas på! En fælde – du sendes tilbage til start.");
                if (Game.rooms.TryGetValue("Baghaven", out var start))
                {
                    Player.CurrentRoom = start;
                }
                Grid[r, c] = Tom; // fjern fælden så den ikke udløses igen
                return;
            }

            // Tom/Væg: ingen ekstra handling her (væg rammes ikke via Room-stierne).
        }

        // Små “tekst-animations”-hjælper (skriver langsomt for stemning)
        private static void SlowWrite(string text, int delay)
        {
            foreach (char ch in text)
            {
                Console.Write(ch);
                System.Threading.Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        // Slutscene når jeg når udgangen (Loftet) med nøglen – og derefter afslut spillet.
        private static void ShowWinSceneAndExit()
        {
            Console.Clear();

            string[] stars =
            {
                "        ✦        ✧       ✦",
                "   ✧         ✦        ✧      ✦",
                "        ✦      ✧         ✦",
                "   ✦         ✧       ✦       ✧",
                "        ✧        ✦        ✧"
            };

            foreach (string line in stars)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(line);
                System.Threading.Thread.Sleep(200);
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            SlowWrite("✨ Du har nået loftet! ✨", 60);
            Console.WriteLine();
            SlowWrite("Du åbner loftsvinduet og kigger ud over en stille, klar nattehimmel...", 40);
            Console.WriteLine();
            SlowWrite("Stjernerne blinker som små diamanter, og du føler en dyb ro.", 40);
            Console.WriteLine();
            SlowWrite("🌟 Du har vundet! 🌟", 80);

            Console.ResetColor();
            Console.WriteLine("\nTryk på en tast for at afslutte...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
