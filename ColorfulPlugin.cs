using BepInEx;
using BepInEx.Unity.IL2CPP;
using ColorfulUI.Assets;
using ColorfulUI.Commands;
using ColorfulUI.Hud;
using ColorfulUI.Settings;
using ColorfulUI.Themes;
using HarmonyLib;

namespace ColorfulUI;

[BepInPlugin(Id, Name, Version)]
public sealed class ColorfulPlugin : BasePlugin
{
    public const string Id = "starlight.colorfului";
    public const string Name = "ColorfulUI";
    public const string Version = "1.2.0";

    public Harmony Harmony { get; } = new(Id);

    public override void Load()
    {
        ColorfulConfig.Init(Config);
        ColorAssets.Load();
        ThemeRegistry.Init();
        HudScaler.Init(Config);
        ChatThemePatch.Init(Config);
        MinimapThemePatch.Init(Config);
        Accessibility.ColorblindMode.Init(Config);
        LevelGate.Init(Config);
        ColorfulCommands.Init(Config);
        PlayerNotes.Init();
        WordFilter.Init();

        Harmony.PatchAll();
    }
}
