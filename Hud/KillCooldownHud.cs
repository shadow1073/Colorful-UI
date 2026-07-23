using ColorfulUI.Themes;
using Il2CppInterop.Runtime.Injection;
using InnerNet;
using UnityEngine;

namespace ColorfulUI.Hud;

public class KillCooldownHud : MonoBehaviour
{
    public static bool Enabled = true;

    private Rect _rect = new(Screen.width - 140f, 12f, 130f, 44f);

    public static void Register()
    {
        ClassInjector.RegisterTypeInIl2Cpp<KillCooldownHud>();
    }

    void OnGUI()
    {
        if (!Enabled) return;

        var player = PlayerControl.LocalPlayer;
        if (player == null || player.Data == null) return;
        if (!player.Data.Role.IsImpostor) return;

        if (GameData.Instance == null || AmongUsClient.Instance == null) return;
        if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started) return;

        float cd = player.killTimer;
        if (cd <= 0f) return;

        var theme = ThemeRegistry.Active;
        GUI.backgroundColor = theme.OverlayBackground;
        GUI.contentColor    = cd > 5f ? theme.OverlayText : Color.red;
        GUI.Box(_rect, $"Kill: {cd:F1}s");
        GUI.backgroundColor = Color.white;
        GUI.contentColor    = Color.white;
    }
}
