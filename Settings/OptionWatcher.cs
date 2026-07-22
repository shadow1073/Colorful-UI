using ColorfulUI.Hud;
using ColorfulUI.Themes;

namespace ColorfulUI.Settings;

public static class OptionWatcher
{
    public static void Apply()
    {
        var themes = ThemeRegistry.All;
        int idx = ColorfulConfig.ActiveThemeIndex.Value;
        if (idx >= 0 && idx < themes.Count)
            ThemeRegistry.TryApply(themes[idx].Name);

        HudScaler.SetScale(ColorfulConfig.HudScale.Value);

        KillCooldownHud.Enabled = ColorfulConfig.ShowKillCooldown.Value;
        PingDisplay.Enabled = ColorfulConfig.ShowPingDisplay.Value;
        MeetingTimerHud.Enabled = ColorfulConfig.ShowMeetingTimer.Value;

        KillFeed.IsEnabledOverride = ColorfulConfig.KillFeedEnabled.Value;
        LevelGate.SetEnabled(ColorfulConfig.LevelGateEnabled.Value, (int)ColorfulConfig.LevelGateMinimum.Value);
    }
}
