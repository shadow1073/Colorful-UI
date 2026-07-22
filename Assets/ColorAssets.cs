using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ColorfulUI.Assets;

public static class ColorAssets
{
    private static readonly Dictionary<string, Texture2D> _cache = new();

    public static readonly string[] PlayerColorNames =
    {
        "red", "blue", "green", "purple", "yellow", "black",
        "white", "orange", "brown", "cyan", "lime", "maroon",
        "rose", "banana", "coral", "teal", "blueberry", "maple",
        "olive", "azure", "gold", "jungle", "cocoa", "cream",
        "strawberry", "watermelon", "chocolate", "sky", "beige", "mint",
    };

    public static void Load()
    {
        var asm = Assembly.GetExecutingAssembly();

        foreach (var colorName in PlayerColorNames)
        {
            var resourceName = $"ColorfulUI.Resources.Colors.{colorName}.png";
            using var stream = asm.GetManifestResourceStream(resourceName);
            if (stream == null) continue;

            var bytes = ReadAll(stream);
            var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            if (tex.LoadImage(bytes))
            {
                tex.name = colorName;
                _cache[colorName] = tex;
            }
        }
    }

    public static Texture2D? GetPlayerColorTexture(string colorName)
    {
        _cache.TryGetValue(colorName.ToLowerInvariant(), out var tex);
        return tex;
    }

    public static Texture2D? GetByIndex(int index)
    {
        if (index < 0 || index >= PlayerColorNames.Length) return null;
        return GetPlayerColorTexture(PlayerColorNames[index]);
    }

    private static byte[] ReadAll(Stream stream)
    {
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }
}
