using BepInEx.Configuration;

namespace ColorfulUI.Settings;

public static class ColorfulConfig
{
    public static ConfigEntry<int> ActiveThemeIndex = null!;
    public static ConfigEntry<float> HudScale = null!;
    public static ConfigEntry<bool> ColorblindPalette = null!;
    public static ConfigEntry<bool> ChatTheme = null!;
    public static ConfigEntry<bool> MinimapTheme = null!;
    public static ConfigEntry<bool> ShowKillCooldown = null!;
    public static ConfigEntry<bool> ShowPingDisplay = null!;
    public static ConfigEntry<bool> ShowMeetingTimer = null!;
    public static ConfigEntry<bool> KillFeedEnabled = null!;
    public static ConfigEntry<bool> LevelGateEnabled = null!;
    public static ConfigEntry<float> LevelGateMinimum = null!;
    public static ConfigEntry<bool> PerformanceMode = null!;
    public static ConfigEntry<bool> ReducedMotion = null!;
    public static ConfigEntry<bool> ShowFpsCounter = null!;
    public static ConfigEntry<bool> ShowVoteTally = null!;
    public static ConfigEntry<bool> ShowAfkBadges = null!;
    public static ConfigEntry<bool> MainMenuTheme = null!;
    public static ConfigEntry<float> MainMenuButtonScale = null!;

    public static void Init(ConfigFile cfg)
    {
        ActiveThemeIndex = cfg.Bind("Visual", "ActiveThemeIndex", 0, "0=Midnight 1=Sunset 2=Ocean 3=Crimson 4=Emerald 5=Void 6=Aurora 7=Rose 8=Neon 9=Lavender");
        HudScale = cfg.Bind("Visual", "HudScale", 0.90f, "HUD size, 1.0 is default");
        ColorblindPalette = cfg.Bind("Visual", "ColorblindPalette", false, "colorblind-friendly role colors");
        ChatTheme = cfg.Bind("Visual", "ChatTheme", true, "recolor chat with active theme");
        MinimapTheme = cfg.Bind("Visual", "MinimapTheme", true, "recolor minimap with active theme");

        ShowKillCooldown = cfg.Bind("Overlays", "ShowKillCooldown", false, "kill cooldown timer, impostors only obviously");
        ShowPingDisplay = cfg.Bind("Overlays", "ShowPingDisplay", true, "just your ping, in ms");
        ShowMeetingTimer = cfg.Bind("Overlays", "ShowMeetingTimer", true, "meeting countdown timer");

        KillFeedEnabled = cfg.Bind("HostTools", "KillFeedEnabled", false, "show kill notifications - HOST ONLY, does nothing for regular players");
        LevelGateEnabled = cfg.Bind("HostTools", "LevelGateEnabled", false, "auto-kick low level players, host only");
        LevelGateMinimum = cfg.Bind("HostTools", "LevelGateMinimum", 5f, "min level to join");

        // low-end phones exist, gotta respect that
        PerformanceMode = cfg.Bind("Performance", "PerformanceMode", false, "cuts overlay update rate further, disables theme transition fades");
        ReducedMotion = cfg.Bind("Performance", "ReducedMotion", false, "skips camera shake and particle-heavy sabotage effects");
        ShowFpsCounter = cfg.Bind("Performance", "ShowFpsCounter", false, "shows fps in corner, also nags you once if its bad lol");
        ShowVoteTally = cfg.Bind("Overlays", "ShowVoteTally", false, "live vote count during meetings");
        ShowAfkBadges = cfg.Bind("Overlays", "ShowAfkBadges", false, "shows AFK tag over players who havent moved in a bit");
        MainMenuTheme = cfg.Bind("Menu", "MainMenuTheme", false, "colors the main menu buttons to match active theme");
        MainMenuButtonScale = cfg.Bind("Menu", "MainMenuButtonScale", 1f, "shrink/grow the play/inventory/shop buttons, 1.0 = normal");
    }
}
