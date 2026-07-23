using BepInEx.Configuration;
using HarmonyLib;
using InnerNet;

namespace ColorfulUI;

// auto kicks low level players on join
// logs every kick now unlike before
public static class LevelGate
{
    static ConfigEntry<bool>? _enabled;
    static int _minLevel = 5;

    public static void Init(ConfigFile cfg)
    {
        _enabled = cfg.Bind("LevelGate", "Enabled", false, "kick low level players on join, host only");
        _minLevel = (int)cfg.Bind("LevelGate", "MinimumLevel", 5, "min level").Value;
    }

    public static void SetEnabled(bool on, int minLvl)
    {
        if (_enabled != null) _enabled.Value = on;
        _minLevel = minLvl;
    }

    static bool IsActive =>
        (_enabled?.Value ?? false) &&
        AmongUsClient.Instance != null &&
        AmongUsClient.Instance.AmHost;

    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
    static class JoinPatch
    {
        static void Postfix(AmongUsClient __instance, ClientData client)
        {
            if (!IsActive) return;
            if (client?.Character == null) return;

            int lvl = (int)client.Character.Data.PlayerLevel;
            if (lvl < _minLevel)
            {
                __instance.KickPlayer(client.Id, false);
                KickLogger.Record(client.Character.Data.PlayerName, lvl, "auto");
            }
        }
    }
}
