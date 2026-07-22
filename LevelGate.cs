using BepInEx.Configuration;
using HarmonyLib;

namespace ColorfulUI;

public static class LevelGate
{
    private static ConfigEntry<bool>? _enabled;
    private static int _minLevel = 5;

    public static void Init(ConfigFile cfg)
    {
        _enabled = cfg.Bind("LevelGate", "Enabled", false,
            "Kick low level players on join (host only)");
        _minLevel = (int)cfg.Bind("LevelGate", "MinimumLevel", 5,
            "Players under this level get kicked").Value;
    }

    public static void SetEnabled(bool enabled, int minLevel)
    {
        if (_enabled != null) _enabled.Value = enabled;
        _minLevel = minLevel;
    }

    private static bool IsActive =>
        (_enabled?.Value ?? false) &&
        AmongUsClient.Instance != null &&
        AmongUsClient.Instance.AmHost;

    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
    private static class PlayerJoinedPatch
    {
        static void Postfix(AmongUsClient __instance, ClientData client)
        {
            if (!IsActive) return;
            if (client?.Character == null) return;

            int level = client.Character.Data.PlayerLevel;
            if (level < _minLevel)
                __instance.KickPlayer(client.Id, false);
        }
    }
}
