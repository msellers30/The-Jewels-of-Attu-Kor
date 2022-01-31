using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Game
{
    internal class Game
    {
        private int numberOfColumns = Console.WindowWidth;

        /// <summary>
        /// An array of all the verbs allowed in the game.
        /// </summary>
        private string[] _verbs = { "OPEN", "CLOSE", "GET", "DROP", "PULL", "PUT", "SMOKE", "READ", "EXAMINE", "THROW", "STEAL", "BUY", "GIVE", "LOOK", "DRINK", "GLUE", "WEAR", "REMOVE", "TAKE", "KILL", "RETURN", "BREAK" };

        /// <summary>
        /// An array of all the nouns allowed in the game.
        /// </summary>
        private string[] _nouns = { "BOOK", "CIGARETTE", "BOLOGNA", "NEWSPAPER", "POSTCARD", "BOTTLE", "CLOTHES", "CAMERA", "FRISBEE", "DOOR", "CLEAR", "COBALT", "RED", "ALL", "GLUE", "PHONE", "ORANGE", "OBSIDIAN", "STRING", "COIN", "COKE", "RICE", "CROWBAR", "COVER", "SUBSTANCE", "AMULET", "BEER", "TOILET", "KEYS", "SUN-DROP", "MACHINE", "BAUBLE", "MANHOLE", "RAT", "MAN", "CLERK" };

        /// <summary>
        /// An array of the shortcut commands in the game: North, South, East, West, Up, Down, Look, Inventory, Again (repeat previous command), Z??.
        /// </summary>
        private string[] _shortcuts = { "N", "S", "E", "W", "U", "D", "L", "I", "G" };   // , "Z" - original had Z in the array but it wasn't used.

        /// <summary>
        /// Contains informaiton about the rooms in the game.  The first dimension is for the room number.  
        /// The second dimension contains information about the room.  The first three spots [0-2] are for the
        /// room description.  
        /// 3 is the room status.  
        /// 4 is the direction that the door is in. 1=N, 2=S, 3=E, 4=W
        /// </summary>
        private string[,] _rooms = new string[256, 5];
        private string[] _exits = new string[256];
        private string[] _items = new string[36];
        private double[] _itemLocations = new double[36];
        private string[] status = new string[30];

        private int _currentRoom;

        internal void Play()
        {
            Initialize();
            string roomDescription;
            int shortcutIndex;
            string verb = string.Empty;
            string noun = string.Empty;
            int verbIndex = int.MinValue;
            int nounIndex = int.MinValue;

            int manholeFlag = 0;
            int storeFrontFlag = 1;
            int jailFlag = 0;
            int disguiseFlag = 0;
            int clothingFlag = 0;
            int amuletFlag = 0;
            int quarterFlag = 0;
            int cigaretteFlag = 0;
            int bookFlag = 0;

            string bottleChoice = string.Empty;

        describeRoom:   // 70
            // Print room description.

            //70 FORI=1TO3:IFR$(R,I)<>""THENPRINTR$(R,I); :NEXT:PRINTELSENEXT:PRINT
            roomDescription = string.Empty;
            for (int i = 0; i < 3; i++)
            {
                roomDescription += _rooms[_currentRoom, i]; 
            }
            Print(roomDescription);

        describeStatus: // 71
            // Print any status associated with the room (i.e. The door is open.  The newsstand is empty.)
            //71 IFS$(VAL(R$(R,4)))<>"0"THENPRINTS$(VAL(R$(R,4))):PRINT
            if (_rooms[_currentRoom, 3] != null && status[int.Parse(_rooms[_currentRoom, 3])] != "0")
            {
                Print(status[int.Parse(_rooms[_currentRoom, 3])]);
                Print();
            }

            // Print any items in the room.
            Print("THE ROOM CONTAINS:");

            int j = 0;
            for (int i = 0; i < _items.Length; i++)
            {
                if (_itemLocations[i] == _currentRoom)
                {
                    Print(_items[i]);
                    j++;
                }
            }

            // If nothing is in the room, then say so.
            if (j == 0)
            {
                Print("NOTHING");
            }

            // Read command.
        getCommand:             //80
            Print();
            Console.Write("? ");
            string command = Console.ReadLine();

            if (command.Length == 0)
            {
                Print();
                Print("BEG PARDON?");
                Print();
                goto getCommand;
            }

            if (command == "HELP")
            {
                DisplayHelp();
                goto describeRoom;
            }

            Print();

            // 90 IFLEN(R$)=1THEN180
            if (command.Length == 1) goto quickCommand;

            //100 J=0:FORI=1TOLEN(R$):IFMID$(R$,I,1)=" "THENV$=LEFT$(R$,I-1):N$=RIGHT$(R$,LEN(R$)-I):J=I:IF N$="JEWEL"THENPRINT"PLEASE REFER TO A JEWEL BY ITS COLOR.":GOTO80ELSEGOTO140
            j = int.MinValue;

            for (int i = 0; i < command.Length; i++)
            {
                if (command.Substring(i, 1) == " ")
                {
                    verb = command.Substring(0, i);
                    noun = command.Substring(i + 1);
                    j = i;
                    if (noun == "JEWEL")
                    {
                        Print("PLEASE REFER TO A JEWEL BY ITS COLOR.");
                        goto getCommand;
                    }
                    break;
                }
            }

            //110 NEXT: IFJ=0THENPRINT"TWO WORDS PLEASE!!":GOTO80
            if (j == int.MinValue)
            {
                Print("TWO WORDS PLEASE!!");
                goto getCommand;
            }

            //140 V=0:FORI=1TO22:IFV$=V$(I)THENV=I:I=22
            verbIndex = int.MinValue;

            for (int i = 0; i < _verbs.Length; i++)
			{
                if (_verbs[i] == verb)
                {
                    verbIndex = i;
                    break;
                }
			}

            //145 NEXT:IFV=0THENPRINT"I DON'T KNOW THE VERB "V$: GOTO80
            if (verbIndex == int.MinValue)
            {
                Print("I DON'T KNOW THE VERB {0}", verb);
                goto getCommand;
            }

            //150 N=0:FORI=1TO36:IFN$=N$(I)THENN=I:I=136
            nounIndex = int.MinValue;

            for (int i = 0; i < _nouns.Length; i++)
			{
                if (_nouns[i] == noun)
                {
                    nounIndex = i;
                    break;
                }
			}

            //160 NEXT:IFN=0THENPRINT"I DON'T KNOW THE NOUN "N$:GOTO80
            if (nounIndex == int.MinValue)
            {
                Print("I DON'T KNOW THE NOUN {0}", noun);
                goto getCommand;
            }

        processVerb:
            //170 ONVGOTO319,340,355,369,405,450,500,515,1330,1350,1420,1460,1490,1330,1500, 1550,1600,1620,355,1640,1660,1750
            switch (verbIndex)
            {
                case 0:
                    goto processOpen;
                case 1:
                    goto processClose;
                case 2:
                    goto processGet;
                case 3:
                    goto processDrop;
                case 4:
                    goto processPull;
                case 5:
                    goto processPut;
                case 6:
                    goto processSmoke;
                case 7:
                    goto processRead;
                case 8:
                    goto processExamine;
                case 9:
                    goto processThrow;
                case 10:
                    goto processSteal;
                case 11:
                    goto processBuy;
                case 12:
                    goto processGive;
                case 13:
                    goto processLook;
                case 14:
                    goto processDrink;
                case 15:
                    goto processGlue;
                case 16:
                    goto processWear;
                case 17:
                    goto processRemove;
                case 18:
                    goto processTake;
                case 19:
                    goto processKill;
                case 20:
                    goto processReturn;
                case 21:
                    goto processBreak;
            }
            
            goto describeRoom;


        quickCommand:       // 180
            //180 A=0:FORI=1TO9:IFR$=A$(I)THENA=I:I=9
            //190 NEXT
            shortcutIndex = int.MinValue;

            for (int i = 0; i < _shortcuts.Length; i++)
            {
                if (command == _shortcuts[i])
                {
                    shortcutIndex = i;
                    break;
                }
            }

            //195 IFA=0THENPRINT"I DON'T KNOW THAT COMMAND":GOTO80
            if (shortcutIndex == int.MinValue)
            {
                Print("I DON'T KNOW THAT COMMAND");
                goto getCommand;
            }

            //200 IFA>6THENONA-6GOTO380,390,170
            if (shortcutIndex > 5)
            {
                switch (shortcutIndex)
                {
                    case 6:
                        {
                            goto describeRoom;
                        }
                    case 7:
                        {
                            goto processInventory;
                        }
                    case 8:
                        {
                            goto processVerb;
                        }
                }
            }

            //209 IFR=11ANDA=6ANDAF=3THEN29995
            if (_currentRoom == 10 && shortcutIndex == 5 && amuletFlag == 3) goto winnerWinner;

            //210 IFASC(MID$(E$(R),A, 1))=0THENPRINT"THAT ISNT AN EXIT":GOTO80
            if (Ascii.Asc(_exits[_currentRoom].Substring(shortcutIndex, 1)) == 255)
            {
                Print("THAT ISNT AN EXIT");
                goto getCommand;
            }

            //211 IFR=11ANDA=6AND(I(23)=0ORMF=1)THENPRINT"YOU FALL A CONSIDERABLE DISTANCE DOWN INTO THE SEWER SYSTEM OF  THE CITY.":R=16:GOTO70
            if (_currentRoom == 10 && shortcutIndex == 5 && (double.IsNaN(_itemLocations[22]) || manholeFlag == 1))
            {
                Print("YOU FALL A CONSIDERABLE DISTANCE DOWN INTO THE SEWER SYSTEM OF  THE CITY.");
                _currentRoom = 15;
                goto describeRoom;
            }

            //212 IFR=11ANDA=6ANDMF=0ANDI(23)<>0THENPRINT"YOU CAN'T OPEN THE MANHOLE COVER WITH YOUR BARE HANDS.":GOTO80
            if (_currentRoom == 10 && shortcutIndex == 5 && manholeFlag == 0 && _itemLocations[22] != double.NaN)
            {
                Print("YOU CAN'T OPEN THE MANHOLE COVER WITH YOUR BARE HANDS.");
                goto getCommand;
            }

            //213 IFR=14ANDA=4ANDI(29)=.5THENPRINT"YOU DON'T HAVE THE KEYS. ":GOTO80
            if (_currentRoom == 13 && shortcutIndex == 3 && _itemLocations[28] == .5)
            {
                Print("YOU DON'T HAVE THE KEYS.");
                goto getCommand;
            }

            //214 IFA=4ANDR=14ANDI(29)=0THENPRINT"YOU UNLOCK THE CELL, DRAG THE JAILER IN, AND LOCK HIM UP.  YOU  ARE NOW IN THE FRONT OFFICE OF THE JAIL.":R=12:GOTO80
            if (shortcutIndex == 3 && _currentRoom == 13 && double.IsNaN(_itemLocations[28]))
            {
                Print("YOU UNLOCK THE CELL, DRAG THE JAILER IN, AND LOCK HIM UP.  YOU  ARE NOW IN THE FRONT OFFICE OF THE JAIL.");
                _currentRoom = 11;
                goto getCommand;
            }

            //215IF(R$(R,4)="7"ORR$(R,4)="2")ANDA=VAL(R$(R,5))THENPRINT"SOMETHING SEEMS TO BE BLOCKING YOUR WAY.":GOTO80
            if ((_rooms[_currentRoom, 3] == "7" || _rooms[_currentRoom, 3] == "2") && shortcutIndex == int.Parse(_rooms[_currentRoom, 4]))
            {
                Print("SOMETHING SEEMS TO BE BLOCKING YOUR WAY.");
                goto getCommand;
            }

            //216 IFR=9ANDZZ=0THENZZ=1:R$(9,4)="0"
            if (_currentRoom == 8 && storeFrontFlag == 0)
            {
                storeFrontFlag = 1;
                _rooms[8, 3] = "0";
            }

            //217 IFR=9ANDJF=1THENJF=2:R=14:PRINT"THE S.W.A.T. TEAM COMES AND CARRIES YOU TO THE LOCAL JAIL. THEY TAKE ALL OF YOUR POSSESSIONS. (ONLY YOUR PRIDE AND YOUR CLOTHES REMAIN.)":R$(10,4)="13":FORI=1TO30:IFI(I)=0ANDI<>7THENI(I)=15:NEXT:GOTO80ELSENEXT:GOTO80
            if (_currentRoom == 8 && jailFlag == 1)
            {
                jailFlag = 2;
                _currentRoom = 13;
                Print("THE S.W.A.T. TEAM COMES AND CARRIES YOU TO THE LOCAL JAIL. THEY TAKE ALL OF YOUR POSSESSIONS. (ONLY YOUR PRIDE AND YOUR CLOTHES REMAIN.)");
                _rooms[11, 3] = "13";

                for (int i = 0; i < _itemLocations.Length; i++)
                {
                    if (double.IsNaN(_itemLocations[i]) && i != 6)
                    {
                        _itemLocations[i] = 14;
                    }
                }
                goto getCommand;
            }

            //219 R=ASC(MID$(E$(R),A,1)):IFR<>10 THEN70
            _currentRoom = Ascii.Asc(_exits[_currentRoom].Substring(shortcutIndex, 1));
            if (_currentRoom != 9) goto describeRoom;
            
            //220 IFR=10ANDDF=0ANDF1=1THENDF=1:GOTO70
            if (_currentRoom == 9 && disguiseFlag == 0 && clothingFlag == 1)
            {
                disguiseFlag = 1;
                goto describeRoom;
            }
            //222 IFR=10ANDDF=0ANDF1=0THENDF=2:GOTO70
            if (_currentRoom == 9 && disguiseFlag == 0 && clothingFlag == 0)
            {
                disguiseFlag = 2;
                goto describeRoom;
            }

            //224 IFR=10ANDDF=1ANDF1=1ANDJF=2THENPRINT"THE CLERK RECOGNIZES YOU AS A VANDAL AND BOOTS YOU OUT OF THE   STORE.":R=9:GOTO70
            if (_currentRoom == 9 && disguiseFlag == 1 && clothingFlag == 1 && jailFlag == 2)
            {
                Print("THE CLERK RECOGNIZES YOU AS A VANDAL AND BOOTS YOU OUT OF THE STORE.");
                _currentRoom = 8;
                goto describeRoom;
            }

            //226 IFR=10ANDDF=2ANDF1=0ANDJF=2THENPRINT"THE CLERK RECOGNIZES YOU AS A VANDAL AND BOOTS YOU OUT OF THE   STORE.":R=9:GOTO70
            if (_currentRoom == 9 && disguiseFlag == 2 && clothingFlag == 0 && jailFlag == 2)
            {
                Print("THE CLERK RECOGNIZES YOU AS A VANDAL AND BOOTS YOU OUT OF THE STORE.");
                _currentRoom = 8;
                goto describeRoom;
            }

            //228 GOTO70
            goto describeRoom;

        processOpen:        // 319
            //319 IFN$<>"COVER"ANDN$<>"DOOR"ANDN$<>"MANHOLE"THENPRINT"YOU CAN'T OPEN  THAT.":GOTO80
            if (noun != "COVER" && noun != "DOOR" && noun != "MANHOLE")
            {
                Print("YOU CAN'T OPEN  THAT.");
                goto getCommand;
            }

            //320 IFN$="DOOR"THEN745
            if (noun == "DOOR") goto openDoor;

            //321 IFR=11 ANDI(23)=0ANDMF=0THENPRINT"USING THE CROWBAR, YOU PRY THE COVER OFF OF THE MANHOLE.":MF=1:GOTO80
            if (_currentRoom == 10 && double.IsNaN(_itemLocations[22]) && manholeFlag == 0)
            {
                Print("USING THE CROWBAR, YOU PRY THE COVER OFF OF THE MANHOLE.");
                manholeFlag = 1;
                goto getCommand;
            }

            //322 IFR=11ANDMF=1 THENPRINT"THE MANHOLE IS ALREADY OPEN.":GOTO80
            if (_currentRoom == 10 && manholeFlag == 1)
            {
                Print("THE MANHOLE IS ALREADY OPEN.");
                goto getCommand;
            }
            //323 IFR=11ANDI(23)<>0THENPRINT"YOU CAN'T OPEN THE MANHOLE WITH YOUR BARE HANDS.":GOTO80
            if (_currentRoom == 10 && _itemLocations[22] != double.NaN)
            {
                Print("YOU CAN'T OPEN THE MANHOLE WITH YOUR BARE HANDS.");
                goto getCommand;
            }

            //324 PRINT"YOU DON'T SEE A MANHOLE. ":GOTO80
            Print("YOU DON'T SEE A MANHOLE.");
            goto getCommand;

        processClose:       // 340
            //340 IFN$<>"COVER"ANDN$<>"DOOR"ANDN$<>"MANHOLE"THENPRINT"YOU CAN'T CLOSE THAT.":GOTO80
            if (noun != "COVER" && noun != "DOOR" && noun != "MANHOLE")
            {
                Print("YOU CAN'T CLOSE THAT.");
                goto getCommand;
            }

            //341 IFN$="DOOR"THEN1250
            if (noun == "DOOR") goto closeDoor;

            //342 IFR<>11THENPRINT"YOU DON'T SEE A MANHOLE HERE. ":GOTO80
            if (_currentRoom != 10)
            {
                Print("YOU DON'T SEE A MANHOLE HERE. ");
                goto getCommand;
            }

            //343 IFMF=1 THENPRINT"YOU SLIDE THE COVER BACK OVER THE HOLE.":MF=0:GOTO80
            if (manholeFlag == 1)
            {
                Print("YOU SLIDE THE COVER BACK OVER THE HOLE.");
                manholeFlag = 0;
                goto getCommand;
            }

            //344 PRINT"THE MANHOLE ISN'T OPEN.":GOTO80
            Print("THE MANHOLE ISN'T OPEN.");
            goto getCommand;

            //350 PRINT"YOU CANT CLOSE THE "N$:GOTO70
            Print("YOU CANT CLOSE THE ", noun);
            goto describeRoom;

        processGet:         // 355
            //355 IFI(N)=0THENPRINT"IT'S ALREADY IN YOUR POSSESSION.":GOTO80
            if (double.IsNaN(_itemLocations[nounIndex]))
            {
                Print("IT'S ALREADY IN YOUR POSSESSION.");
                goto getCommand;
            }

            //360ONNGOTO365,365,365,361,362,1700,365,365,1300,375,365,365,365,1400,1435,365,365,365,365,1320,1450,1450,365,365,365,365,365,375,1530,365,365,365,365,375,375,375
            switch (nounIndex)
            {
#region get nouns
                case 0:
                    goto getItem;       // 365
                case 1:
                    goto getItem;       // 365
                case 2:
                    goto getItem;       // 365
                case 3:
                    goto getNewspaper;  // 361
                case 4:
                    goto getPostcard;   // 362
                case 5:
                    goto getBottle;     // 1700
                case 6:
                    goto getItem;       // 365
                case 7:
                    goto getItem;       // 365
                case 8:
                    goto getFrisbee;    // 1300
                case 9:
                    goto processImpossible; // 375: Door
                case 10:
                    goto getItem;       // 365
                case 11:
                    goto getItem;       // 365
                case 12:
                    goto getItem;       // 365
                case 13:
                    goto getAll;        // 1400
                case 14:
                    goto getGlue;       // 1435
                case 15:
                    goto getItem;       // 365
                case 16:
                    goto getItem;       // 365
                case 17:
                    goto getItem;       // 365
                case 18:
                    goto getItem;       // 365
                case 19:
                    goto getCoin;       // 1320
                case 20:
                    goto getHerring;    // 1450: Coke
                case 21:
                    goto getHerring;    // 1450: Rice
                case 22:
                    goto getItem;       // 365
                case 23:
                    goto getItem;       // 365
                case 24:
                    goto getItem;       // 365
                case 25:
                    goto getItem;       // 365
                case 26:
                    goto getItem;       // 365
                case 27:
                    goto processImpossible; // 375: Toilet
                case 28:
                    goto getKeys;       // 1530: Keys
                case 29:
                    goto getItem;       // 365
                case 30:
                    goto getItem;       // 365
                case 31:
                    goto getItem;       // 365
                case 32:
                    goto getItem;       // 365
                case 33:
                    goto processImpossible; // 375: Rat
                case 34:
                    goto processImpossible; // 375: Man
                case 35:
                    goto processImpossible; // 375: Clerk
#endregion
            }

        getNewspaper:
            //361 IFI(N)<0THENIFN=4ANDR=1ANDR$(1,4)="3"THENR$(1,4)="5"
            if (_itemLocations[nounIndex] % 1.0 == .1)
            {
                if (nounIndex == 3 && _currentRoom == 0 && _rooms[0, 3] == "3") _rooms[0, 3] = "5";
            }

        getPostcard:
            //362 IFI(N)<0THENIFN=5ANDR=1ANDR$(1,4)="3"THENR$(1,4)="4"
            if (_itemLocations[nounIndex] % 1.0 == .1)
            {
                if (nounIndex == 4 && _currentRoom == 0 && _rooms[0, 3] == "3") _rooms[0, 3] = "4";
            }

            //363 IFI(N)<0THENIFN=4ANDR=1ANDR$(1,4)="4"THENR$(1,4)="6"
            if (_itemLocations[nounIndex] % 1.0 == .1)
            {
                if (nounIndex == 3 && _currentRoom == 0 && _rooms[0, 3] == "4") _rooms[0, 3] = "6";
            }

            //364 IFI(N)<0THENIFN=5ANDR=1ANDR$(1,4)="5"THENR$(1,4)="6"
            if (_itemLocations[nounIndex] % 1.0 == .1)
            {
                if (nounIndex == 4 && _currentRoom == 0 && _rooms[0, 3] == "5") _rooms[0, 3] = "6";
            }

        getItem:
            //365 IFABS(I(N))=RTHENI(N)=0:PRINT"TAKEN.":GOTO80
            // Use decimal of .1 to indicate that it's in the room, but not listed.
            if (Math.Truncate(_itemLocations[nounIndex]) == _currentRoom && _itemLocations[nounIndex] % 1.0 <= 0.1)
            {
                _itemLocations[nounIndex] = double.NaN;
                Print("TAKEN.");
                goto getCommand;
            }

            //367 IFI(N)=0THENPRINT "YOU ALREADY HAVE THAT.":GOTO80
            if (double.IsNaN(_itemLocations[nounIndex]))
            {
                Print("YOU ALREADY HAVE THAT.");
                goto getCommand;
            }

        processDrop:        // 369
            //369 IFN=7ANDF1=1THENPRINT"WHY DON'T YOU REMOVE THEM FIRST.":GOTO80
            if (nounIndex == 6 && clothingFlag == 1)
            {
                Print("WHY DON'T YOU REMOVE THEM FIRST.");
                goto getCommand;
            }

            //370IFI(30)=0AND(N=14ORN=6ORN=30)ANDI$(30)="SUN-DROP"THENPRINT"THE BOTTLE BREAKS IN HALF REVEALING AN ORANGE JEWEL, WHICH YOU PROCEED TO TAKE.":I(17)=0:I$(30)="BROKEN BOTTLE":I(30)=R:GOTO80
            if (double.IsNaN(_itemLocations[29]) && (nounIndex == 13 || nounIndex == 5 || nounIndex == 29) && _items[29] == "SUN-DROP")
            {
                Print("THE BOTTLE BREAKS IN HALF REVEALING AN ORANGE JEWEL, WHICH YOU PROCEED TO TAKE.");
                _itemLocations[16] = double.NaN;
                _items[29] = "BROKEN BOTTLE";
                _itemLocations[29] = _currentRoom;
                goto getCommand;
            }

            //371 IFN=14THENFORI=1TO30:IFI(I)=0THENI(I)=R:PRINTN$(I)": DROPPED.":NEXT:GOTO80ELSENEXT:GOTO80
            if (nounIndex == 13)
            {
                for (int i = 0; i < _itemLocations.Length; i++)
                {
                    if (double.IsNaN(_itemLocations[i]))
                    {
                        _itemLocations[i] = _currentRoom;
                        Print("{0}: DROPPED.", _nouns[i]);
                    }
                }
                goto getCommand;
            }

            //372 IF(N=30ORN=6)ANDI(30)=0THENPRINT"DROPPED.":I(30)=R:GOTO80
            if ((nounIndex == 29 || nounIndex == 5) && double.IsNaN(_itemLocations[29]))
            {
                Print("DROPPED.");
                _itemLocations[29] = _currentRoom;
                goto getCommand;
            }

            //373 IFI(N)=0THENPRINT"DROPPED.":I(N)=R:GOTO80
            if (double.IsNaN(_itemLocations[nounIndex]))
            {
                Print("DROPPED.");
                goto getCommand;
            }

        processImpossible:      // 375
            //375 PRINT"YOU CAN'T DO THAT.":GOTO80
            Print("YOU CAN'T DO THAT.");
            goto getCommand;

        processInventory:   // 390
            //390 J=0:PRINT"YOU ARE CARRYING:":FORI=1TO35:IFI(I)=0THENPRINTI$(I):J=J+1
            //392 NEXT
            j = 0;
            Print("YOU ARE CARRYING:");
            for (int i = 0; i < _itemLocations.Length; i++)
            {
                if (double.IsNaN(_itemLocations[i]))
                {
                    Print(_items[i]);
                    j++;
                }
            }

            //395 IFJ=0THENPRINT"NOTHING"
            if (j == 0) Print("NOTHING");

            //400 GOTO80
            goto getCommand;

        processPull:        // 405
            //405 IFN=6ANDR=5ANDR$(5,4)="7"THENPRINT"A PORTION OF THE SOUTH WALL SLIDES FROM VIEW. ":R$(5,4)="8":GOTO71
            if (nounIndex == 5 && _currentRoom == 4 && _rooms[4, 3] == "7")
            {
                Print("A PORTION OF THE SOUTH WALL SLIDES FROM VIEW. ");
                _rooms[4, 3] = "8";
                goto describeStatus;
            }

            //445 PRINT"WHY WOULD YOU WANT TO PULL THAT?":GOTO80
            Print("WHY WOULD YOU WANT TO PULL THAT?");
            goto getCommand;

        processPut:         // 450
            //450 PRINT"WHERE DO YOU WANT TO PUT THE "N$(N);:INPUTW$:PRINT
            Print("WHERE DO YOU WANT TO PUT THE {0}", noun);
            string putTarget = Console.ReadLine();
            Print();

            //452 IFW$<>"STAND"ANDW$<>"AMULET"THENPRINT"YOU CAN'T PUT THAT THERE.":GOTO80
            if (putTarget != "STAND" && putTarget != "AMULET")
            {
                Print("YOU CAN'T PUT THAT THERE.");
                goto getCommand;
            }

            //455 IFW$="STAND"THEN478
            if (putTarget == "STAND") goto putOnStand;

            //456 IFN<>12ANDN<>17THENPRINT"THAT WON'T FIT IN THE AMULET.":GOTO80
            if (nounIndex != 11 && nounIndex != 16)
            {
                Print("THAT WON'T FIT IN THE AMULET.");
                goto getCommand;
            }

            //457 IFI(26)<>0THENPRINT"YOU DON'T EVEN HAVE THE AMULET.":GOTO80
            if (!double.IsNaN(_itemLocations[25]))
            {
                Print("YOU DON'T EVEN HAVE THE AMULET.");
            }

            //459 IFI(N)<>0THENPRINT"YOU DON'T HAVE THE "N$" JEWEL.":GOTO80
            if (!double.IsNaN(_itemLocations[nounIndex]))
            {
                Print("YOU DON'T HAVE THE {0} JEWEL.", noun);
                goto getCommand;
            }

            //460 IF(AF=1ANDN=12)OR(AF=2ANDN=17)ORAF=3THENPRINT"THE AMULET ALREADY HOLDS THAT JEWEL.":GOTO80
            if ((amuletFlag == 1 && nounIndex == 11) || (amuletFlag == 2 && nounIndex == 16) || amuletFlag == 3)
            {
                Print("THE AMULET ALREADY HOLDS THAT JEWEL.");
                goto getCommand;
            }

            //461IF(AF=1ANDN=17)OR(AF=2ANDN=12)THENPRINT"DONE.":AF=3:I$(26)="AN AMULET CONTAINING ORANGE AND COBALT JEWELS":I(N)=.5:IFR=16THEN29995ELSE80
            if ((amuletFlag == 1 && nounIndex == 16) || (amuletFlag == 2 && nounIndex == 11))
            {
                Print("DONE.");
                amuletFlag = 3;
                _items[25] = "AN AMULET CONTAINING ORANGE AND COBALT JEWELS";
                _itemLocations[nounIndex] = 0.5;
                if (_currentRoom == 15) goto winnerWinner; else goto getCommand;
            }

            //462 IFN=12THENPRINT"DONE.":AF=1:I$(26)="AN AMULET CONTAINING A COBALT JEWEL":I(12)=.5:GOTO80
            if (nounIndex == 11)
            {
                Print("DONE.");
                amuletFlag = 1;
                _items[25] = "AN AMULET CONTAINING A COBALT JEWEL";
                _itemLocations[11] = 0.5;
                goto getCommand;
            }

            //463 IFN=17THENPRINT"DONE.":AF=2:I$(26)="AN AMULET CONTAINING AN ORANGE JEWEL":I(17)=.5:GOTO80
            if (nounIndex == 16)
            {
                Print("DONE.");
                amuletFlag = 2;
                _items[25] = "AN AMULET CONTAINING AN ORANGE JEWEL";
                _itemLocations[16] = 0.5;
                goto getCommand;
            }

            //464 PRINT"SHANNON...MATT...MARK...TIM ALL SAY HELLO!!!":GOTO80
            Print("SHANNON...MATT...MARK...TIM ALL SAY HELLO!!!");
            goto getCommand;

        putOnStand:         // 478
            //478 IFN<>4ANDN<>5THENPRINT"THAT WON'T FIT IN THE STAND.":GOTO80
            if (nounIndex != 3 && nounIndex != 4)
            {
                Print("THAT WON'T FIT IN THE STAND.");
                goto getCommand;
            }

            //480IF(N=4ANDR$(1,4)="5")OR(N=5ANDR$(1,4)="4")THENR$(1,4)="3":GOTO71
            if ((nounIndex == 3 && _rooms[0, 3] == "5") || (nounIndex == 4 && _rooms[0, 3] == "4"))
            {
                _rooms[0, 3] = "3";
                _itemLocations[nounIndex] = 0.1;        // MDS: Bug fix.  Change the location to being back in the room.
                goto describeStatus;
            }

            //485 IFR$(1,4)="6"THENR$(1,4)=STR$(N):GOTO71
            if (_rooms[0, 3] == "6")
            {
                _rooms[0, 3] = (nounIndex + 1).ToString();
                _itemLocations[nounIndex] = 0.1;        // MDS: Bug fix.  Change the location to being back in the room.
                goto describeStatus;
            }

            //490 PRINT"THE STAND IS NOT A GOOD PLACE FOR THAT":GOTO80
            Print("THE STAND IS NOT A GOOD PLACE FOR THAT");
            goto getCommand;

        processSmoke:       // 500
            //500 IFN=2ANDI(N)=0THENPRINT"WARNING: THE SURGEON GENERAL HAS DETERMINED THAT CIGARETTE      SMOKING CAUSES CANCER.  (BUT IT'S BEEN A WHILE SINCE YOUR LAST  PUFF SO YOU DECIDE IT WAS WORTH THE RISK.)":CF=CF+1:IFCF=3THEN501ELSE80
            if (nounIndex == 1 && double.IsNaN(_itemLocations[nounIndex]))
            {
                Print("WARNING: THE SURGEON GENERAL HAS DETERMINED THAT CIGARETTE SMOKING CAUSES CANCER.  (BUT IT'S BEEN A WHILE SINCE YOUR LAST  PUFF SO YOU DECIDE IT WAS WORTH THE RISK.)");
                cigaretteFlag++;
                if (cigaretteFlag == 3) goto cigaretteDeath; else goto getCommand;
            }

        cigaretteDeath:
            //501 IFCF=3THENPRINT:PRINT:PRINT"WE TOLD YOU IT CAUSED CANCER. MAYBE IT'S NOT YOUR FAULT, IT'S PROBABLY BEEN BREWING IN YOU FOR YEARS.":PRINT"*** YOU HAVE DIED ***":END
            if (cigaretteFlag == 3)
            {
                Print();
                Print();
                Print("WE TOLD YOU IT CAUSED CANCER. MAYBE IT'S NOT YOUR FAULT, IT'S PROBABLY BEEN BREWING IN YOU FOR YEARS.");
                Print("*** YOU HAVE DIED ***");
                return;
            }

            //505 IFN=1ANDI(N)=0THENPRINT"CHAPTER 6 TASTES ESPECIALLY NICE. MAYBE YOU SHOULD ALSO SMOKE THE TABLE OF CONTENTS. THE BOOK IS NOW COMPLETELY USELESS.":BF=BF+1:GOTO80
            if (nounIndex == 0 && double.IsNaN(_itemLocations[nounIndex]))
            {
                Print("CHAPTER 6 TASTES ESPECIALLY NICE. MAYBE YOU SHOULD ALSO SMOKE THE TABLE OF CONTENTS. THE BOOK IS NOW COMPLETELY USELESS.");
                bookFlag++;
                goto getCommand;
            }

            //510 PRINT"THE "N$" ISN'T LIT, AND YOU DON'T SEEM TO HAVE A MEANS OF LIGHTING IT.":GOTO80
            Print("THE {0} ISN'T LIT, AND YOU DON'T SEEM TO HAVE A MEANS OF LIGHTING IT.", noun);
            goto getCommand;

        processRead:        // 515

        openDoor:           // 745
            //745 IFR=12ANDI(29)=0THENR$(R,4)="0":PRINT"YOU USE YOUR STOLEN KEYS TO OPEN THE DOOR TO THE EVIDENCE ROOM.":GOTO80
            if (_currentRoom == 11 && double.IsNaN(_itemLocations[28]))
            {
                _rooms[_currentRoom, 3] = "0";
                Print("YOU USE YOUR STOLEN KEYS TO OPEN THE DOOR TO THE EVIDENCE ROOM.");
                goto getCommand;
            }
            //746 IFR=12ANDI(29)<>0THENPRINT"THE DOOR IS LOCKED AND YOU DON'T SEEM TO HAVE THE KEYS.  WHAT A PITY.":GOTO80
            if (_currentRoom == 11 && _itemLocations[28] != double.NaN)
            {
                Print("THE DOOR IS LOCKED AND YOU DON'T SEEM TO HAVE THE KEYS.  WHAT A PITY.");
                goto getCommand;
            }

            //750 IFR$(R,4)="2"THENR$(R,4)="1":PRINT"THE DOOR IS NOW OPEN":R$(ASC(MID$(E$(R),VAL(R$(R,5)),1)),4)="1":GOTO80
            if (_rooms[_currentRoom, 3] == "2")
            {
                _rooms[_currentRoom, 3] = "1";
                Print("THE DOOR IS NOW OPEN");

                Debug.WriteLine("Current room: " + _currentRoom);
                Debug.WriteLine("Door direction: " + _rooms[_currentRoom, 4]);
                Debug.WriteLine("Door leads to: " + Ascii.Asc(_exits[_currentRoom].Substring(int.Parse(_rooms[_currentRoom, 4]), 1)));
                _rooms[Ascii.Asc(_exits[_currentRoom].Substring(int.Parse(_rooms[_currentRoom, 4]), 1)), 3] = "1";
                goto getCommand;
            }

            //755 IFR$(R,4)="1"THENPRINT"WHY WOULD YOU WANT TO RE-OPEN AN ALREADY OPEN DOOR MR. EINSTEIN.":GOTO80
            if (_rooms[_currentRoom, 3] == "1")
            {
                Print("WHY WOULD YOU WANT TO RE-OPEN AN ALREADY OPEN DOOR MR. EINSTEIN.");
                goto getCommand;
            }

            //756 PRINT"I DON'T SEE A DOOR HERE":GOTO80
            Print("I DON'T SEE A DOOR HERE");
            goto getCommand;

        closeDoor:          // 1250
            //1250 IFR$(R,4)="1"THENR$(R,4)="2":PRINT"THE DOOR IS NOW CLOSED":R$(ASC(MID$(E$(R),VAL(R$(R,5)),1)),4)="2":GOTO80
            if (_rooms[_currentRoom, 3] == "1")
            {
                _rooms[_currentRoom, 3] = "2";
                Print("THE DOOR IS NOW CLOSED");
                _rooms[Ascii.Asc(_exits[_currentRoom].Substring(int.Parse(_rooms[_currentRoom, 4]), 1)), 3] = "2";
                goto getCommand;
            }

            //1255 IFR$(R,4)="2"THENPRINT"THAT ALREADY CLOSED DOOR WILL PROBABLY NEVER OPEN AGAIN.":GOTO80
            if (_rooms[_currentRoom, 3] == "2")
            {
                Print("THAT ALREADY CLOSED DOOR WILL PROBABLY NEVER OPEN AGAIN.");
                goto getCommand;
            }
            
            //1260 PRINT"YOU CAN'T CLOSE A DOOR THAT ISN'T HERE.":GOTO80
            Print("YOU CAN'T CLOSE A DOOR THAT ISN'T HERE.");
            goto getCommand;

        getFrisbee:         // 1300
            //1300 IFR=7ANDR$(7,4)="9"THENPRINT"THAT WAS AS EASY AS TAKING CANDY FROM A BABY.":R$(7,4)="10":I(9)=0:I$(9)="A WHAM-O FRISBEE":GOTO71
            if (_currentRoom == 6 && _rooms[6, 3] == "9")
            {
                Print("THAT WAS AS EASY AS TAKING CANDY FROM A BABY.");
                _rooms[6, 3] = "10";
                _itemLocations[8] = double.NaN;
                _items[8] = "A WHAM-O FRISBEE";
                goto describeStatus;
            }

            //1305 IFI(N)=0THENPRINT"YOU ALREADY HAVE THE FRISBEE.":GOTO80
            if (double.IsNaN(_itemLocations[nounIndex]))
            {
                Print("YOU ALREADY HAVE THE FRISBEE.");
                goto getCommand;
            }

            //1307 IFI(9)=RTHENI(9)=0:PRINT"TAKEN.":GOTO80
            if (_itemLocations[8] == _currentRoom)
            {
                Print("TAKEN.");
                goto getCommand;
            }

            //1310 PRINT"YOU CAN'T GET THE FRISBEE FROM HERE.":GOTO80
            Print("YOU CAN'T GET THE FRISBEE FROM HERE.");
            goto getCommand;

        getCoin:            // 1320
            //1320 PRINT"YOU DON'T SEE A COIN HERE.":GOTO80
            Print("YOU DON'T SEE A COIN HERE.");
            goto getCommand;

        processExamine:     // 1330
        processLook:        // 1330
            //1330 IFZ=0ANDN=16ANDR=8THENPRINT"UPON EXAMINATION OF THE PHONE, YOU FIND A QUARTER IN THE SLOT.  YOU ARE NOW TWENTY-FIVE CENTS RICHER.":Z=1:I$(20)="A SHINY QUARTER":I(20)=0:GOTO80
            if (quarterFlag == 0 && nounIndex == 15 && _currentRoom == 7)
            {
                Print("UPON EXAMINATION OF THE PHONE, YOU FIND A QUARTER IN THE SLOT.  YOU ARE NOW TWENTY-FIVE CENTS RICHER.");
                quarterFlag = 1;
                _items[19] = "A SHINY QUARTER";
                _itemLocations[19] = double.NaN;
                goto getCommand;

            }
            //1331 IFR=4ANDN=31THENPRINT"THE MACHINE CONSISTS OF A DIME SIZED SLOT AND A TRAY AT THE END OF A CHUTE.  A GLASS GLOBE ON THE TOP OF THE MACHINE ALLOWS YOU TO SEE THE BAUBLES.":GOTO80
            if (_currentRoom == 3 && nounIndex == 30)
            {
                Print("THE MACHINE CONSISTS OF A DIME SIZED SLOT AND A TRAY AT THE END OF A CHUTE.  A GLASS GLOBE ON THE TOP OF THE MACHINE ALLOWS YOU TO SEE THE BAUBLES.");
                goto getCommand;
            }

            //1332 IFR=11ANDN=31THENPRINT"IT LOOKS JUST LIKE ANY OTHER SUN-DROP MACHINE EXCEPT THAT THEY  ONLY COST A QUARTER.":GOTO80
            if (_currentRoom == 10 && nounIndex == 30)
            {
                Print("IT LOOKS JUST LIKE ANY OTHER SUN-DROP MACHINE EXCEPT THAT THEY ONLY COST A QUARTER.");
                goto getCommand;
            }

            //1334 IFN=28ANDR=14ANDI(27)<>.5THENPRINT"THE TOILET ONLY CONTAINS... WELL YOU DON'T WANT TO KNOW.":GOTO80
            if (nounIndex == 27 && _currentRoom == 13 && _itemLocations[26] != .5)
            {
                Print("THE TOILET ONLY CONTAINS... WELL YOU DON'T WANT TO KNOW.");
                goto getCommand;
            }

            //1335 IFZ=1ANDN=16ANDR=8THENPRINT"IT'S JUST LIKE ANY OTHER PAY PHONE YOU'VE EVER SEEN.":GOTO80
            if (quarterFlag == 1 && nounIndex == 15 && _currentRoom == 7)
            {
                Print("IT'S JUST LIKE ANY OTHER PAY PHONE YOU'VE EVER SEEN.");
                goto getCommand;
            }

            //1336 IFN=28ANDR=14ANDI(27)=.5THENPRINT"AT FIRST GLANCE THE COMODE CONTAINS NOTHING BUT ORGANIC REMAINS BUT, ON A HUNCH, YOU CHECK OUT THE TANK AND FIND A SIX-PACK OF  COORS LIGHT.":I(27)=0:GOTO80
            if (nounIndex == 27 && _currentRoom == 13 && _itemLocations[26] == .5)
            {
                Print("AT FIRST GLANCE THE COMODE CONTAINS NOTHING BUT ORGANIC REMAINS BUT, ON A HUNCH, YOU CHECK OUT THE TANK AND FIND A SIX-PACK OF COORS LIGHT.");
                _itemLocations[26] = 0;
                goto getCommand;
            }

            //1337 IF(I(N)<>0ANDI(N)<>R)THENPRINT"THERE'S NO ";N$;" HERE TO SEE.":GOTO80
            if (!double.IsNaN(_itemLocations[nounIndex]) && _itemLocations[nounIndex] != _currentRoom)
            {
                Print("THERE'S NO {0} HERE TO SEE.", noun);
                goto getCommand;
            }

            //1338 IfI(30)=0ANDN=30ANDI$(30)="SUN-DROP"THENPRINT"YOU NOTICE SOMETHING IN THE BOTTLE.":GOTO80
            if (double.IsNaN(_itemLocations[29]) && nounIndex == 29 && _items[29] == "SUN-DROP")
            {
                Print("YOU NOTICE SOMETHING IN THE BOTTLE.");
                goto getCommand;
            }

            //1340 PRINT"IT LOOKS JUST LIKE ANY OTHER ";N$;" YOU HAVE EVER SEEN.":GOTO80
            Print("IT LOOKS JUST LIKE ANY OTHER {0} YOU HAVE EVER SEEN.", noun);
            goto getCommand;

        processThrow:       // 1350
            //1350 IF(R$(R, 4)="11"ORR$(R,4)="13")AND(R=9ORR=10)AND N=9 ANDI(9)=0THENPRINT"GREAT SHOT!!  THE FRISBEE SAILS THROUGH THE WINDOW WITH THE GREATEST OF EASE.  THE CLERK GOES TO GET THE POLICE, LEAVING THESTORE A PERFECT TARGET FOR THIEVES":GOTO1355
            if ((_rooms[_currentRoom, 3] == "11" || _rooms[_currentRoom, 3] == "13") && (_currentRoom == 8 || _currentRoom == 9) && nounIndex == 8 && double.IsNaN(_itemLocations[8]))
            {
                Print("GREAT SHOT!!  THE FRISBEE SAILS THROUGH THE WINDOW WITH THE GREATEST OF EASE.  THE CLERK GOES TO GET THE POLICE, LEAVING THESTORE A PERFECT TARGET FOR THIEVES");
                goto frisbeeThrown;
            }

            //1351 IFI(N)=0THENPRINT"THROWN":I(N)=R:GOTO80
            if (double.IsNaN(_itemLocations[nounIndex]))
            {
                Print("THROWN");
                _itemLocations[nounIndex] = _currentRoom;
                goto getCommand;
            }

            //1352 PRINT"SINCE YOU DON'T HAVE IT, YOU THROW A FIT.  LOSING YOUR TEMPER WON'T HELP ANYTHING.":GOTO80
            Print("SINCE YOU DON'T HAVE IT, YOU THROW A FIT.  LOSING YOUR TEMPER WON'T HELP ANYTHING.");
            goto getCommand;

        frisbeeThrown:
            //1355 R$(9,4)="12":R$(10,4)="12":I(9)=.5:JF=1:GOTO80
            _rooms[8, 3] = "12";
            _rooms[9, 3] = "12";
            _itemLocations[8] = 0.5;
            jailFlag = 1;
            goto getCommand;

        getAll:             // 1400
            //1400 FORI=1TO35:IFABS(I(I))=RTHENI(I)=0:PRINTN$(I)": TAKEN."
            //1401 NEXT:IFR=1THENR$(1,4)="6"
            //1405 GOTO80
            for (int i = 0; i < _itemLocations.Length; i++)
            {
                if (Math.Truncate(_itemLocations[i]) == _currentRoom && _itemLocations[i] % 1.0 <= 0.1)
                {
                    _itemLocations[i] = double.NaN;
                    Print("{0}: TAKEN.", _items[i]);
                }
            }
            if (_currentRoom == 0) _rooms[0, 3] = "6";
            goto getCommand;

        processSteal:       // 1420
            //1420 IFR=10ANDI(N)=.5ANDR$(10,4)<>"12"THENPRINT"AS YOU START TO SLIP IT INTO YOUR POCKET THE CLERK GIVES YOU    THE EVIL EYE, SO YOU PUT IT BACK.  (A GOOD DISTRACTION WOULD BE  EXTREMELY HELPFUL ABOUT NOW.)":GOTO80
            if (_currentRoom == 9 && _itemLocations[nounIndex] == .5 && _rooms[9, 3] != "12")
            {
                Print("AS YOU START TO SLIP IT INTO YOUR POCKET THE CLERK GIVES YOU THE EVIL EYE, SO YOU PUT IT BACK.  (A GOOD DISTRACTION WOULD BE  EXTREMELY HELPFUL ABOUT NOW.)");
                goto getCommand;
            }

            //1425 IFI(15)=.5ANDR=10ANDR$(10,4)="12"ANDN=15THENPRINT"YOU SUCCESSFULLY SHOPLIFTED THE GLUE.  THIS IS A CLASS X OFFENSE, YOU KNOW.":I(15)=0:GOTO80
            if (_itemLocations[14] == .5 && _currentRoom == 9 && _rooms[9, 3] == "12")
            {
                Print("YOU SUCCESSFULLY SHOPLIFTED THE GLUE.  THIS IS A CLASS X OFFENSE, YOU KNOW.");
                _itemLocations[14] = double.NaN;
                goto getCommand;
            }

            //1427 IFR=7ANDR$(R,4)="9"THENPRINT"THAT WAS AS EASY AS TAKING CANDY FROM A BABY.":R$(7,4)="10":I(9)=0:I$(9)="A WHAM-O FRISBEE":GOTO80
            if (_currentRoom == 6 && _rooms[_currentRoom, 3] == "9")
            {
                Print("THAT WAS AS EASY AS TAKING CANDY FROM A BABY.");
                _rooms[6, 3] = "10";
                _itemLocations[8] = double.NaN;
                _items[8] = "A WHAM-O FRISBEE";
                goto getCommand;
            }

            //1428 IFI(9)=0THENPRINT"YOU ALREADY HAVE THE FRISBEE":GOTO80
            if (double.IsNaN(_itemLocations[8]))
            {
                Print("YOU ALREADY HAVE THE FRISBEE");
                goto getCommand;
            }

            //1429 IFI(9)<>RTHENPRINT"IT'S NOT EVEN HERE.":GOTO80
            if (_itemLocations[8] != _currentRoom)
            {
                Print("IT'S NOT EVEN HERE.");
                goto getCommand;
            }
            //1430 PRINT"STEALING IS A FELONY.  YOUR PARENTS BROUGHT YOU UP BETTER THAN  THAT.":GOTO80
            Print("STEALING IS A FELONY.  YOUR PARENTS BROUGHT YOU UP BETTER THAN THAT.");
            goto getCommand;

        getGlue:            // 1435
            //1435 PRINT"MAYBE YOU SHOULD STEAL IT.":GOTO80
            Print("MAYBE YOU SHOULD STEAL IT.");
            goto getCommand;

        getHerring:         // 1450
            //1450 PRINT"YOU DON'T HAVE ENOUGH MONEY TO BUY THE "N$:GOTO80
            Print("YOU DON'T HAVE ENOUGH MONEY TO BUY THE {0}", noun);
            goto getCommand;

        processBuy:         // 1460
            //1460 IFR<>10ANDR<>11ANDR<>4THENPRINT"THERE'S NOTHING HERE TO BUY.":GOTO80
            if (_currentRoom != 9 && _currentRoom != 10 && _currentRoom != 3)
            {
                Print("THERE'S NOTHING HERE TO BUY.");
                goto getCommand;
            }

            //1469 IFN=32ANDR=4ANDI(20)<>0THENPRINT"YOU DON'T HAVE ANY MONEY TO BUY A BAUBLE.":GOTO80
            if (nounIndex == 31 && _currentRoom == 3 && !double.IsNaN(_itemLocations[19]))
            {
                Print("YOU DON'T HAVE ANY MONEY TO BUY A BAUBLE.");
                goto getCommand;
            }

            //1470 IFN=32ANDR<>4THENPRINT"THE BAUBLE MACHINE ISN'T EVEN HERE":GOTO80
            if (nounIndex == 31 && _currentRoom != 3)
            {
                Print("THE BAUBLE MACHINE ISN'T EVEN HERE");
                goto getCommand;
            }

            //1471 IFN=32ANDR=4ANDI(20)=0ANDI$(20)="AN OLD DIRTY DIME"THENPRINT"A COBALT JEWEL. SLIDES DOWN THE CHUTE, HITS THE TRAY AND LANDS  IN THE FLOOR.  MAYBE THIS WASN'T WORTH YOUR DIME.":I(20)=.5: I(12)=4:GOTO80
            if (nounIndex == 31 && _currentRoom == 3 && double.IsNaN(_itemLocations[19]) && _items[19] == "AN OLD DIRTY DIME")
            {
                Print("A COBALT JEWEL. SLIDES DOWN THE CHUTE, HITS THE TRAY AND LANDS IN THE FLOOR.  MAYBE THIS WASN'T WORTH YOUR DIME.");
                _itemLocations[19] = 0.5;
                _itemLocations[11] = 3;
                goto getCommand;
            }

            //1472 IFI(20)=0ANDI$(20)="A SHINY QUARTER"ANDR=11ANDN=30THENPRINT"YOU INSERT THE QUARTER IN THE SUN-DROP MACHINE AND PUSH THE SUN-DROP BUTTON.  YOU NOTE THE 'RETURN FOR DEPOSIT' INSCRIPTION ON  THE BOTTLE.":
            // I(30)=0:I(20)=.5:GOTO80
            if (double.IsNaN(_itemLocations[19]) && _items[19] == "A SHINY QUARTER" && _currentRoom == 10 && nounIndex == 29)
            {
                Print("YOU INSERT THE QUARTER IN THE SUN-DROP MACHINE AND PUSH THE SUN-DROP BUTTON.  YOU NOTE THE 'RETURN FOR DEPOSIT' INSCRIPTION ON  THE BOTTLE.");
                _itemLocations[29] = double.NaN;
                _itemLocations[19] = 0.5;
                goto getCommand;
            }

            //1473 IFR=10ANDI(15)=0ANDN=15THENPRINT"YOU ALREADY HAVE THE GLUE.":GOTO80
            if (_currentRoom == 9 && double.IsNaN(_itemLocations[14]) && nounIndex == 14)
            {
                Print("YOU ALREADY HAVE THE GLUE.");
                goto getCommand;
            }

            //1474 IFR=10ANDR$(R,4)<>"13"THENPRINT"THERE ISN'T A CLERK HERE TO HELP YOU.  WHY DON'T YOU JUST STEAL IT.":GOTO80
            if (_currentRoom == 9 && _rooms[_currentRoom, 3] != "13")
            {
                Print("THERE ISN'T A CLERK HERE TO HELP YOU.  WHY DON'T YOU JUST STEAL IT.");
                goto getCommand;
            }

            //1475 IFR=10ANDN=15ANDI(15)=.5ANDI(20)=0THENPRINT"WHAT LUCK.  THE GLUE COSTS EXACTLY TWENTY-FIVE CENTS.  THE CLERK RINGS UP YOUR PURCHASE, YOU GIVE THE CLERK YOUR QUARTER AND SHE TELLS YOU TO HAVE A NICE DAY.":I(20)=.5:I(15)=0:GOTO80
            if (_currentRoom == 9 && nounIndex == 14 && _itemLocations[14] == 0.5 && double.IsNaN(_itemLocations[19]))
            {
                Print("WHAT LUCK.  THE GLUE COSTS EXACTLY TWENTY-FIVE CENTS.  THE CLERK RINGS UP YOUR PURCHASE, YOU GIVE THE CLERK YOUR QUARTER AND SHE TELLS YOU TO HAVE A NICE DAY.");
                _itemLocations[19] = 0.5;
                _itemLocations[14] = double.NaN;
                goto getCommand;
            }

            //1480 IFR=10AND(N=21ORN=22)ANDI(20)=0THENPRINT"YOU ONLY HAVE A QUARTER WHICH ISN'T ENOUGH TO BUY THE "N$:GOTO80
            if (_currentRoom == 9 && (nounIndex == 20 || nounIndex == 21) && double.IsNaN(_itemLocations[19]))
            {
                Print("YOU ONLY HAVE A QUARTER WHICH ISN'T ENOUGH TO BUY THE {0}", noun);
                goto getCommand;
            }

            //1482 IFR=10THENIFI(20)<>0THENPRINT"YOU DON'T HAVE ANY MONEY.":GOTO80
            if (_currentRoom == 9)
            {
                if (!double.IsNaN(_itemLocations[19]))
                {
                    Print("YOU DON'T HAVE ANY MONEY.");
                    goto getCommand;
                }
            }

            //1484 IFR=10THENPRINT"YOU LOOK AND LOOK AND EVEN ASK THE CLERK TO HELP YOU BUT YOU    JUST CAN'T FIND AND "N$" IN THIS STORE.":GOTO80
            if (_currentRoom == 9)
            {
                Print("YOU LOOK AND LOOK AND EVEN ASK THE CLERK TO HELP YOU BUT YOU JUST CAN'T FIND AND {0} IN THIS STORE.", noun);
                goto getCommand;
            }

            //1486 IFR=10ANDJF=1THENPRINT"THE CLERK IS GONE TO GET THE PIGS.  THERE'S NO ONE TO HELP YOU  WITH YOUR PURCHASE. NO. NO. NO.  YOU MUST BE LOONY.":GOTO80
            if (_currentRoom == 9 && jailFlag == 1)
            {
                Print("THE CLERK IS GONE TO GET THE PIGS.  THERE'S NO ONE TO HELP YOU WITH YOUR PURCHASE. NO. NO. NO.  YOU MUST BE LOONY.");
                goto getCommand;
            }

            //1487 PRINT"THAT ISN'T FOR SALE HERE.":GOTO80
            Print("THAT ISN'T FOR SALE HERE.");
            goto getCommand;

        processGive:        // 1490
            //1490 IFN=6THENN=30:GOTO1490ELSEIFI(N)<>0THENPRINT"YOU CAN'T GIVE WHAT YOU DON'T HAVE":GOTO80
            if (nounIndex == 5)
            {
                nounIndex = 29;
                goto processGive;
            }
            else
            {
                if (!double.IsNaN(_itemLocations[nounIndex]))
                {
                    Print("YOU CAN'T GIVE WHAT YOU DON'T HAVE");
                    goto getCommand;
                }
            }

            //1491 IFN=30ANDR=10ANDI$(30)<>"BROKEN BOTTLE"ANDI(30)=0ANDR$(10,4)="13"THENPRINT"THE CLERK TAKES THE BOTTLE AND HAPPILY GIVES YOU A DIME.":I$(20)="AN OLD DIRTY DIME":I(30)=.8:I(20)=0:GOTO80
            if (nounIndex == 29 && _currentRoom == 9 && _items[29] != "BROKEN BOTTLE" && double.IsNaN(_itemLocations[29]) && _rooms[9, 3] == "13")
            {
                Print("THE CLERK TAKES THE BOTTLE AND HAPPILY GIVES YOU A DIME.");
                _items[19] = "AN OLD DIRTY DIME";
                _itemLocations[29] = .8;
                _itemLocations[19] = double.NaN;
                goto getCommand;
            }

            //1492 IFR=5ANDN=3THENPRINT"THE RAT GRACIOUSLY ACCEPTS YOUR GIFT. HE PROMISES TO NAME HIS FIRST BORN AFTER YOU (WHAT AN HONOR).":I(3)=.5:GOTO80
            if (_currentRoom == 4 && nounIndex == 2)
            {
                Print("THE RAT GRACIOUSLY ACCEPTS YOUR GIFT. HE PROMISES TO NAME HIS FIRST BORN AFTER YOU (WHAT AN HONOR).");
                _itemLocations[2] = .5;
                goto getCommand;
            }

            //1493 IFN=30ANDR$(10,4)="13"ANDR=10ANDI$(30)="BROKEN BOTTLE"THENPRINT"SORRY SIR, WE DON'T ACCEPT BROKEN BOTTLES FOR DEPOSIT, PERHAPS YOU SHOULD MEND IT.":GOTO80
            if (nounIndex == 29 && _rooms[9, 3] == "13" && _currentRoom == 9 && _items[29] == "BROKEN BOTTLE")
            {
                Print("SORRY SIR, WE DON'T ACCEPT BROKEN BOTTLES FOR DEPOSIT, PERHAPS YOU SHOULD MEND IT.");
                goto getCommand;
            }

            //1494IFR=11ANDN=3ANDR$(11,4)="15"THENPRINT"THE MAN TAKES THE BOLOGNA AND PUTS IT IN HIS MOUTH - IT IS GONE! - WHAT A PIG! HE REACHES UNDER HIS HAT AND HANDS YOU AN AMULET.  THE MAN DISAPPEARS INTO THE ALLEY.":R$(11,4)="0":I(N)=.5:I(26)=0:GOTO80
            if (_currentRoom == 10 && nounIndex == 2 && _rooms[10, 3] == "15")
            {
                Print("THE MAN TAKES THE BOLOGNA AND PUTS IT IN HIS MOUTH - IT IS GONE! - WHAT A PIG! HE REACHES UNDER HIS HAT AND HANDS YOU AN AMULET.  THE MAN DISAPPEARS INTO THE ALLEY.");
                _rooms[10, 3] = "0";
                _itemLocations[nounIndex] = .5;
                _itemLocations[25] = double.NaN;
                goto getCommand;
            }

            //1495 IFR=14ANDN=27ANDR$(R, 4)="14"THENPRINT"THE JAILER GRACIOUSLY TAKES THE BEER, DRINKS ALL SIX, AND PASSES OUT.":I(27)=.7:R$(R,4)="16":GOTO80
            if (_currentRoom == 13 && nounIndex == 26 && _rooms[_currentRoom, 3] == "14")
            {
                Print("THE JAILER GRACIOUSLY TAKES THE BEER, DRINKS ALL SIX, AND PASSES OUT.");
                _itemLocations[26] = .7;
                _rooms[_currentRoom, 3] = "16";
                goto getCommand;
            }

            //1496 IFN=30ANDI(30)ANDR=1THENPRINT"THIS CLERK DOESN'T WANT THE BOTTLE.":GOTO80
            if (nounIndex == 29 && double.IsNaN(_itemLocations[29]) && _currentRoom == 0)
            {
                Print("THIS CLERK DOESN'T WANT THE BOTTLE.");
                goto getCommand;
            }

            //1497 IFN=30ANDI(30)AND(R<>1ANDR<>10)THENPRINT"NO ONE HERE WANTS YOUR BOTTLE, JERK!":GOTO80
            if (nounIndex == 29 && double.IsNaN(_itemLocations[29]) && (_currentRoom != 0 && _currentRoom != 9))
            {
                Print("NO ONE HERE WANTS YOUR BOTTLE, JERK!");
                goto getCommand;
            }

            //1498 PRINT"UNFORTUNATELY, THERE'S NO ONE HERE THAT WANTS IT.":GOTO80
            Print("UNFORTUNATELY, THERE'S NO ONE HERE THAT WANTS IT.");
            goto getCommand;

        processDrink:       // 1500
            //1500 IFI(N)<>0THENPRINT"YOU DON'T EVEN HAVE IT.":GOTO80
            //1503 IF(N=6ORN=30)ANDI(30)=0ANDSF=0THENPRINT"AH!! HOW REFRESHING. AS YOU TURN UP THE BOTTLE, YOU NOTICE A    RATTLING SOUND FROM WITHIN.":SF=1:GOTO80
            //1504 IFN=30THENPRINT"ITS ALL GONE IDIOT, YOU SHOULD HAVE SAVED SOME FROM THE FIRST   TIME.":GOTO80
            //1505 IFN<>27THENPRINT"I DON'T THINK THE "N$" WOULD AGREE WITH YOU.":GOTO80
            //1510 PRINT"YOU DOWN ALL SIX BEERS AND PASS OUT.  YOU SURE COULDN'T HANDLE  YOUR ALCOHOL VERY WELL.  IT SEEMS ALCOHOL POISONING WAS THE     CAUSE OF DEATH.":PRINT"  *** YOU HAVE DIED ***":END 

        getKeys:            // 1530
            //1530 IFR=14ANDR$(R,4)="14"THENPRINT"THE JAILER BEATS YOU WITH HIS BILLY CLUB.":GOTO80
            //1535 IFR=14ANDR$(R,4)="16"THENPRINT"YOU REACH THROUGH THE BARS AND GRAB HIS KEYS (THIS IS ANOTHER FELONY, OF COURSE.)":I(29)=O:R$(R,4)="17":GOTO80
            //1540 IFI(29)=RTHENI(29)=0: PRINT"TAKEN.":GOTO80
            //1545 PRINT"THERE ARE NO KEYS HERE TO GET.":GOTO80

        processGlue:        // 1550
            //1550 IF(N=6ORN=30)ANDI$(N)="BROKEN BOTTLE"ANDI(N)=0ANDI(15)=0THENPRINT"YOU SUCCESSFULLY MENDED THE BROKEN BOTTLE.":I$(30)="MENDED BOTTLE": GOTO80
            //1555 PRINT"CONFUCIOUS SAYS: YOU CAN'T FIX WHAT YOU HAVEN'T BROKEN.":GOTO80

        processWear:        // 1600
            //1600 IFN=7ANDI(7)=0ANDF1=0THENPRINT"THE CLOTHES FIT NICELY.  YOU LOOK RATHER DAPPER.":F1=1:GOTO80
            //1602 IFI(N)<>0THENPRINT"YOU DON'T EVEN HAVE THAT.":GOTO80
            //1604 IFN=7ANDI(7)=0ANDF1=1THENPRINT"YOU ALREADY SEEM TO BE DRESSED UP.":GOTO80
            //1610 PRINT"YOU CAN'T WEAR THAT.":GOTO80

        processRemove:      // 1620
            //1620 IFN=7ANDI(7)=0ANDF1=1THENPRINT"TOO BAD.  THEY WERE SUCH A NICE FIT.":F1=0:GOTO80
            //1622 IFI(N)<>0THENPRINT"YOU DON'T EVEN HAVE IT.":GOTO80
            //1624 IFN=7ANDI(7)=0ANDF1=0THENPRINT"YOU DON'T EVEN HAVE THE CLOTHES ON.":GOTO80
            //1630 PRINT"YOU CAN'T REMOVE WHAT YOU DON'T HAVE ON.":GOTO80

        processTake:        // 355 - note: same as Get
        processKill:        // 1640
            //1640 IFN=34ANDR=5THENPRINT"YOU DON'T HAVE A RAT TRAP.":GOTO80
            //1642 IF(N=35ANDR=11)OR(N=36AND(R=1ORR=10))THENPRINT"THE "N$" IS A NINJA MASTER.  YOU ARE PROMPTLY DISPOSED OF.":PRINT"  *** YOU HAVE DIED ***":END 
            //1644 PRINT"YOU CAN'T DO THAT.":GOTO80

        processReturn:      // 1660
            //1660 IFI(N)<>0THENPRINT"YOU CAN'T RETURN WHAT YOU DON'T HAVE":GOTO80
            //1662 IFN=3OANDR=10ANDI$(30)<>"BROKEN BOTTLE"ANDI(30)=0THENPRINT"THE CLERK TAKES THE BOTTLE AND HAPPILY GIVES YOU A DIME.":I$(20)="AN OLD DIRTY DIME":I(30)=.8:I(20)=0:GOTO80
            //1664 IFN=3OANDR=10ANDI$(30)="BROKEN BOTTLE"THENPRINT"SORRY SIR, WE DON'T ACCEPT BROKEN BOTTLES FOR DEPOSIT, PERHAPS  YOU SHOULD MEND IT.":GOTO80
            //1666 IFR<>10THENPRINT "NOBODY WANTS THAT.":GOTO80
            //1668 PRINT"WHY WOULD YOU WANT TO RETURN THAT?": GOTO80

        getBottle:          // 1700
            //1700 IFR=5ANDI(30)=5ANDR$(5,4)="7"THENINPUT"'SUN-DROP' OR 'WINE' BOTTLE";W$:PRINT:GOTO1702
            if (_currentRoom == 4 && _itemLocations[29] == 4 && _rooms[4, 3] == "7")
            {
                Console.Write("'SUN-DROP' OR 'WINE' BOTTLE? ");
                bottleChoice = Console.ReadLine();
                Print();
                goto whichBottle;
            }
            //1701 GOTO1719 
            goto getUnambiguousBottle;

        whichBottle:        // 1702
            //1702 IFW$="SUN-DROP"THENI(30)=0:PRINT"TAKEN.":GOTO80
            if (bottleChoice == "SUN-DROP")
            {
                _itemLocations[29] = double.NaN;
                goto getCommand;
            }

        openWall:           // 1704
            //1704 IFW$="WINE"ANDR$(5,4)="7"THENPRINT"A PORTION OF THE SOUTH WALL SLIDES FROM VIEW.":R$(5,4)="8":GOTO80
            if (bottleChoice == "WINE" && _rooms[4, 3] == "7")
            {
                Print("A PORTION OF THE SOUTH WALL SLIDES FROM VIEW.");
                _rooms[4, 3] = "8";
                goto getCommand;
            }

            //1706 IFW$="WINE"ANDR$(5,4)="8"THENPRINT"THE WINE BOTTLE ISN'T HERE ANYMORE.":GOTO80
            if (bottleChoice == "WINE" && _rooms[4, 3] == "8")
            {
                Print("THE WINE BOTTLE ISN'T HERE ANYMORE.");
                goto getCommand;
            }

            //1708 PRINT"THAT WASN'T ONE OF THE CHOICES NOW, WAS IT?":GOTO80
            Print("THAT WASN'T ONE OF THE CHOICES NOW, WAS IT?");
            goto getCommand;

        getUnambiguousBottle:         // 1719
            //1719 IFR=5ANDR$(5,4)="8"ANDI(30)=RTHEN1722
            if (_currentRoom == 4 && _rooms[4, 3] == "8" && _itemLocations[29] == _currentRoom) goto getSunDrop;

            //1720 IFR=5THENW$="WINE":GOTO1704
            if (_currentRoom == 4)
            {
                bottleChoice = "WINE";
                goto openWall;
            }

        getSunDrop:         // 1722
            //1722 IFI(30)=RTHENI(30)=0:PRINT"TAKEN.":GOTO80
            if (_itemLocations[29] == _currentRoom)
            {
                _itemLocations[29] = double.NaN;
                Print("TAKEN.");
                goto getCommand;
            }

        processBreak:       // 1750
            //1750 IFI(30)=0AND(N=14ORN=6ORN=30)ANDI$(30)="SUN-DROP"THENPRINT"THE BOTTLE BREAKS IN HALF REVEALING AN ORANGE JEWEL, WHICH YOU  PROCEED TO TAKE.":I(17)=0:I$(30)="BROKEN BOTTLE":I(30)=0:IFN<>14THEN80ELSE1760
            if (double.IsNaN(_itemLocations[29]) && (nounIndex == 13 || nounIndex == 5 || nounIndex == 29) && _items[29] == "SUN-DROP")
            {
                Print("THE BOTTLE BREAKS IN HALF REVEALING AN ORANGE JEWEL, WHICH YOU  PROCEED TO TAKE.");
                _itemLocations[16] = double.NaN;
                _items[29] = "BROKEN BOTTLE";
                _itemLocations[29] = double.NaN;
                if (nounIndex != 13) goto getCommand; else goto breakAll;
            }

            //1751 IFN=14THEN1760
            if (nounIndex == 13) goto breakAll;

            //1754 IFI(N)<>0THENPRINT"YOU DON'T EVEN HAVE THE "N$".":GOTO80
            if (!double.IsNaN(_itemLocations[nounIndex]))
            {
                Print("YOU DON'T EVEN HAVE THE {0}.", noun);
                goto getCommand;
            }

            //1755 IFI(N)<>0THENPRINT"YOU DON'T EVEN HAVE THE "N$:GOTO80
            if (!double.IsNaN(_itemLocations[nounIndex]))
            {
                Print("YOU DON'T EVEN HAVE THE {0}", noun);
                goto getCommand;
            }
        
            //1756 PRINT"YOU HAVE DESTROYED IT. IT IS NOW COMPLETELY USELESS, SO YOU     THROW IT AWAY.":I(N)=.5:GOTO80
            Print("YOU HAVE DESTROYED IT. IT IS NOW COMPLETELY USELESS, SO YOU THROW IT AWAY.");
            _itemLocations[nounIndex] = 0.5;
            goto getCommand;

        breakAll:
            //1760 FORI=1TO33: IFI(I)=0THENI(I)=.5:PRINTN$(I)": BROKEN"
            //1765 NEXT 
            for (int i = 0; i < 33; i++)
            {
                if (double.IsNaN(_itemLocations[i]))
                {
                    _itemLocations[i] = 0.5;
                    Print("{0}: BROKEN", noun[i]);
                }
            }
            
            //1770 PRINT"ALL OF YOUR POSSESSIONS ARE NOW USELESS SO YOU THROW THEM ALL   AWAY.":GOTO80
            Print("ALL OF YOUR POSSESSIONS ARE NOW USELESS SO YOU THROW THEM ALL AWAY.");
            goto getCommand;

        winnerWinner:       // 29995

            goto describeRoom;
        }

        /// <summary>
        /// Initializes all the array data for rooms, item locations, status, etc;
        /// </summary>
        internal void Initialize()
        {
            _rooms[0,0]="  YOU ARE IN A HOTEL LOBBY.  THERE IS A NEWSPAPER STAND BESIDE AN OLD MUSTY COUCH ALONG THE WEST WALL.  THERE ARE TWO THROW PILLOWS ON THE COUCH.  OPPOSITE ALL THIS ON THE WEST WALL IS A RECEPTION DESK WITH A SMILING CLERK";
            _rooms[0,1]=" STANDING BEHIND IT. A FLIGHT OF STAIRS LEADS UP AND DOWN. THERE IS AN EXIT TO THE SOUTH AND THE STREET LIES TO THE EAST.";
            _rooms[1,0]="  THIS IS THE ROOM YOU WERE SUPPOSED TO SLEEP IN LAST NIGHT.  YOUR BED IS IN A SHAMBLES.  IT LOOKS LIKE THERE MIGHT HAVE BEEN A PARTY NEARBY LAST NIGHT. BY THE WAY, THERE IS A DOORWAY TO THE WEST AND STAIRS LEADING DOWN.";
            _rooms[2,0]="  NO WONDER YOU DIDN'T SLEEP IN YOUR ROOM LAST NIGHT.  NO ONE COULD HAVE LIVED THROUGH THE PARTY IN THIS ROOM MUCH LESS SLEPT THROUGH IT.  THERE ARE NUMEROUS PEOPLE LYING AROUND THE ROOM IN VARIOUS STATES OF UNDRESS AND";
            _rooms[2,1]=" CONSCIOUSNESS.  A BUSTED T.V. AND A DEMOLISHED MIRROR ARE AMONG THE RECOGNIZABLE REMAINS OF THIS  ROOM.";
            _rooms[3,0]="  THIS ROOM SIMPLY ASTOUNDS YOU.  THERE ARE LARGE TROPICAL PLANTS IN EVERY CORNER AND A BLUE HAZE FILLS THE AIR.  YOU BEGIN TO LAUGH AND YOU GET THE MUNCHIES.  A SINGLE EXIT LIES TO THE NORTH.  THERE IS A MACHINE HERE THAT CLOSELY RESEMBLES A";
            _rooms[3,1]=" BAUBLE MACHINE.";
            _rooms[4,0]="  THIS IS THE CELLAR. THERE ARE RACKS OF WINE LINING THE WALLS--IT'S A WINE CELLAR!  A MUSTY SMELL PERMEATES THE ROOM AND A SMALL RAT EYES YOU SUSPICIOUSLY.  A STEEP FLIGHT OF STAIRS LEADS UPWARDS.";
            _rooms[5,0]="  YOU EMERGE FROM THE HOLE INTO A VERY ANCIENT LOOKING ROOM.  BUT,THEN AGAIN, NOT MUCH COULD BE WORSE THAN THAT CELLAR.  SOME KIND OF EVIL GENIUS MUST HAVE USED THIS FOR HIS LAB.  THOUSANDS OF CHEMICALS LINE A SHELF THAT RUNS";
            _rooms[5,1]=" ALONG ONE WALL.  OBVIOUSLY, NO ONE HAS BEEN IN HERE FOR AGES AS AT LEAST TWO INCHES OF DUST IS ON THE FLOOR.  ALL SORTS OF CONTRAPTIONS THAT YOU CAN ONLY GUESS WHAT THEY DO LIE SCATTERED ON THE FLOOR.  OH WELL, THEY PROBABLY";
            _rooms[5,2]=" DIDN'T WORK ANYWAY.";
            _rooms[6,0]="  YOU ARE OUT ON THE STREET.  IN FACT, A SIGN SAYS YOU ARE 'DOWNTOWN'.  TO THE EAST, OPEN DOORS LEAD INTO AN ARCADE.  THE  STREET IS SPARSLEY POPULATED BUT THE FEW HERE ARE DRIVING LIKE IDIOTS. (RED LIGHTS HAVE NO";
            _rooms[6,1]=" MEANING TO THESE PEOPLE.) THERE ARE TWO CHILDREN ON THE OPPOSITE SIDEWALK.  YOUR HOTEL IS TO THE WEST.  THE STREET CONTINUES NORTH AND SOUTH.";
            _rooms[7,0]="  IT SEEMS YOU HAVE WANDERED INTO AN ARCADE.  ALL YOUR FAVORITE GAMES ARE HERE.  SOME INCLUDE: ALCON, XEVIOUS, PINBOT, IKARI WARRIORS, AND MANY MORE.  A PAY PHONE HANGS ON THE WALL.";
            _rooms[8,0]="  YOU ARE IN THE STREET.  A SIGN HERE SAYS 'UPTOWN'.  THERE IS A JIM DANDY MARKET IS TO THE EAST.  THE STREET CONTINUES TO THE NORTH AND SOUTH.  THERE ARE MANY CARS HERE.  YOU COUGH YOUR WAY THROUGH THE CARBON MONOXIDE.";
            _rooms[9,0]="  THIS JIM DANDY'S IS JUST LIKE ANY OTHER CONVENIENCE MARKET. IT HAS EVERYTHING YOU'D EVER NEED AT OUTRAGEOUS PRICES.  YOU NOTICE SOME ITEMS ARE ON SALE: RICE-A-RONI, 2 LITRE COKES, AND GLUE.";
            _rooms[10,0]="  YOU ARE STILL IN THE STREET.  ANOTHER SIGN HERE SAYS 'MOTOWN'.  JAMES BROWN MUSIC CAN BE HEARD FAINTLY IN THE BACKGROUND.  A SUN-DROP MACHINE IS IN FRONT OF THE LOCAL JAIL WHICH IS TO THE  EAST.  EXITS LIE IN ALL FOUR DIRECTIONS.";
            _rooms[10,1]=" A MANHOLE IS IN THE STREET.";
            _rooms[11,0]="  THIS IS THE FRONT OFFICE OF THE LOCAL POLICE STATION.  THE STREET IS TO THE WEST AND A DOOR LEADS NORTH.  THERE ARE SOME EMPTY JAIL CELLS IN THE BACK.";
            _rooms[12,0]="  YOU ARE IN A DARK ALLEY.  THE OPPRESSIVENESS OF THIS PLACE DROWNS OUT THE MUSIC TO A MERE WHISPER.  ALLEY CATS SQUALL AT YOU.  THIS DOESN'T LOOK GOOD, YOU'D BETTER GET OUT OF HERE QUICK BEFORE YOU BECOME CAT FOOD.";
            _rooms[13,0]="  YOU ARE IN A CELL. THE REPUGNANT SMELL OF THE TOILET OVERWHELMS YOU. THERE IS A LONE BENCH IN ONE CORNER AND RAUNCHY DRAWINGS (WITH CAPTIONS) ON THE WALL.";
            _rooms[14,0]="  THIS IS THE EVIDENCE ROOM.";
            _rooms[15,0]="  THIS IS THE TEMPLE OF ATTU-KOR.  YOU ASK YOURSELF WHY NO ONE HAS EVER DISCOVERED IT BEFORE.  AFTER ALL, IT'S ONLY UNDER THE STREET.  THE WALLS ARE OF SOLID GOLD AND THE FLOOR IS INLAID WITH DIAMONDS.  OTHER THAN THAT,";
            _rooms[15,1]=" THIS PLACE IS NO BIG DEAL.  A  LARGE THRONE IS AT ONE END.  THE ROOM IS ILLUMINATED BUT THERE IS NO APPARENT LIGHT SOURCE.";

            for (int i = 0; i < _itemLocations.Length; i++)
            {
                _itemLocations[i] = .5;
            }

            // locations: 
            // double.Nan = in your posession
            // .5 = hidden or unavailable (for instance the beer in the toilet or jewel in the bottle).
            // .8 = used for the sun-drop bottle when you give it to the clerk for a dime.  effectvely makes it hidden from the game.
            // x.1 - the item is avaialble and it a room (the integer part of the number), but hidden.
            // 0 or greater is the room number it's in.
            _items[0] = "BOOK"; _itemLocations[0] = 4;
            _items[1] = "CIGARETTE (BURNING)"; _itemLocations[1] = double.NaN;
            _items[2] = "A STICK OF BOLOGNA"; _itemLocations[2] = 1;
            _items[3] = "A NEWSPAPER";_itemLocations[3]=0.1;
            _items[4] = "A POSTCARD OF LA PAZ, BOLIVIA"; _itemLocations[4] = 0.1;
            _items[6] = "A HAT, GLASSES, AND A COAT (CLOTHES)";_itemLocations[6]=2;
            _items[7] = "AN INSTANT CAMERA";_itemLocations[7]=1;
            _items[10] = "A CLEAR JEWEL"; _itemLocations[10] = 5;
            _items[11] = "A COBALT JEWEL";
            _items[14] = "A BOTTLE OF GLUE";
            _items[16] = "AN ORANGE JEWEL";
            _items[18] = "A SPOOL OF STRING"; _itemLocations[18] = 38;
            _items[22] = "A RUSTY CROWBAR";_itemLocations[22]=14;
            _items[24] = "PLAYDOUGH-LIKE SUBSTANCE"; _itemLocations[24]=14;
            _items[25] = "GOLDEN AMULET";
            _items[26] = "A SIX-PACK OF COORS LIGHT";
            _items[28] = "SOME KEYS";
            _items[29] = "SUN-DROP";

            status[0] = " ";
            status[1] = "THE DOOR IS OPEN";
            status[2] = "THE DOOR IS CLOSED";
            status[3] = "THE STAND CONTAINS A NEWSPAPER AND A POSTCARD";
            status[4] = "THE STAND CONTAINS A NEWSPAPER";
            status[5] = "THE STAND CONTAINS A LONELY LITTLE POSTCARD";
            status[6] = "THE STAND HAS BEEN LOOTED.  IT IS BARE";
            status[7] = "A WINE RACK HANGS ON THE SOUTH WALL.  A LONLEY BOTTLE OF OLD WINE SITS IN THE RACK.";
            status[8] = "A LARGE HOLE LEADS SOUTH.";
            status[9] = "THE CHILDREN ARE THROWING A FRISBEE.  YOU CAN'T BE SURE FROM HERE, BUT THE FRISBEE APPEARS TO BE A WHAM-O. ";
            status[10] = "THE CHILDREN LOOK AS IF THEY JUST LOST THEIR FRISBEE.  THEY ARE CRYING.";
            status[11] = "YOU NOTICE WORKMEN CAREFULLY INSTALLING A NEW WINDOW AT JIM DANDY MARKET.  YOU WONDER WHAT MIGHT HAVE BROKEN IT.";
            status[12] = "SHARDS OF GLASS LIE EVERYWHERE.  THE WINDOW OF JIM DANDY ALSO SEEMS TO BE BROKEN. A SHREDDED FRISBEE LIES AMONG THE SHARDS.";
            status[13] = "THE CLERK IS SMILING AND ASKS IF SHE CAN BE OF ANY ASSISTANCE.";
            status[14] = "THE JAILER IS STANDING OUTSIDE THE CELL.  HIS KEYS HANG ON HIS BELT.";
            status[15] = "THERE IS AN OLD MAN HERE WITH A FUNNY HAT AND DIRTY CLOTHES.";
            status[16] = "THE JAILER IS SPRAWLED OUT DRUNK ON THE FLOOR.  HIS KEYS STILL HANG ON HIS BELT.";
            status[17] = "THERE IS A DRUNK JAILER HERE.";
            
            // Set room status.  Corresponds to entries in the status array.
            _rooms[0,3]="3";
            _rooms[1,3]="2";
            _rooms[2,3]="2";
            _rooms[3,3]="0";
            _rooms[4,3]="7";
            _rooms[6,3]="9";
            _rooms[8,3]="11";
            _rooms[9,3]="13";
            _rooms[10,3]="15";
            _rooms[13,3]="14";
            _rooms[11,3]="2";

            // Sets the door direction.  Corresponds to entries in the _shortcuts array.
            _rooms[1, 4] = "3";
            _rooms[2, 4] = "2";
            _rooms[4, 4] = "1";
            _rooms[11, 4] = "0";

            // Since arrays are now 0 based, had to replace 0, indicating 'not an exit' with 255.
            _exits[0]= Ascii.Chr(255)+Ascii.Chr(3)+Ascii.Chr(6)+Ascii.Chr(255)+Ascii.Chr(1)+Ascii.Chr(4);
            _exits[1]= Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(2)+Ascii.Chr(255)+Ascii.Chr(0);
            _exits[2]=Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(1)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255);
            _exits[3]=Ascii.Chr(0)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255);
            _exits[4]=Ascii.Chr(255)+Ascii.Chr(5)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(0)+Ascii.Chr(255);
            _exits[5]=Ascii.Chr(4)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255);
            _exits[6]=Ascii.Chr(8)+Ascii.Chr(255)+Ascii.Chr(7)+Ascii.Chr(0)+Ascii.Chr(255)+Ascii.Chr(255);
            _exits[7]=Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(6)+Ascii.Chr(255)+Ascii.Chr(255);
            _exits[8]=Ascii.Chr(10)+Ascii.Chr(6)+Ascii.Chr(9)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255);
            _exits[9]=Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(8)+Ascii.Chr(255)+Ascii.Chr(255);
            _exits[10]=Ascii.Chr(12)+Ascii.Chr(8)+Ascii.Chr(11)+Ascii.Chr(12)+Ascii.Chr(255)+Ascii.Chr(15);
            _exits[11]=Ascii.Chr(14)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(10)+Ascii.Chr(255)+Ascii.Chr(255);
            _exits[12]=Ascii.Chr(255)+Ascii.Chr(10)+Ascii.Chr(10)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255);
            _exits[13]=Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(11)+Ascii.Chr(255)+Ascii.Chr(255);
            _exits[14]=Ascii.Chr(255)+Ascii.Chr(11)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255);
            _exits[15]=Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255)+Ascii.Chr(255);

            _currentRoom = 0;
        }

        private void DisplayHelp()
        {
        }

        private void Print()
        {
            Console.WriteLine();
        }

        private void Print(string buffer, params object[] args)
        {
            Print(string.Format(buffer, args));
        }

        private void Print(string buffer)
        {
            while (buffer.Length > 0)
            {
                if (buffer.Length <= numberOfColumns)
                {
                    Console.WriteLine(buffer);
                    buffer = string.Empty;
                }
                else
                {
                    string nextChar = buffer.Substring(numberOfColumns, 1);
                    string fullLine = buffer.Substring(0, numberOfColumns);

                    if (nextChar == " ")
                    {
                        Console.WriteLine(fullLine);
                        buffer = buffer.Substring(numberOfColumns).Trim();
                    }
                    else
                    {
                        int lastSpacePosition = buffer.LastIndexOf(' ', numberOfColumns);
                        Console.WriteLine(buffer.Substring(0, lastSpacePosition));
                        buffer = buffer.Substring(lastSpacePosition).Trim();
                    }
                }
            }
        }
    }
}
