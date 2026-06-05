using static Tests.GameRunner;

namespace Tests;

/// <summary>
/// Golden-master tests over the current Game.Play() behavior. These exist to catch any accidental
/// change in puzzle outcomes or message text during the GameEngine.ProcessCommand extraction.
///
/// Each scenario was traced against the room/exit/item tables in Game.Initialize(). Movement uses the
/// single-letter shortcuts (N/S/E/W/U/D) and the parser's two-word "VERB NOUN" form.
/// </summary>
public class CharacterizationTests
{
    // ---- Opening state ----------------------------------------------------------------------

    [Fact]
    public void NewGame_OpensInTheHotelLobby()
    {
        string output = Run(); // no commands -> immediate EOF, just the opening render

        AssertSays(output, "YOU ARE IN A HOTEL LOBBY");
        AssertSays(output, "THE STAND CONTAINS A NEWSPAPER AND A POSTCARD");
        AssertSays(output, "THE ROOM CONTAINS:");
        AssertSays(output, "NOTHING");
    }

    [Fact]
    public void NewGame_InventoryHoldsTheBurningCigarette()
    {
        string output = Run("I");

        AssertSays(output, "YOU ARE CARRYING:");
        AssertSays(output, "CIGARETTE (BURNING)");
    }

    // ---- Parser error handling --------------------------------------------------------------

    [Fact]
    public void EmptyCommand_AsksToBegPardon()
    {
        AssertSays(Run(""), "BEG PARDON?");
    }

    [Fact]
    public void SingleUnknownToken_ReportsUnknownCommand()
    {
        AssertSays(Run("X"), "I DON'T KNOW THAT COMMAND");
    }

    [Fact]
    public void OneWord_AsksForTwoWords()
    {
        AssertSays(Run("HELLO"), "TWO WORDS PLEASE!!");
    }

    [Fact]
    public void UnknownVerb_IsReportedByName()
    {
        AssertSays(Run("FLING BOOK"), "I DON'T KNOW THE VERB FLING");
    }

    [Fact]
    public void UnknownNoun_IsReportedByName()
    {
        AssertSays(Run("GET WIDGET"), "I DON'T KNOW THE NOUN WIDGET");
    }

    [Fact]
    public void ReferringToAJewelGenerically_AsksForItsColor()
    {
        AssertSays(Run("GET JEWEL"), "PLEASE REFER TO A JEWEL BY ITS COLOR.");
    }

    // ---- Movement ---------------------------------------------------------------------------

    [Fact]
    public void GoingEastFromLobby_ReachesDowntownStreet()
    {
        AssertSays(Run("E"), "'DOWNTOWN'");
    }

    [Fact]
    public void WalkingEastThenNorthTwice_ReachesMotown()
    {
        // Lobby --E--> Downtown --N--> Uptown --N--> Motown
        AssertSays(Run("E", "N", "N"), "'MOTOWN'");
    }

    [Fact]
    public void MovingIntoANonExit_IsRejected()
    {
        // North is not an exit from the lobby.
        AssertSays(Run("N"), "THAT ISNT AN EXIT");
    }

    // ---- Picking things up ------------------------------------------------------------------

    [Fact]
    public void GettingTheBookInTheCellar_Succeeds()
    {
        // Lobby --D--> Cellar, where the book sits.
        string output = Run("D", "GET BOOK");

        AssertSays(output, "THIS IS THE CELLAR");
        AssertSays(output, "TAKEN.");
    }

    [Fact]
    public void GettingTheUnavailableAmulet_FallsThroughToCantDoThat()
    {
        // Characterization of a goto fall-through quirk: the amulet's "unavailable" location (0.5)
        // truncates to room 0, so GET fails its in-room check, slides through the DROP handler, and
        // ends at the generic refusal. Pinning this down so the extraction reproduces it exactly.
        AssertSays(Run("GET AMULET"), "YOU CAN'T DO THAT.");
    }

    // ---- A puzzle interaction: the manhole --------------------------------------------------

    [Fact]
    public void OpeningTheManholeBareHanded_Fails()
    {
        // You need the crowbar to pry the manhole; bare-handed in Motown it refuses.
        string output = Run("E", "N", "N", "OPEN MANHOLE");

        AssertSays(output, "YOU CAN'T OPEN THE MANHOLE WITH YOUR BARE HANDS.");
    }

    // ---- A multi-step command: PUT prompts for a destination --------------------------------

    [Fact]
    public void Put_PromptsForDestination_ThenResolvesAgainstIt()
    {
        // PUT issues a SECOND read for the destination. This two-step interaction is exactly what the
        // ProcessCommand extraction must preserve via an explicit "awaiting input" state.
        string output = Run("PUT BOOK", "STAND");

        AssertSays(output, "WHERE DO YOU WANT TO PUT THE BOOK");
        AssertSays(output, "THAT WON'T FIT IN THE STAND.");
    }

    // ---- A death path -----------------------------------------------------------------------

    [Fact]
    public void SmokingTheCigaretteThreeTimes_IsFatal()
    {
        string output = Run("SMOKE CIGARETTE", "SMOKE CIGARETTE", "SMOKE CIGARETTE");

        AssertSays(output, "WARNING: THE SURGEON GENERAL");
        AssertSays(output, "*** YOU HAVE DIED ***");
    }
}
