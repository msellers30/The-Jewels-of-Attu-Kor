using static Tests.GameRunner;

namespace Tests;

/// <summary>
/// End-to-end golden-master tests: the full winning path and the remaining death traps called out in
/// docs/solution.md. The winning sequence here is the one that ACTUALLY wins in the current code, which
/// differs from the documented walkthrough in three places (see WinningWalkthrough below). Locking this
/// down means the upcoming GameEngine extraction has to keep the game winnable along the same chain.
/// </summary>
public class WalkthroughTests
{
    /// <summary>
    /// The verified winning command sequence. Differences from docs/solution.md, which does NOT win as
    /// written:
    ///   1. The frisbee must be thrown from INSIDE the store: the arrest fires on the first move out of
    ///      Uptown once jailFlag is set, so throwing from Uptown then stepping east gets you jailed
    ///      before you can steal the glue.
    ///   2. The bottle is mended/returned with the SUN-DROP noun, not BOTTLE (they map to different item
    ///      slots; BOTTLE silently fails).
    ///   3. The keys/door steps are effectively no-ops (keys land in room 0, the locked door is bypassed),
    ///      so the cell-unlock and door-open flavor text never appears.
    /// </summary>
    private static readonly string[] WinningWalkthrough =
    {
        // Hotel: bologna (for the amulet) + clothes (for the later disguise)
        "U", "OPEN DOOR", "GET BOLOGNA", "W", "GET CLOTHES", "E", "D",
        // Quarter + frisbee, then Motown for the amulet and the sun-drop bottle
        "E", "GET FRISBEE", "E", "EXAMINE PHONE", "W", "N", "N",
        "GIVE BOLOGNA", "BUY SUN-DROP", "S",
        // Heist: enter the store first, throw from inside, steal, leave
        "E", "THROW FRISBEE", "STEAL GLUE", "W", "N",
        // Jailbreak
        "EXAMINE TOILET", "GET BEER", "GIVE BEER", "GET KEYS", "W",
        // Evidence room: recover everything
        "N", "GET ALL",
        // Orange jewel: break the bottle for the jewel, then mend it
        "BREAK BOTTLE", "GLUE SUN-DROP",
        // Disguise on; return the mended bottle for a dime
        "WEAR CLOTHES", "S", "W", "S", "E", "RETURN SUN-DROP",
        // Buy the cobalt jewel with the dime
        "W", "S", "W", "S", "BUY BAUBLE", "GET COBALT",
        // Seat both jewels in the amulet, then descend to the Temple
        "N", "E", "N", "N", "PUT COBALT", "AMULET", "PUT ORANGE", "AMULET", "D",
    };

    [Fact]
    public void WinningWalkthrough_ReachesTheVictoryEnding()
    {
        string output = Run(WinningWalkthrough);

        // The full puzzle chain, in order.
        AssertSays(output, "HE REACHES UNDER HIS HAT AND HANDS YOU AN AMULET");      // amulet
        AssertSays(output, "YOU SUCCESSFULLY SHOPLIFTED THE GLUE");                  // glue heist
        AssertSays(output, "THE S.W.A.T. TEAM COMES AND CARRIES YOU TO THE LOCAL JAIL"); // arrest
        AssertSays(output, "THE JAILER GRACIOUSLY TAKES THE BEER");                  // jailbreak
        AssertSays(output, "THE BOTTLE BREAKS IN HALF REVEALING AN ORANGE JEWEL");   // orange jewel
        AssertSays(output, "YOU SUCCESSFULLY MENDED THE BROKEN BOTTLE");             // mend
        AssertSays(output, "THE CLERK TAKES THE BOTTLE AND HAPPILY GIVES YOU A DIME"); // dime
        AssertSays(output, "A COBALT JEWEL. SLIDES DOWN THE CHUTE");                 // cobalt jewel

        // The victory ending.
        AssertSays(output, "A BLUE FLAME LEAPS FROM THE AMULET TO THE THRONE");
        AssertSays(output, "YOU SCORE IS 400 OUT OF A POSSIBLE 400");
        AssertSays(output, "YOU HAVE ACHIEVED THE RANK OF SUPREME ARCHAEOLOGIST");
    }

    [Fact]
    public void DrinkingAllSixBeers_IsFatal()
    {
        // Get arrested (frisbee heist) to reach the cell, recover the beer from the toilet tank, drink it.
        string output = Run(
            "E", "GET FRISBEE", "N", "THROW FRISBEE", "N", // -> arrested into the cell
            "EXAMINE TOILET", "GET BEER", "DRINK BEER");

        AssertSays(output, "IT SEEMS ALCOHOL POISONING WAS THE CAUSE OF DEATH");
        AssertSays(output, "*** YOU HAVE DIED ***");
    }

    [Fact]
    public void AttackingTheClerk_IsFatal()
    {
        // The clerk in the lobby is a "ninja master".
        string output = Run("KILL CLERK");

        AssertSays(output, "IS A NINJA MASTER. YOU ARE PROMPTLY DISPOSED OF");
        AssertSays(output, "YOU HAVE DIED");
    }
}
