using System.Collections.Generic;
using ColorfulUI.Themes;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace ColorfulUI.Hud;

public static class KillFeed
{
    public static bool IsEnabledOverride = false;

    public static bool IsActive =>
        IsEnabledOverride &&
        AmongUsClient.Instance != null &&
        AmongUsClient.Instance.AmHost;

    public static void Push(string victimName)
    {
        if (!IsActive) return;
        KillFeedOverlay.Push(victimName);
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    private static class MurderPlayerPatch
    {
        static void Postfix(PlayerControl target)
        {
            if (target?.Data == null) return;
            Push(target.Data.PlayerName);
        }
    }
}

public class KillFeedOverlay : MonoBehaviour
{
    private sealed class FeedEntry
    {
        public string Text  = "";
        public float  Timer = 4f;
    }

    private static readonly List<FeedEntry> _entries = new();

    public static void Register()
    {
        ClassInjector.RegisterTypeInIl2Cpp<KillFeedOverlay>();
    }

    public static void Push(string name)
    {
        _entries.Add(new FeedEntry { Text = $"[dead] {name} got killed" });
    }

    void Update()
    {
        for (int i = _entries.Count - 1; i >= 0; i--)
        {
            _entries[i].Timer -= Time.deltaTime;
            if (_entries[i].Timer <= 0f) _entries.RemoveAt(i);
        }
    }

    void OnGUI()
    {
        if (_entries.Count == 0) return;

        var theme = ThemeRegistry.Active;
        float y = Screen.height / 2f - 20f;

        foreach (var entry in _entries)
        {
            float a = Mathf.Clamp01(entry.Timer);
            GUI.backgroundColor = new Color(
                theme.OverlayBackground.r,
                theme.OverlayBackground.g,
                theme.OverlayBackground.b,
                theme.OverlayBackground.a * a);
            GUI.contentColor = new Color(1f, 0.40f, 0.40f, a);
            GUI.Box(new Rect(8f, y, 240f, 30f), entry.Text);
            y += 34f;
        }

        GUI.backgroundColor = Color.white;
        GUI.contentColor    = Color.white;
    }
}
