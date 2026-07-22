using ColorfulUI.Settings;
using ColorfulUI.Themes;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace ColorfulUI.Hud;

public class FpsMonitor : MonoBehaviour
{
    public static bool enabled_ = false; // renamed bc MonoBehaviour already has "enabled"

    float nextSample;
    float fps;
    int lowFpsStreak;
    bool suggested;

    public static void Register()
    {
        ClassInjector.RegisterTypeInIl2Cpp<FpsMonitor>();
    }

    void Update()
    {
        // sample every half second, nobody needs a jittery live fps readout
        if(Time.unscaledTime < nextSample) return;
        nextSample = Time.unscaledTime + 0.5f;

        fps = 1f / Time.unscaledDeltaTime;

        if (fps < 30f && !ColorfulConfig.PerformanceMode.Value)
            lowFpsStreak++;
        else
            lowFpsStreak = 0;

        // been chugging for 10+ consecutive checks (~5 sec) and perf mode is
        // off - nudge once, don't nag every time it dips
        if (lowFpsStreak > 10 && !suggested)
        {
            suggested = true;
            Debug.Log("[ColorfulUI] fps has been low for a while, consider enabling Performance Mode in settings");
        }
    }

    void OnGUI()
    {
        if (!ColorfulConfig.ShowFpsCounter.Value) return;

        var theme = ThemeRegistry.Active;
        GUI.contentColor = fps < 30 ? Color.red : theme.OverlayText;
        GUI.Label(new Rect(10, Screen.height - 30, 100, 24), $"{fps:F0} fps");
        GUI.contentColor = Color.white;
    }
}
