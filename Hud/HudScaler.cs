using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace ColorfulUI.Hud;

public static class HudScaler
{
    private static ConfigEntry<float>? _scale;
    private static float _lastAppliedScale = -1f; // forces first apply

    public static float Scale => _scale?.Value ?? 1.0f;

    public static void Init(ConfigFile cfg)
    {
        _scale = cfg.Bind("Hud", "Scale", 0.90f, "HUD size, 1.0 is default");
    }

    public static void SetScale(float value)
    {
        if (_scale == null) return;
        _scale.Value = Mathf.Clamp(value, 0.4f, 1.5f);
    }

    private static void Apply(HudManager hud)
    {
        var rt = hud.GetComponent<RectTransform>();
        if (rt == null) return;
        rt.localScale = Vector3.one * Scale;
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    private static class HudStartPatch
    {
        static void Postfix(HudManager __instance)
        {
            Apply(__instance);
            _lastAppliedScale = Scale;
        }
    }

    // was reapplying this transform every single frame via HudManager.Update -
    // pointless if nobody touched the slider since the last frame. only
    // reapply when the value actually changed (e.g. user just moved the
    // slider), otherwise skip entirely
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    private static class HudUpdatePatch
    {
        static void Postfix(HudManager __instance)
        {
            if (Mathf.Approximately(Scale, _lastAppliedScale)) return;
            Apply(__instance);
            _lastAppliedScale = Scale;
        }
    }
}
