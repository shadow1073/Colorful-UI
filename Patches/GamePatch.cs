using ColorfulUI.Commands;
using ColorfulUI.Settings;
using ColorfulUI.Themes;
using HarmonyLib;

namespace ColorfulUI.Patches;

[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameJoined))]
public static class GameJoinedPatch
{
    static void Postfix()
    {
        OptionWatcher.Apply();
        ThemeApplicator.Refresh();
    }
}

[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.ExitGame))]
public static class GameExitPatch
{
    static void Postfix()
    {
        ColorfulCommands.ClearSession();
    }
}

[HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.ResetStartState))]
public static class RoundResetPatch
{
    static void Postfix()
    {
        ColorfulCommands.ClearSession();
        OptionWatcher.Apply();
    }
}
