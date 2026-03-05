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

This game was created by Mark Benderman, Tim Eubanks, Matt Sellers and Shannon Smith in our high school computer programming class in 1987 as our final project. The purpose of the class was to teach the BASIC programming language on the schools Radio Shack TRS-80 computers. We all knew BASIC pretty well and so while the class project was to create a 10 question multiple choice quiz written in BASIC on the topic of our choice, the four of us asked the teacher if we could write a text adventure instead. Of course, she had no idea what a text adventure was, but fortunately she said yes.

The TRS-80 disks were propriatary AFAIK and we had no way of saving the game code, but thankfully Shannon and Mark had the foresight of printing it out. They kept the printout in the bauble storage case for many years until one day someone scanned it and used the OCR from the scanner to turn it back into the original BASIC program. This scanner output is the [original game](docs/original-game.txt) file. I converted it to C# (back when .NET Framework 3.5 was a thing) and recently updated it to .NET 10 Core.

## Building

To publish platform-specific self-contained executables:

```bash
dotnet publish "The Jewels of Attu-Kor/The Jewels of Attu-Kor.csproj" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish "The Jewels of Attu-Kor/The Jewels of Attu-Kor.csproj" -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish "The Jewels of Attu-Kor/The Jewels of Attu-Kor.csproj" -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true
```

A GitHub Actions workflow is included to automate building and publishing releases for all three platforms. Trigger it manually from the Actions tab.
