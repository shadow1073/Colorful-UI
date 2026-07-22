using ColorfulUI.Themes;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace ColorfulUI.Hud;

public class MeetingTimerHud : MonoBehaviour
{
    public static bool Enabled = true;

    private Rect _rect = new(Screen.width / 2f - 70f, 8f, 140f, 38f);

    public static void Register()
    {
        ClassInjector.RegisterTypeInIl2Cpp<MeetingTimerHud>();
    }

    void OnGUI()
    {
        if (!Enabled) return;

        var meeting = MeetingHud.Instance;
        if (meeting == null) return;

        float remaining = meeting.discussionTimer;
        if (remaining <= 0f) return;

        var theme = ThemeRegistry.Active;
        GUI.backgroundColor = remaining <= 10f
            ? new Color(0.60f, 0.05f, 0.05f, 0.88f)
            : theme.OverlayBackground;
        GUI.contentColor = theme.OverlayText;
        GUI.Box(_rect, $"Time: {remaining:F0}s");
        GUI.backgroundColor = Color.white;
        GUI.contentColor    = Color.white;
    }
}
