using ColorfulUI.Settings;
using ColorfulUI.Themes;
using HarmonyLib;
using UnityEngine;

namespace ColorfulUI.Menu;

public static class MainMenuThemePatch
{
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    private static class MainMenuStartPatch
    {
        static void Postfix(MainMenuManager __instance)
        {
            if (!ColorfulConfig.MainMenuTheme.Value) return;

            var theme = ThemeRegistry.Active;
            float scale = ColorfulConfig.MainMenuButtonScale.Value;

            TintButton(__instance.playButton, theme.PanelAccent, scale);
        }
    }

    static void TintButton(PassiveButton? btn, Color color, float scale)
    {
        if (btn == null) return;
        
        foreach (var sr in btn.GetComponentsInChildren<SpriteRenderer>(true))
        {
            if (sr.color == Color.white)
                sr.color = color;
        }

        var rt = btn.GetComponent<RectTransform>();
        if (rt != null) rt.localScale = Vector3.one * scale;
    }
}
