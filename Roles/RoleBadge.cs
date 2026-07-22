using System.Collections.Generic;
using ColorfulUI.Assets;
using UnityEngine;

namespace ColorfulUI.Roles;

public static class RoleBadge
{
    private static readonly Dictionary<string, Color> _colors = new()
    {
        ["Impostor"] = new Color(0.88f, 0.12f, 0.12f),
        ["Shapeshifter"] = new Color(0.85f, 0.30f, 0.00f),
        ["Phantom"] = new Color(0.60f, 0.10f, 0.80f),
        ["Crewmate"] = new Color(0.25f, 0.60f, 1f),
        ["Engineer"] = new Color(0.15f, 0.75f, 0.95f),
        ["Scientist"] = new Color(0.9f, 0.8f, 0.1f),
        ["Guardian"] = new Color(0.30f, 0.90f, 0.55f),
        ["Noisemaker"] = new Color(0.95f, 0.55f, 0.15f),
        ["Tracker"] = new Color(0.35f, 0.95f, 0.35f),
        ["Oracle"] = new Color(0.70f, 0.50f, 0.95f),
    };

    public static Color Get(string roleName)
    {
        return _colors.TryGetValue(roleName, out var c) ? c : Color.white;
    }

    public static Texture2D GetTexture(string roleName)
    {
        var key = roleName.ToLowerInvariant();
        var tex = ColorAssets.GetPlayerColorTexture(key);
        if (tex != null) return tex;

        var color = Get(roleName);
        var fallback = new Texture2D(32, 32);
        var pixels = new Color[32 * 32];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = color;
        fallback.SetPixels(pixels);
        fallback.Apply();
        return fallback;
    }
}
