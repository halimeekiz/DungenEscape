using ConsoleApp1;


/* ----------------------------------------------
   Spilbeskrivelse:
   Dette program er et simpelt tekstbaseret eventyrspil, hvor spilleren
   udforsker et hus og dets omgivelser gennem forskellige rum.
  
   Rummene er designet til at give en fornemmelse af et realistisk hus:
   - Spillet starter i baghaven (Rum1) og bevæger sig gennem entré, køkken,
     trappe, første sal, børneværelse, soveværelse, loft og et mørkt rum.
  
   Formålet med så mange rum er at give variation og mulighed for udforskning,
   som passer til spillets opgave: at bevæge sig, samle ting og møde udfordringer.
  
   Spilleren starter i baghaven for at skabe en naturlig begyndelse udenfor,
   og bevæger sig derefter ind i huset.

   Dette er lavet som læring i C#-programmering: klasser, objekter og logik.
 ---------------------------------------------- */


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

Game.rooms.Add("Rum1", rum1);
Game.rooms.Add("Rum2", rum2);
Game.rooms.Add("Rum3", rum3);
Game.rooms.Add("Rum4", rum4);
Game.rooms.Add("Rum5", rum5);
Game.rooms.Add("Rum6", rum6);
Game.rooms.Add("Rum7", rum7);
Game.rooms.Add("Rum8", rum8);
Game.rooms.Add("Rum9", rum9);
Game.rooms.Add("Rum10", rum10);


rum1.East = "Rum3";    // Fra haven --> baghaven (entré)
rum3.West = "Rum1";    // Fra entré --> baghaven

rum3.East = "Rum4";    // Fra entré --> køkken
rum4.West = "Rum3";    // Fra Køkken --> entré

rum3.North = "Rum5";   // Entre --> Trappper 1 sal
rum5.South = "Rum3";   // Trapper 1 sal --> entré (Stue etagen) 

rum5.North = "Rum6";   // Trapper --> Førstesal gangen
rum6.South = "Rum5";   // Førstesal gangen --> Trapper

rum6.East = "Rum9";    //Førstesal gangen --> Børneværelset
rum9.West = "Rum6";    //Børneværelset --> Førstesal gangen

rum6.West = "Rum7";    // Førstesal gangen --> Soveværelset
rum7.East = "Rum6";    // Soveværelset --> Førstesal gangen

rum6.North = "Rum10";  // Førstesal gangen --> Loftet (op ad en lem eller trappe)
rum10.South = "Rum6";  // Loftet --> Førstesal gangen (tilbage ned)

rum10.North = "Rum8";  // Loftet --> Det mørke rum 
rum8.South = "Rum10";  // Det mørke rum --> Loftet (tilbage den vej man kom)

Player.CurrentRoom = rum1; // Spilleren starter i baghaven

// Opret item-objekter
Item kniv = new Item("Kniv", damage: 10, heal: 0, attackPower: 5);
Item potion = new Item("Potion", damage: 0, heal: 20, attackPower: 0);
Item lommelygte = new Item("Lommelygte", damage: 0, heal: 0, attackPower: 0);
Item hjerte = new Item("Hjerte", damage: 0, heal: 50, attackPower:0);
Item giftflaske = new Item("Giftflaske", damage: 30, heal: 50, attackPower: 0);
Item bombe = new Item("Bombe", damage: 0, heal: 0, attackPower: 50);





Console.ReadKey();