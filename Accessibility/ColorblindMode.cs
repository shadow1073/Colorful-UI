using System.Collections.Generic;
using BepInEx.Configuration;
using UnityEngine;

namespace ColorfulUI.Accessibility;

public static class ColorblindMode
{
    private static ConfigEntry<bool>? _enabled;

    public static bool IsEnabled => _enabled?.Value ?? false;

    private static readonly Dictionary<string, Color> _palette = new()
    {
        ["Impostor"]    = new Color(0.90f, 0.62f, 0.00f),
        ["Crewmate"]    = new Color(0.00f, 0.45f, 0.70f),
        ["Dead"]        = new Color(0.60f, 0.60f, 0.60f),
        ["Engineer"]    = new Color(0.00f, 0.62f, 0.45f),
        ["Scientist"]   = new Color(0.80f, 0.47f, 0.65f),
        ["Guardian"]    = new Color(0.94f, 0.89f, 0.26f),
        ["Phantom"]     = new Color(0.34f, 0.71f, 0.91f),
        ["Noisemaker"]  = new Color(0.90f, 0.62f, 0.00f),
        ["Shapeshifter"]= new Color(0.90f, 0.38f, 0.00f),
    };

    public static void Init(ConfigFile cfg)
    {
        _enabled = cfg.Bind("Accessibility", "ColorblindMode", false, "colorblind role colors");
    }

    public static Color Resolve(string roleKey, Color fallback)
    {
        if (!IsEnabled) return fallback;
        return _palette.TryGetValue(roleKey, out var c) ? c : fallback;
    }
}
