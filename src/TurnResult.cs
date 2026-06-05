namespace Game
{
    /// <summary>
    /// The result of one engine turn (<see cref="Game.Start"/> or <see cref="Game.ProcessCommand"/>).
    /// Carries the unwrapped logical text to display, the room it pertains to (which graphic a future
    /// GUI would render), and whether the engine is mid-command awaiting more input or the session has
    /// ended. The UI renders <see cref="Text"/>; the engine never touches the console.
    /// </summary>
    public sealed class TurnResult
    {
        /// <summary>Unwrapped text produced this turn. The UI is responsible for any word-wrapping.</summary>
        public string Text { get; }

        /// <summary>The room this turn pertains to — i.e. which room graphic a GUI should show.</summary>
        public int Room { get; }

        /// <summary>
        /// True when the engine suspended mid-command (the PUT destination or SUN-DROP/WINE bottle
        /// prompt) and the next <see cref="Game.ProcessCommand"/> call supplies the answer. The UI
        /// should read another line without emitting its normal command prompt.
        /// </summary>
        public bool AwaitingInput { get; }

        /// <summary>True when the session has ended (death or victory); no further input is expected.</summary>
        public bool GameOver { get; }

        public TurnResult(string text, int room, bool awaitingInput, bool gameOver)
        {
            Text = text;
            Room = room;
            AwaitingInput = awaitingInput;
            GameOver = gameOver;
        }
    }
}
