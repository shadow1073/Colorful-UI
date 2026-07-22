using ColorfulUI.Hud;
using HarmonyLib;
using UnityEngine;

namespace ColorfulUI.Patches;

[HarmonyPatch(typeof(HudManager), nameof(HudManager.Awake))]
public static class HudAwakePatch
{
    private static bool _registered;

    static void Postfix(HudManager __instance)
    {
        if (!_registered)
        {
            KillCooldownHud.Register();
            MeetingTimerHud.Register();
            PingDisplay.Register();
            KillFeedOverlay.Register();
            FpsMonitor.Register();
            VoteTallyOverlay.Register();
            AfkIndicator.Register();
            _registered = true;
        }

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
