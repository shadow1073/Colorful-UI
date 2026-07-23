using BepInEx.Configuration;
using HarmonyLib;
using InnerNet;

namespace ColorfulUI;

// auto starts the game when lobby hits target player count
// 0 = disabled
public static class AutoStart
{
    static int _target = 0;

    public static bool IsArmed => _target > 0;
    public static int Target => _target;

    public static void Init(ConfigFile cfg)
    {
        // not saved to cfg, host sets it each session manually
    }

    public static void Set(int n) => _target = n;
    public static void Disable() => _target = 0;

    static void TryStart()
    {
        if (!IsArmed) return;
        if (AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost) return;
        if (LobbyBehaviour.Instance == null) return;

        int count = AmongUsClient.Instance.allClients.Count;
        if (count < _target) return;
        
        if (GameStartManager.Instance != null)
            GameStartManager.Instance.StartButton.ReceiveClickDown();

        _target = 0; // disarm so it doesn't fire again
    }

    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
    static class JoinPatch
    {
        static void Postfix(ClientData client)
        {
            if (client?.Character == null) return;
            TryStart();
        }
    }
}
