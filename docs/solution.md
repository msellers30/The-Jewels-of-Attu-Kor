# The Jewels of Attu-Kor -- Complete Solution

## World Map

```
                    [12] Dark Alley
                     |  \
[14] Evidence Room   |   |
  |                  |   |
[11] Police Station--[10] Motown---...manhole down...---[15] TEMPLE
  |  (door, locked)   |
                      |
                    [8] Uptown---[9] Jim Dandy Market
                      |
[7] Arcade----------[6] Downtown---[0] Hotel Lobby---[3] Tropical Room
                                     |        |         (bauble machine)
                                  [1] Room--[2] Party Room
                                  (door)
                                     |
                                  [4] Wine Cellar
                                     |  (hidden passage behind wine bottle)
                                  [5] Ancient Lab
```

## Key Items & Where They Are

| Item | Location | Purpose |
|---|---|---|
| Cigarette (burning) | Inventory (start) | Red herring (3 smokes = death!) |
| Bologna | Room 1 (Hotel Room) | Trade to old man for the **Amulet** |
| Clothes | Room 2 (Party Room) | Disguise to re-enter Jim Dandy after jail |
| Book | Room 4 (Cellar) | READ it for the legend clue |
| Clear Jewel | Room 5 (Lab) | **Red herring** -- doesn't fit the amulet |
| Frisbee | Room 6 (kids have it) | Throw through Jim Dandy's window as distraction |
| Quarter | Room 7 (in pay phone) | EXAMINE PHONE to find it; buys Sun-Drop |
| Glue | Room 9 (Jim Dandy, $0.25) | Must STEAL after breaking window; mends bottle |
| Crowbar | Room 14 (Evidence Room) | Opens manhole cover |
| Beer | Room 13 (hidden in toilet tank) | Give to jailer to knock him out |
| Keys | Room 13 (on jailer's belt) | Escape jail & open evidence room |
| Amulet | Old man in Motown | Holds the two jewels -- the win condition |
| Sun-Drop bottle | Motown machine ($0.25) | Contains the **Orange Jewel** inside |
| **Cobalt Jewel** | Bauble machine (Room 3, costs a dime) | One of two jewels for the amulet |
| **Orange Jewel** | Inside the Sun-Drop bottle | One of two jewels for the amulet |

## Win Condition

Put both the **Cobalt** and **Orange** jewels into the **Amulet**, then go **Down** from Motown (Room 10). The game checks `amuletFlag == 3` before anything else and sends you straight to the victory ending at the Temple of Attu-Kor. Score: 400/400, rank of Supreme Archaeologist.

## Death Traps to Avoid

- `SMOKE CIGARETTE` 3 times -- lung cancer
- `DRINK BEER` -- alcohol poisoning (all six at once)
- `KILL MAN` or `KILL CLERK` -- they're ninja masters

## Complete Walkthrough

### Phase 1: Hotel Area
1. **Room 0** (Hotel Lobby): `GET ALL` -- takes newspaper & postcard from stand
2. `U` -- Room 1 (Your Room)
3. `OPEN DOOR` -- opens the door west to the party room
4. `GET BOLOGNA`, `GET CAMERA`
5. `W` -- Room 2 (Party Room): `GET CLOTHES`
6. `E` -- Room 1, `D` -- Room 0

### Phase 2: Cellar & Secret Lab
7. `D` -- Room 4 (Wine Cellar): `GET BOOK`
8. `PULL BOTTLE` -- "A PORTION OF THE SOUTH WALL SLIDES FROM VIEW" (reveals the hidden passage)
9. `S` -- Room 5 (Ancient Lab): `GET CLEAR` (red herring, but why not)
10. `N` -- Room 4, `U` -- Room 0

### Phase 3: Street - Get Quarter, Frisbee, Amulet, Sun-Drop
11. `E` -- Room 6 (Downtown): `GET FRISBEE` -- steal it from the kids
12. `E` -- Room 7 (Arcade): `EXAMINE PHONE` -- find a quarter
13. `W` -- Room 6, `N` -- Room 8 (Uptown), `N` -- Room 10 (Motown)
14. `GIVE BOLOGNA` -- the old man eats it and gives you the **Golden Amulet**
15. `BUY SUN-DROP` -- uses the quarter, you get a Sun-Drop bottle
16. `DRINK SUN-DROP` -- "YOU NOTICE A RATTLING SOUND FROM WITHIN"

### Phase 4: The Heist -- Steal Glue, Get Arrested
17. `S` -- Room 8: `THROW FRISBEE` -- smashes the Jim Dandy window; the clerk leaves to get the police (`jailFlag = 1`)
18. `E` -- Room 9 (Jim Dandy, first visit -- clerk is gone, your appearance is recorded)
19. `STEAL GLUE` -- "YOU SUCCESSFULLY SHOPLIFTED THE GLUE"
20. `W` -- Room 8 (arrive safely)
21. Any direction from Room 8 -- **ARRESTED!** S.W.A.T. takes you to jail. All items confiscated to Room 14 (Evidence Room). You're now in Room 13 (Jail Cell).

### Phase 5: Jailbreak
22. `EXAMINE TOILET` -- "YOU CHECK OUT THE TANK AND FIND A SIX-PACK OF COORS LIGHT" (beer appears in cell)
23. `GET BEER`
24. `GIVE BEER` -- jailer drinks all six and passes out
25. `GET KEYS` -- reach through bars and grab them
26. `W` -- "YOU UNLOCK THE CELL, DRAG THE JAILER IN, AND LOCK HIM UP." You're now in Room 11 (Police Station front office)

### Phase 6: Evidence Room -- Recover Everything
27. `OPEN DOOR` -- uses stolen keys to open the evidence room door
28. `N` -- Room 14 (Evidence Room)
29. `GET ALL` -- recover all confiscated items plus the crowbar and substance that were already here

### Phase 7: Orange Jewel -- Break & Mend the Bottle
30. `BREAK BOTTLE` -- "THE BOTTLE BREAKS IN HALF REVEALING AN **ORANGE JEWEL**" (you take the jewel; bottle becomes "BROKEN BOTTLE")
31. `GLUE BOTTLE` -- "YOU SUCCESSFULLY MENDED THE BROKEN BOTTLE" (now a "MENDED BOTTLE")

### Phase 8: Return Bottle for Dime, Buy Cobalt Jewel
32. `WEAR CLOTHES` -- change your appearance so the clerk doesn't recognize you (disguise was recorded without clothes; now you're wearing them)
33. `S` -- Room 11, `W` -- Room 10, `S` -- Room 8, `E` -- Room 9 (Jim Dandy -- not booted because your appearance changed!)
34. `RETURN BOTTLE` -- "THE CLERK TAKES THE BOTTLE AND HAPPILY GIVES YOU A **DIME**"
35. `W` -- Room 8, `S` -- Room 6, `W` -- Room 0, `S` -- Room 3 (Tropical Room)
36. `BUY BAUBLE` -- "A **COBALT JEWEL** SLIDES DOWN THE CHUTE" (appears on floor)
37. `GET COBALT`

### Phase 9: Victory!
38. `N` -- Room 0, `E` -- Room 6, `N` -- Room 8, `N` -- Room 10 (Motown)
39. `PUT COBALT` -- where? `AMULET` -- "DONE." (amuletFlag = 1)
40. `PUT ORANGE` -- where? `AMULET` -- "DONE." (amuletFlag = 3)
41. `D` -- **YOU WIN!**

> "A BLUE FLAME LEAPS FROM THE AMULET TO THE THRONE AND YOU ARE FILLED WITH A MYSTICAL POWER... THE INCAN GOD LINUS APPEARS. HE GIVES YOU A LAUREL WREATH..."
>
> **SCORE: 400 OUT OF 400. RANK: SUPREME ARCHAEOLOGIST.**

## The Puzzle Chain

The key insight is the intricate **bottle economy**: the quarter buys Sun-Drop, the bottle hides the orange jewel, breaking it requires glue (which requires the frisbee heist & jail sequence), then the mended bottle gets returned for a dime, and the dime buys the cobalt jewel from the bauble machine. Quite the puzzle chain for a 1987 high school BASIC project!
