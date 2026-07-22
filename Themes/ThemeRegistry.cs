using System.Collections.Generic;
using UnityEngine;

namespace ColorfulUI.Themes;

public static class ThemeRegistry
{
    public static readonly List<ThemeData> All = new();
    public static ThemeData Active { get; private set; } = null!;

    public static void Init()
    {
        All.Add(new ThemeData
        {
            Name = "Midnight",
            PanelBackground = new Color(0.06f, 0.06f, 0.10f, 0.96f),
            PanelAccent = new Color(0.40f, 0.40f, 0.70f),
            PanelText = Color.white,
            ChatBackground = new Color(0.04f, 0.04f, 0.08f),
            ChatText = new Color(0.75f, 0.75f, 1f),
            MinimapBackground = new Color(0.03f, 0.03f, 0.07f),
            MinimapVent = new Color(0.45f, 0.65f, 1f),
            MinimapSabotage = new Color(1f, 0.3f, 0.3f),
            OverlayBackground = new Color(0.05f, 0.05f, 0.12f, 0.88f),
            OverlayText = new Color(0.8f, 0.8f, 1f),
            OverlayAccent = new Color(0.4f, 0.4f, 0.9f),
        });

        // warm one
        All.Add(new ThemeData
        {
            Name = "Sunset",
            ChatBackground = new Color(0.14f, 0.05f, 0.02f),
            ChatText = new Color(1f, 0.82f, 0.55f),
            PanelBackground = new Color(0.28f, 0.09f, 0.04f, 0.96f),
            PanelAccent = new Color(0.95f, 0.55f, 0.12f),
            PanelText = Color.white,
            MinimapBackground = new Color(0.18f, 0.07f, 0.03f),
            MinimapVent = new Color(1f, 0.6f, 0.2f),
            MinimapSabotage = new Color(1f, 0.2f, 0.1f),
            OverlayBackground = new Color(0.22f, 0.08f, 0.03f, 0.88f),
            OverlayText = new Color(1f, 0.85f, 0.6f),
            OverlayAccent = new Color(0.95f, 0.55f, 0.12f),
        });

        All.Add(new ThemeData
        {
            Name = "Ocean",
            PanelBackground = new Color(0.03f, 0.10f, 0.16f, 0.96f),
            PanelAccent = new Color(0.15f, 0.6f, 0.7f),
            PanelText = Color.white,
            ChatBackground = new Color(0.02f, 0.08f, 0.11f),
            ChatText = new Color(0.6f, 0.92f, 1f),
            MinimapBackground = new Color(0.02f, 0.07f, 0.10f),
            MinimapVent = new Color(0.3f, 0.9f, 0.9f),
            MinimapSabotage = new Color(1f, 0.4f, 0.3f),
            OverlayBackground = new Color(0.03f, 0.09f, 0.14f, 0.88f),
            OverlayText = new Color(0.65f, 0.95f, 1f),
            OverlayAccent = new Color(0.2f, 0.75f, 0.85f),
        });

        All.Add(new ThemeData
        {
            Name = "Crimson",
            PanelBackground = new Color(0.18f, 0.03f, 0.03f, 0.96f),
            PanelAccent = new Color(0.9f, 0.18f, 0.18f),
            PanelText = Color.white,
            ChatBackground = new Color(0.12f, 0.02f, 0.02f),
            ChatText = new Color(1f, 0.7f, 0.7f),
            MinimapBackground = new Color(0.10f, 0.02f, 0.02f),
            MinimapVent = new Color(1f, 0.35f, 0.35f),
            MinimapSabotage = new Color(1f, 0.9f, 0.2f), // yellow, otherwise invisible on red
            OverlayBackground = new Color(0.15f, 0.03f, 0.03f, 0.88f),
            OverlayText = new Color(1f, 0.75f, 0.75f),
            OverlayAccent = new Color(0.9f, 0.18f, 0.18f),
        });

        All.Add(new ThemeData
        {
            Name = "Emerald",
            PanelBackground = new Color(0.03f, 0.15f, 0.06f, 0.96f),
            PanelAccent = new Color(0.18f, 0.78f, 0.35f),
            PanelText = Color.white,
            ChatBackground = new Color(0.02f, 0.10f, 0.04f),
            ChatText = new Color(0.60f, 1f, 0.72f),
            MinimapBackground = new Color(0.02f, 0.09f, 0.04f),
            MinimapVent = new Color(0.25f, 0.9f, 0.45f),
            MinimapSabotage = new Color(1f, 0.3f, 0.3f),
            OverlayBackground = new Color(0.02f, 0.12f, 0.05f, 0.88f),
            OverlayText = new Color(0.65f, 1f, 0.75f),
            OverlayAccent = new Color(0.2f, 0.85f, 0.40f),
        });

        // basically pure black
        All.Add(new ThemeData
        {
            Name = "Void",
            PanelBackground = new Color(0.02f, 0.02f, 0.02f, 0.98f),
            PanelAccent = new Color(0.55f, 0.55f, 0.55f),
            PanelText = new Color(0.85f, 0.85f, 0.85f),
            ChatBackground = Color.black,
            ChatText = new Color(0.75f, 0.75f, 0.75f),
            MinimapBackground = Color.black,
            MinimapVent = new Color(0.5f, 0.5f, 0.5f),
            MinimapSabotage = new Color(0.8f, 0.8f, 0.8f),
            OverlayBackground = new Color(0.03f, 0.03f, 0.03f, 0.92f),
            OverlayText = new Color(0.8f, 0.8f, 0.8f),
            OverlayAccent = new Color(0.5f, 0.5f, 0.5f),
        });

        All.Add(new ThemeData
        {
            Name = "Aurora",
            PanelBackground = new Color(0.05f, 0.08f, 0.12f, 0.96f),
            PanelAccent = new Color(0.40f, 0.90f, 0.65f),
            PanelText = Color.white,
            ChatBackground = new Color(0.03f, 0.06f, 0.09f),
            ChatText = new Color(0.55f, 1f, 0.8f),
            MinimapBackground = new Color(0.03f, 0.05f, 0.08f),
            MinimapVent = new Color(0.35f, 0.95f, 0.70f),
            MinimapSabotage = new Color(1f, 0.35f, 0.80f),
            OverlayBackground = new Color(0.04f, 0.07f, 0.11f, 0.88f),
            OverlayText = new Color(0.55f, 1f, 0.8f),
            OverlayAccent = new Color(0.4f, 0.9f, 0.65f),
        });

        All.Add(new ThemeData
        {
            Name = "Rose",
            PanelBackground = new Color(0.20f, 0.05f, 0.10f, 0.96f),
            PanelText = Color.white,
            PanelAccent = new Color(0.95f, 0.45f, 0.65f),
            ChatBackground = new Color(0.14f, 0.03f, 0.07f),
            ChatText = new Color(1f, 0.75f, 0.85f),
            MinimapBackground = new Color(0.12f, 0.03f, 0.06f),
            MinimapVent = new Color(1f, 0.5f, 0.7f),
            MinimapSabotage = new Color(0.9f, 0.9f, 0.2f),
            OverlayBackground = new Color(0.16f, 0.04f, 0.08f, 0.88f),
            OverlayText = new Color(1f, 0.78f, 0.88f),
            OverlayAccent = new Color(0.95f, 0.45f, 0.65f),
        });

        All.Add(new ThemeData
        {
            Name = "Neon",
            PanelBackground = new Color(0.04f, 0.04f, 0.04f, 0.96f),
            PanelAccent = new Color(0.10f, 1f, 0.65f),
            PanelText = new Color(0.10f, 1f, 0.65f),
            ChatBackground = new Color(0.03f, 0.03f, 0.03f),
            ChatText = new Color(0.10f, 1f, 0.65f),
            MinimapBackground = new Color(0.02f, 0.02f, 0.02f),
            MinimapVent = new Color(0.1f, 1f, 0.65f),
            MinimapSabotage = new Color(1f, 0.1f, 0.6f),
            OverlayBackground = new Color(0.04f, 0.04f, 0.04f, 0.90f),
            OverlayText = new Color(0.1f, 1f, 0.65f),
            OverlayAccent = new Color(1f, 0.1f, 0.6f),
        });

        All.Add(new ThemeData
        {
            Name = "Lavender",
            PanelBackground = new Color(0.12f, 0.08f, 0.20f, 0.96f),
            PanelAccent = new Color(0.72f, 0.55f, 0.95f),
            PanelText = Color.white,
            ChatBackground = new Color(0.09f, 0.06f, 0.15f),
            ChatText = new Color(0.85f, 0.75f, 1f),
            MinimapBackground = new Color(0.07f, 0.05f, 0.12f),
            MinimapVent = new Color(0.7f, 0.5f, 0.95f),
            MinimapSabotage = new Color(1f, 0.4f, 0.4f),
            OverlayBackground = new Color(0.10f, 0.07f, 0.17f, 0.88f),
            OverlayText = new Color(0.88f, 0.78f, 1f),
            OverlayAccent = new Color(0.72f, 0.55f, 0.95f),
        });

        Active = All[0];
    }

    public static bool TryApply(string name)
    {
        var match = All.Find(t => t.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        if (match == null) return false;
        Active = match;
        ThemeApplicator.Refresh();
        return true;
    }
}
