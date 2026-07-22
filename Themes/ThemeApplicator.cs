using System.Collections.Generic;
using UnityEngine;

namespace ColorfulUI.Themes;

public static class ThemeApplicator
{
    public static void Refresh()
    {
        ApplyChatColors();
        ApplyMinimapColors();
    }

    private static void ApplyChatColors()
    {
        var theme = ThemeRegistry.Active;
        ChatThemePatch.OverrideBackground = theme.ChatBackground;
        ChatThemePatch.OverrideText = theme.ChatText;
    }

    private static void ApplyMinimapColors()
    {
        var theme = ThemeRegistry.Active;
        MinimapThemePatch.OverrideBackground = theme.MinimapBackground;
        MinimapThemePatch.OverrideVent = theme.MinimapVent;
        MinimapThemePatch.OverrideSabotage = theme.MinimapSabotage;
    }

    // was allocating a new 1x1 texture every single call - fine for a one-off
    // theme swap but adds up fast if anything calls this per-frame (overlays
    // do). caching by color since there's only ever a handful of colors in
    // play at once anyway
    private static readonly Dictionary<Color, Texture2D> _textureCache = new();

    public static Texture2D SolidTexture(Color color)
    {
        if (_textureCache.TryGetValue(color, out var cached) && cached != null)
            return cached;

        var tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, color);
        tex.Apply();
        _textureCache[color] = tex;
        return tex;
    }
}
