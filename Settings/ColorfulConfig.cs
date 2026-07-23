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

    // new stuff
    public static ConfigEntry<bool> WordFilterEnabled = null!;

    public static void Init(ConfigFile cfg)
    {
        ActiveThemeIndex = cfg.Bind("Visual", "ActiveThemeIndex", 0, "0=Midnight 1=Sunset 2=Ocean 3=Crimson 4=Emerald 5=Void 6=Aurora 7=Rose 8=Neon 9=Lavender");
        HudScale = cfg.Bind("Visual", "HudScale", 0.90f, "hud size multiplier");
        ColorblindPalette = cfg.Bind("Visual", "ColorblindPalette", false, "colorblind role colors");
        ChatTheme = cfg.Bind("Visual", "ChatTheme", true, "recolor chat with theme");
        MinimapTheme = cfg.Bind("Visual", "MinimapTheme", true, "recolor minimap with theme");

        ShowKillCooldown = cfg.Bind("Overlays", "ShowKillCooldown", false, "kill cooldown, impostors only");
        ShowPingDisplay = cfg.Bind("Overlays", "ShowPingDisplay", true, "show ping");
        ShowMeetingTimer = cfg.Bind("Overlays", "ShowMeetingTimer", true, "meeting timer");
        ShowVoteTally = cfg.Bind("Overlays", "ShowVoteTally", false, "live vote tally in meetings");
        ShowAfkBadges = cfg.Bind("Overlays", "ShowAfkBadges", false, "afk badge over idle players");

        KillFeedEnabled = cfg.Bind("HostTools", "KillFeedEnabled", false, "kill feed, host only");
        LevelGateEnabled = cfg.Bind("HostTools", "LevelGateEnabled", false, "auto kick low levels");
        LevelGateMinimum = cfg.Bind("HostTools", "LevelGateMinimum", 5f, "min level to join");
        WordFilterEnabled = cfg.Bind("HostTools", "WordFilterEnabled", true, "auto kick for banned words");

        // for potato phones 
        PerformanceMode = cfg.Bind("Performance", "PerformanceMode", false, "lower update rate, no theme fades");
        ReducedMotion = cfg.Bind("Performance", "ReducedMotion", false, "no camera shake");
        ShowFpsCounter = cfg.Bind("Performance", "ShowFpsCounter", false, "fps counter");

        MainMenuTheme = cfg.Bind("Menu", "MainMenuTheme", false, "theme colors on main menu");
        MainMenuButtonScale = cfg.Bind("Menu", "MainMenuButtonScale", 1f, "main menu button size");
    }
}
