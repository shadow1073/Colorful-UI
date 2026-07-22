using ColorfulUI.Themes;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace ColorfulUI.Hud;

public class PingDisplay : MonoBehaviour
{
    public static bool Enabled = true;

    private Rect _rect = new(Screen.width - 100f, Screen.height - 44f, 92f, 34f);
    private float _nextUpdate;
    private int _lastPing;

    public static void Register()
    {
        ClassInjector.RegisterTypeInIl2Cpp<PingDisplay>();
    }

    void Update()
    {
        if (Time.time < _nextUpdate) return;
        _nextUpdate = Time.time + 1f;

        if (AmongUsClient.Instance != null)
            _lastPing = AmongUsClient.Instance.Ping;
    }

    void OnGUI()
    {
        if (!Enabled) return;

        var theme = ThemeRegistry.Active;
        GUI.backgroundColor = theme.OverlayBackground;
        GUI.contentColor    = _lastPing > 200 ? Color.red
                            : _lastPing > 100 ? Color.yellow
                            : theme.OverlayText;
        GUI.Box(_rect, $"{_lastPing} ms");
        GUI.backgroundColor = Color.white;
        GUI.contentColor    = Color.white;
    }
}
