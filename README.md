#ColorfulUI
A client-side visual overhaul mod for Among Us, for the Starlight launcher.
What it does
Themes - 9 built-in color themes (Midnight, Sunset, Ocean, Crimson, Emerald, Void, Aurora, Rose, Neon) that recolor the chat window, minimap, and main menu to match. Switch anytime from the mod settings.
HUD scaling - resize the in-game HUD independently of the game's own settings, useful on smaller or bigger phone screens.
Colorblind palette - optional colorblind-friendly role colors.
Overlays (all off by default, toggle individually):
Kill cooldown timer, impostors only
Ping display, updates once a second
Meeting countdown timer, turns red under 10 seconds left
Kill feed - shows recent kills, host only
FPS counter, with a one-time nudge to turn on Reduce Overlay Load if your framerate's been low for a while
Live vote tally during meetings
AFK badges over players who haven't moved in ~20 seconds
Host tools
Level gate - auto-kick players under a minimum level on join. /kicklevel <N> also lets you sweep the current lobby manually whenever
Kick history - keeps a local log of who got kicked, when, and why (auto or manual). /history for the count, /history <name> for someone specific
Word filter - auto-kicks anyone who says a banned word in chat. words live in a local txt file, kicks get added to history too
Custom lobby title - /title to set one
Auto-start - arm the lobby with /autostart <N> and it'll start once you hit N players. still WIP, see notes below
Player notes - leave yourself a note next to someone's name (good teammate, troll, whatever). local only, nobody else sees it, and yeah if they change their name it won't follow them, havent gotten around to fixing that
Overlay performance (Among Us itself doesn't need this, this is just for the mod's own overlays)
Reduce Overlay Load and Reduced Motion toggles for older/weaker phones
Overlays are throttled instead of recalculating every frame
solid color textures get cached instead of regenerated every draw call
Settings
All toggles live in the mod's config, accessible through Starlight's settings menu. everything overlay-related defaults off so it stays vanilla until you turn stuff on.
Requirements
Built against BepInEx.Unity.IL2CPP. Needs the Il2Cpp interop assemblies for Among Us Android to compile.
Notes / known issues
vote tally, AFK badges, and main menu theming all poke at Among Us internals I haven't fully confirmed - might break on the next game update.
auto-start doesn't press the start button yet, that part's commented out til I get the real field name. arming/player count still works fine.
