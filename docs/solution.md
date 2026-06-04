# The Jewels of Attu-Kor -- Complete Solution

> **Verified.** The walkthrough below is exercised end-to-end by an automated test
> (`tests/WalkthroughTests.cs` -> `WinningWalkthrough_ReachesTheVictoryEnding`) and really does reach the
> 400/400 victory ending in the current code. An earlier version of this document described a plausible
> but **untested** solution that does **not** actually win; see *Engine Quirks* at the bottom for the
> three places it went wrong.

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
| Book | Room 4 (Cellar) | Optional -- READ it for the legend clue |
| Clear Jewel | Room 5 (Lab) | Optional **red herring** -- doesn't fit the amulet |
| Frisbee | Room 6 (kids have it) | Throw through Jim Dandy's window as distraction |
| Quarter | Room 7 (in pay phone) | EXAMINE PHONE to find it; buys Sun-Drop |
| Glue | Room 9 (Jim Dandy, $0.25) | Must STEAL after breaking window; mends bottle |
| Crowbar | Room 14 (Evidence Room) | Opens manhole cover (alternate route to the Temple) |
| Beer | Room 13 (hidden in toilet tank) | Give to jailer to knock him out |
| Keys | Room 13 (on jailer's belt) | Needed to leave the cell |
| Amulet | Old man in Motown | Holds the two jewels -- the win condition |
| Sun-Drop bottle | Motown machine ($0.25) | Contains the **Orange Jewel** inside |
| **Cobalt Jewel** | Bauble machine (Room 3, costs a dime) | One of two jewels for the amulet |
| **Orange Jewel** | Inside the Sun-Drop bottle | One of two jewels for the amulet |

## Win Condition

Put both the **Cobalt** and **Orange** jewels into the **Amulet**, then go **Down** from Motown (Room 10).
The game checks `amuletFlag == 3` before anything else and sends you straight to the victory ending at the
Temple of Attu-Kor. Score: 400/400, rank of Supreme Archaeologist.

## Death Traps to Avoid

- `SMOKE CIGARETTE` 3 times -- lung cancer
- `DRINK BEER` -- alcohol poisoning (all six at once)
- `KILL MAN` or `KILL CLERK` -- they're ninja masters

## Complete Walkthrough (verified winning path)

The whole puzzle is one long chain, and a few steps are order-sensitive (see *Engine Quirks*). The exact
command sequence below is the one the automated test plays.

### Phase 1: Hotel -- collect the bologna and the clothes
1. **Room 0** (Hotel Lobby) -- start
2. `U` -- Room 1 (Your Room)
3. `OPEN DOOR` -- opens the door west to the party room
4. `GET BOLOGNA`
5. `W` -- Room 2 (Party Room)
6. `GET CLOTHES`
7. `E` -- Room 1, `D` -- Room 0

### Phase 2: Streets -- quarter, frisbee, amulet, sun-drop
8. `E` -- Room 6 (Downtown)
9. `GET FRISBEE` -- steal it from the kids
10. `E` -- Room 7 (Arcade): `EXAMINE PHONE` -- find a quarter
11. `W` -- Room 6, `N` -- Room 8 (Uptown), `N` -- Room 10 (Motown)
12. `GIVE BOLOGNA` -- the old man eats it and hands you the **Golden Amulet**
13. `BUY SUN-DROP` -- the quarter buys a Sun-Drop bottle (the Orange Jewel is sealed inside)
14. `S` -- Room 8 (Uptown)

### Phase 3: The heist -- enter the store FIRST, then make your distraction
> Order matters here -- see *Engine Quirks #1*. You must be **inside** the store when you throw.
15. `E` -- Room 9 (Jim Dandy, clerk present)
16. `THROW FRISBEE` -- smashes the window from inside; the clerk leaves to get the police (`jailFlag = 1`)
17. `STEAL GLUE` -- "YOU SUCCESSFULLY SHOPLIFTED THE GLUE"
18. `W` -- Room 8 (you arrive safely; the arrest only fires on your *next* move out of Uptown)
19. `N` -- **ARRESTED!** The S.W.A.T. team hauls you to jail. Everything except your clothes is
    confiscated to Room 14 (Evidence Room). You're now in Room 13 (Cell).

### Phase 4: Jailbreak
20. `EXAMINE TOILET` -- "YOU CHECK OUT THE TANK AND FIND A SIX-PACK OF COORS LIGHT"
21. `GET BEER`
22. `GIVE BEER` -- the jailer drinks all six and passes out
23. `GET KEYS` -- grab them through the bars (required to free the cell exit -- see *Engine Quirks #3*)
24. `W` -- Room 11 (Police Station front office)

### Phase 5: Evidence Room -- recover everything
25. `N` -- Room 14 (Evidence Room)
26. `GET ALL` -- recover all confiscated items (amulet, sun-drop, glue, ...) plus the crowbar and
    substance already stored here

### Phase 6: Orange Jewel -- break, then mend the bottle
27. `BREAK BOTTLE` -- "THE BOTTLE BREAKS IN HALF REVEALING AN **ORANGE JEWEL**" (you take the jewel; the
    bottle becomes a "BROKEN BOTTLE")
28. `GLUE SUN-DROP` -- "YOU SUCCESSFULLY MENDED THE BROKEN BOTTLE"
    > Use the noun **SUN-DROP**, not BOTTLE -- see *Engine Quirks #2*.

### Phase 7: Disguise, then return the bottle for a dime
29. `WEAR CLOTHES` -- changes your appearance so the clerk won't recognize the vandal
30. `S` -- Room 11, `W` -- Room 10, `S` -- Room 8, `E` -- Room 9 (not booted, thanks to the disguise)
31. `RETURN SUN-DROP` -- "THE CLERK TAKES THE BOTTLE AND HAPPILY GIVES YOU A **DIME**"
    > Again the noun is **SUN-DROP**, not BOTTLE.

### Phase 8: Buy the Cobalt Jewel
32. `W` -- Room 8, `S` -- Room 6, `W` -- Room 0, `S` -- Room 3 (Tropical Room)
33. `BUY BAUBLE` -- "A **COBALT JEWEL** SLIDES DOWN THE CHUTE" (it lands on the floor)
34. `GET COBALT`

### Phase 9: Victory!
35. `N` -- Room 0, `E` -- Room 6, `N` -- Room 8, `N` -- Room 10 (Motown)
36. `PUT COBALT` -- when asked where, answer `AMULET` -- "DONE." (`amuletFlag = 1`)
37. `PUT ORANGE` -- when asked where, answer `AMULET` -- "DONE." (`amuletFlag = 3`)
38. `D` -- **YOU WIN!**

> "A BLUE FLAME LEAPS FROM THE AMULET TO THE THRONE AND YOU ARE FILLED WITH A MYSTICAL POWER... THE INCAN
> GOD LINUS APPEARS. HE GIVES YOU A LAUREL WREATH..."
>
> **SCORE: 400 OUT OF 400. RANK: SUPREME ARCHAEOLOGIST.**

## The Puzzle Chain

The key insight is the intricate **bottle economy**: the quarter buys Sun-Drop, the bottle hides the
orange jewel, breaking it requires nothing but mending it back requires glue (which requires the frisbee
heist & jail sequence), then the mended bottle gets returned for a dime, and the dime buys the cobalt
jewel from the bauble machine. Quite the puzzle chain for a 1987 high school BASIC project!

## Engine Quirks (why the earlier walkthrough didn't win)

These are behaviors of the current C# port (faithfully carried over from, or introduced while porting, the
original BASIC). They are pinned down by tests so they survive refactoring; they are documented here, not
"fixed," unless we deliberately decide to change the game.

1. **Throw the frisbee from *inside* the store.** Once `jailFlag` is set, the arrest fires on the *first*
   move out of Uptown (Room 8). The earlier walkthrough threw the frisbee from Uptown and then stepped
   east into the store -- but that eastward step *is* a move out of Uptown, so you get arrested before you
   can steal the glue. Enter the store first, throw from inside, steal, then leave.
2. **Mend/return the bottle with the noun `SUN-DROP`, not `BOTTLE`.** The parser maps `BOTTLE` to a
   different (empty) item slot than the sun-drop bottle, so `GLUE BOTTLE` and `RETURN BOTTLE` silently
   fail. `GLUE SUN-DROP` and `RETURN SUN-DROP` work.
3. **Keys and the evidence-room door are mostly cosmetic.** `GET KEYS` files the keys into Room 0 (the
   lobby) instead of your inventory -- you can literally see them lying in the lobby later -- so the
   "unlock the cell, drag the jailer in" flavor text never prints; you simply walk west to the front
   office. Likewise the locked evidence-room door is bypassed: you can walk north into Room 14 without it
   opening. You still need to `GET KEYS` (it changes the cell's exit state enough to let you move) and you
   do *not* need to `OPEN DOOR`.
