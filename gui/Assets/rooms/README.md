# Room images

Drop room art in this folder. It is auto-embedded into the GUI via
`<AvaloniaResource Include="Assets/**" />` (in `gui/Game.Gui.csproj`) and resolved at runtime by
`RoomImageProvider` — **no code or project changes needed**. A room with no image falls back to a
placeholder card showing the room name.

## Naming

- **Primary image:** `room{N}.png` — one per room, `room0.png` … `room15.png`.
- **Extra variants** (optional; supports the planned "multiple images per room", e.g. state variants
  like a broken window): `room{N}_2.png`, `room{N}_3.png`, … up to `room{N}_5.png`. The primary
  (`room{N}.png`) is what displays today; variants are reserved for future selection logic.
- **Format:** PNG only (that's what `RoomImageProvider` probes). Ask before switching to another format.

## Spec

The image panel is ~320px wide (image-left / image-right layouts) and ~240px tall (image-top layout),
rendered with `Stretch="Uniform"`. Use a **consistent aspect ratio across all rooms** so they don't
jump as you move — e.g. 4:3 landscape (~800×600) or 1:1 square.

## Room map

| File | Room |
| --- | --- |
| `room0.png` | Hotel Lobby |
| `room1.png` | Your Room (bedroom) |
| `room2.png` | Party Room |
| `room3.png` | Tropical Lounge (plants + bauble / sun-drop machine) |
| `room4.png` | Wine Cellar |
| `room5.png` | Ancient Lab |
| `room6.png` | Downtown Street (arcade entrance) |
| `room7.png` | Arcade |
| `room8.png` | Uptown Street (Jim Dandy market) |
| `room9.png` | Jim Dandy Market (interior) |
| `room10.png` | Motown Street (sun-drop machine, jail, manhole) |
| `room11.png` | Police Station (front office) |
| `room12.png` | Dark Alley |
| `room13.png` | Jail Cell |
| `room14.png` | Evidence Room |
| `room15.png` | Temple of Attu-Kor |

Room numbers and names mirror `RoomImageProvider` and `Game.Initialize()` in the engine.
