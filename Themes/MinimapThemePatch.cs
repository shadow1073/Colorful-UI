using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace ColorfulUI.Themes;

public static class MinimapThemePatch
{
    public static Color OverrideBackground = new(0.03f, 0.03f, 0.07f);
    public static Color OverrideVent = new(0.45f, 0.65f, 1f);
    public static Color OverrideSabotage = new(1f, 0.30f, 0.30f);

    private static ConfigEntry<bool>? _enabled;

    public static void Init(ConfigFile cfg)
    {
        _enabled = cfg.Bind("Minimap", "ThemeEnabled", true, "recolor minimap with active theme");
    }

    public static bool IsEnabled => _enabled?.Value ?? true;

    [HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.ShowNormalMap))]
    private static class ShowNormalMapPatch
    {
        static void Postfix(MapBehaviour __instance)
        {
            if (!IsEnabled) return;
            ApplyToMap(__instance);
        }
    }

    private static void ApplyToMap(MapBehaviour map)
    {
        var bg = map.GetComponent<UnityEngine.UI.Image>();
        if (bg != null) bg.color = OverrideBackground;

        foreach (var vent in map.GetComponentsInChildren<MapVent>(true))
        {
            var img = vent.GetComponent<SpriteRenderer>();
            if (img != null) img.color = OverrideVent;
        }

        foreach (var sab in map.GetComponentsInChildren<SabotageTask>(true))
        {
            var img = sab.GetComponent<SpriteRenderer>();
            if (img != null) img.color = OverrideSabotage;
        }
    }
}
