namespace Game
{
    /// <summary>
    /// Console front-end for the <see cref="Game"/> engine. Owns everything that used to live inside
    /// the old Game.Play() loop but is really a UI concern: the read/prompt cycle, EOF handling, and
    /// the word-wrapping to the terminal width (the engine now returns unwrapped logical text). A
    /// future GUI would be a sibling of this class, rendering <see cref="TurnResult.Room"/> as graphics.
    /// </summary>
    internal sealed class ConsoleUi
    {
        private readonly int _columns = Console.IsOutputRedirected ? 80 : Console.WindowWidth;

        /// <summary>
        /// Drives a full session: render the opening room, then loop reading a line and feeding it to
        /// the engine until the game ends or input is exhausted (EOF), which ends the session — the
        /// same "quit" semantics the characterization tests rely on.
        /// </summary>
        internal void Run(Game game)
        {
            TurnResult result = game.Start();

            while (true)
            {
                Write(result.Text);
                if (result.GameOver) break;

                // A normal turn gets the "? " prompt; a suspended turn (PUT/bottle) already emitted its
                // own question, so we just read the answer.
                if (!result.AwaitingInput)
                {
                    Console.WriteLine();
                    Console.Write("? ");
                }

                string? line = Console.ReadLine();
                if (line == null) break;    // EOF: end the session cleanly.

                result = game.ProcessCommand(line);
            }
        }

        /// <summary>Writes a turn's text, word-wrapping each logical line to the console width.</summary>
        private void Write(string text)
        {
            string[] lines = text.Split('\n');

            // AppendLine always leaves a trailing newline, which yields one empty tail element here;
            // drop it so we don't emit an extra blank line.
            int count = lines.Length;
            if (count > 0 && lines[count - 1].Length == 0) count--;

            for (int i = 0; i < count; i++)
            {
                WriteWrapped(lines[i].TrimEnd('\r'));
            }
        }

        /// <summary>
        /// Word-wraps a single logical line to <see cref="_columns"/>. This is the wrapping logic moved
        /// verbatim out of the old engine Print(string) so console output looks exactly as it did.
        /// </summary>
        private void WriteWrapped(string buffer)
        {
            if (buffer.Length == 0)
            {
                Console.WriteLine();
                return;
            }

            while (buffer.Length > 0)
            {
                if (buffer.Length <= _columns)
                {
                    Console.WriteLine(buffer);
                    buffer = string.Empty;
                }
                else
                {
                    string nextChar = buffer.Substring(_columns, 1);
                    string fullLine = buffer.Substring(0, _columns);

                    if (nextChar == " ")
                    {
                        Console.WriteLine(fullLine);
                        buffer = buffer.Substring(_columns).Trim();
                    }
                    else
                    {
                        int lastSpacePosition = buffer.LastIndexOf(' ', _columns);
                        Console.WriteLine(buffer.Substring(0, lastSpacePosition));
                        buffer = buffer.Substring(lastSpacePosition).Trim();
                    }
                }
            }
        }
    }
}
