# ColorfulUI

A client-side visual overhaul mod for Among Us,for the Starlight launcher.

## What it does

**Themes** - 9 built-in color themes (Midnight, Sunset, Ocean, Crimson, Emerald, Void, Aurora, Rose, Neon) that recolor the chat window and minimap to match. Switch anytime from the mod settings.

**HUD scaling** - resize the in-game HUD independently of the game's own settings, useful on smaller or bigger phone screens.

**Colorblind palette** - optional colorblind-friendly role colors.

**Overlays** (all off by default, toggle individually):
- Kill cooldown timer, impostors only
- Ping display, updates once a second
- Meeting countdown timer, turns red under 10 seconds left
- Kill feed - shows recent kills, host only
- FPS counter, with a one-time nudge to enable Performance Mode if your framerate's been low for a while
- Live vote tally during meetings
- AFK badges over players who haven't moved in ~20 seconds

**Host tools**
- Level gate - auto-kick players under a minimum level on join

**Performance**
- Performance Mode and Reduced Motion toggles for lower-end devices
- Overlays are throttled (updating a few times a second, not every frame) rather than recalculating every tick
- Solid-color textures are cached instead of regenerated on every draw call

## Settings

All toggles live in the mod's config, accessible through Starlight's settings menu. Everything overlay-related defaults to off so the base experience stays vanilla until you turn things on.

## Requirements

Built against BepInEx.Unity.IL2CPP. Needs the Il2Cpp interop assemblies for Among Us Android to compile.

## Notes

A couple of the newer overlays (vote tally, AFK badges) reference some Among Us internals that haven't been fully double-checked against the current game version - if something looks off after an Among Us update, that's probably where to look first.
