using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace ColorfulUI.Themes;

public static class ChatThemePatch
{
    public static Color OverrideBackground = new(0.04f, 0.04f, 0.08f);
    public static Color OverrideText = new(0.75f, 0.75f, 1f);

    private static ConfigEntry<bool>? _enabled;

    public static void Init(ConfigFile cfg)
    {
        _enabled = cfg.Bind("Chat", "ThemeEnabled", true, "recolor chat with active theme");
    }

    public static bool IsEnabled => _enabled?.Value ?? true;

    [HarmonyPatch(typeof(ChatController), nameof(ChatController.Awake))]
    private static class ChatControllerAwakePatch
    {
        static void Postfix(ChatController __instance)
        {
            if (!IsEnabled) return;
            ApplyToChat(__instance);
        }
    }

    [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
    private static class ChatAddPatch
    {
        static void Postfix(ChatController __instance)
        {
            if (!IsEnabled) return;
            ApplyToChat(__instance);
        }
    }

    private static void ApplyToChat(ChatController chat)
    {
        foreach (var sr in chat.GetComponentsInChildren<SpriteRenderer>(true))
        {
            if (sr.color == Color.white)
                sr.color = OverrideBackground;
        }

        foreach (var bubble in chat.GetComponentsInChildren<ChatBubble>(true))
        {
            if (bubble == null) continue;
            foreach (var t in bubble.GetComponentsInChildren<TMPro.TextMeshPro>(true))
                t.color = OverrideText;
        }
    }
}
