namespace Game
{
    /// <summary>
    /// All mutable world and session data for one playthrough, split out of the engine in Phase 2b.
    /// The engine (<see cref="Game"/>) holds the logic and the parser/turn scratch; everything that
    /// describes "the current state of the world" lives here so it can later be saved, loaded, or
    /// inspected independently. A fresh instance is created at the start of each session and populated
    /// by <see cref="Game.Initialize"/>.
    ///
    /// Fields are intentionally public and keep the engine's original names so the engine can address
    /// them as _state.* with no other change. (Promoting them to encapsulated properties is a fine
    /// follow-up; it was deliberately left out of this data-only split.)
    /// </summary>
    internal sealed class GameState
    {
        /// <summary>
        /// Room table. First dimension is the room number. Second dimension [0-2] hold the room
        /// description; [3] is the room status (index into <see cref="status"/>); [4] is the door
        /// direction (1=N, 2=S, 3=E, 4=W).
        /// </summary>
        public string[,] _rooms = new string[256, 5];

        /// <summary>Per-room exit table; each string encodes the destination room per direction.</summary>
        public string[] _exits = new string[256];

        /// <summary>Display names of the items, indexed by item number.</summary>
        public string[] _items = new string[36];

        /// <summary>
        /// Item locations, indexed by item number. NaN = carried; .5 = hidden/unavailable; .8 = the
        /// returned sun-drop bottle (hidden); x.1 = present in room x but not listed; >=0 = room number.
        /// </summary>
        public double[] _itemLocations = new double[36];

        /// <summary>Room status strings (door open/closed, stand contents, etc.).</summary>
        public string[] status = new string[30];

        /// <summary>The room the player is currently in.</summary>
        public int _currentRoom;

        // --- Puzzle flags. Defaults here are the new-game starting values. ---
        public int manholeFlag = 0;
        public int storeFrontFlag = 1;
        public int jailFlag = 0;
        public int disguiseFlag = 0;
        public int clothingFlag = 0;
        public int amuletFlag = 0;
        public int quarterFlag = 0;
        public int cigaretteFlag = 0;
        public int bookFlag = 0;
        public int sunDropFlag = 0;
    }
}
