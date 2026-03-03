# The Jewels of Attu-Kor

A classic text adventure game in the tradition of Infocom titles like *Zork* and *Planetfall*. Explore a strange world of hotel lobbies, dark alleys, retro arcades, and ancient temples as you search for the legendary Jewels of Attu-Kor.

## Download and Run

### Pre-built executables

Download the latest release from the [Releases](../../releases) page. Each release includes standalone executables for:

- **Windows** (x64) — `The-Jewels-of-Attu-Kor-win-x64.zip`
- **Linux** (x64) — `The-Jewels-of-Attu-Kor-linux-x64.zip`
- **macOS** (x64) — `The-Jewels-of-Attu-Kor-osx-x64.zip`

These are self-contained — no .NET installation required. Download, unzip, and run.

**Windows:**
```
"The Jewels of Attu-Kor.exe"
```

**Linux / macOS:**
```bash
chmod +x "The Jewels of Attu-Kor"
./"The Jewels of Attu-Kor"
```

### Build from source

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0).

```bash
dotnet run --project "The Jewels of Attu-Kor"
```

## How to Play

You interact with the game by typing commands at the prompt. The parser accepts **two-word commands** in the form `VERB NOUN` (e.g., `GET NEWSPAPER`, `OPEN DOOR`, `EXAMINE BOOK`). Commands are not case-sensitive.

### Navigation

Move between locations using compass directions:

| Command | Action |
|---------|--------|
| `N` | Go North |
| `S` | Go South |
| `E` | Go East |
| `W` | Go West |
| `U` | Go Up |
| `D` | Go Down |

### Shortcut Commands

| Command | Action |
|---------|--------|
| `L` | Look around the current room |
| `I` | Check your inventory |
| `G` | Repeat your last command |

### Verbs

The game understands the following actions:

`GET` `TAKE` `DROP` `OPEN` `CLOSE` `PULL` `PUT` `READ` `EXAMINE` `LOOK` `THROW` `GIVE` `BUY` `STEAL` `KILL` `DRINK` `SMOKE` `WEAR` `REMOVE` `GLUE` `BREAK` `RETURN`

### Tips

- **Look everywhere.** Use `EXAMINE` on objects to discover hidden items and clues.
- **Refer to jewels by color.** The game expects `GET COBALT` or `GET ORANGE`, not `GET JEWEL`.
- **Some paths are blocked.** You may need the right item or action to open a new route.
- **Watch your step.** Not every action is safe — some choices are fatal.
- **Explore thoroughly.** There are 16 locations to discover, from a hotel lobby to an ancient temple.

## History

*[Add your history and background here.]*

## Building

To publish platform-specific self-contained executables:

```bash
dotnet publish "The Jewels of Attu-Kor/The Jewels of Attu-Kor.csproj" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish "The Jewels of Attu-Kor/The Jewels of Attu-Kor.csproj" -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish "The Jewels of Attu-Kor/The Jewels of Attu-Kor.csproj" -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true
```

A GitHub Actions workflow is included to automate building and publishing releases for all three platforms. Trigger it manually from the Actions tab.
