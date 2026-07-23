using ColorfulUI.Hud;
using HarmonyLib;
using UnityEngine;

namespace ColorfulUI.Patches;

[HarmonyPatch(typeof(HudManager), nameof(HudManager.Awake))]
public static class HudAwakePatch
{
    private static bool _typesRegistered;
    private static bool _overlaysSpawned;

    static void Postfix(HudManager __instance)
    {
        // ClassInjector.RegisterTypeInIl2Cpp only needs to happen once ever,
        // calling it again explodes, so guard it
        if (!_typesRegistered)
        {
            KillCooldownHud.Register();
            MeetingTimerHud.Register();
            PingDisplay.Register();
            KillFeedOverlay.Register();
            FpsMonitor.Register();
            VoteTallyOverlay.Register();
            AfkIndicator.Register();
            _typesRegistered = true;
        }

        // HudManager.Awake fires on every scene load but the host GameObject
        // is DontDestroyOnLoad so it sticks around - without this guard you'd
        // end up with like 4 stacked copies of every overlay by midgame lol
        if (_overlaysSpawned) return;
        _overlaysSpawned = true;

        var host = new GameObject("ColorfulUI_Overlays");
        Object.DontDestroyOnLoad(host);

        host.AddComponent<KillCooldownHud>();
        host.AddComponent<MeetingTimerHud>();
        host.AddComponent<PingDisplay>();
        host.AddComponent<KillFeedOverlay>();
        host.AddComponent<FpsMonitor>();
        host.AddComponent<VoteTallyOverlay>();
        host.AddComponent<AfkIndicator>();
    }
}
