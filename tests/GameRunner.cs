using GameClass = Game.Game;

namespace Tests;

/// <summary>
/// Characterization (golden-master) harness for the legacy <see cref="GameClass.Play"/> loop.
///
/// The game reads from <see cref="Console.In"/> and writes to <see cref="Console.Out"/>, so we
/// drive it by scripting stdin and capturing stdout. The point of these tests is to PIN DOWN the
/// current behavior so the upcoming extraction into a GameEngine.ProcessCommand model can be proven
/// to preserve every puzzle outcome.
///
/// Assertions go through <see cref="Normalize"/>: the engine currently word-wraps output to a column
/// width, but that wrapping is slated to move to the UI layer. Matching on whitespace-normalized text
/// means these tests assert on what the game SAYS, not how it happens to be laid out today, so they
/// survive the refactor.
/// </summary>
public static class GameRunner
{
    /// <summary>
    /// Runs one full session, feeding <paramref name="commands"/> as successive input lines, and
    /// returns everything the game printed. The session ends when input is exhausted (EOF), which
    /// <see cref="GameClass.Play"/> treats as "quit".
    /// </summary>
    public static string Run(params string[] commands)
    {
        string input = string.Concat(commands.Select(c => c + Environment.NewLine));

        TextReader originalIn = Console.In;
        TextWriter originalOut = Console.Out;
        try
        {
            var output = new StringWriter();
            Console.SetIn(new StringReader(input));
            Console.SetOut(output);

            new GameClass().Play();

            return output.ToString();
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    /// Collapses every run of whitespace (spaces, tabs, and the engine's word-wrap newlines) down to
    /// a single space, so assertions compare logical text rather than layout.
    /// </summary>
    public static string Normalize(string text) =>
        string.Join(' ', text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

    /// <summary>
    /// Asserts that the (normalized) game output contains the given (normalized) phrase.
    /// </summary>
    public static void AssertSays(string output, string expectedPhrase) =>
        Assert.Contains(Normalize(expectedPhrase), Normalize(output));

    /// <summary>
    /// Asserts that the (normalized) game output does NOT contain the given (normalized) phrase.
    /// </summary>
    public static void AssertDoesNotSay(string output, string phrase) =>
        Assert.DoesNotContain(Normalize(phrase), Normalize(output));
}
