using ColorfulUI.Settings;
using ColorfulUI.Themes;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace ColorfulUI.Menu;

// recolors + resizes the main menu buttons to match whatever theme is active.
// button field names here are a guess based on how MainMenuManager usually
// gets structured in these games (playButton / closetButton / storeButton),
// havent actually confirmed against real Among Us source so this ones a bit
// of a gamble tbh
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
            TintButton(__instance.closetButton, theme.PanelAccent, scale); // closet = inventory, weird internal name but whatever
            TintButton(__instance.storeButton, theme.PanelAccent, scale);
        }
    }

    static void TintButton(Button? btn, Color color, float scale)
    {
        if (btn == null) return;

        var img = btn.GetComponent<Image>();
        if (img != null) img.color = color;

        // hard // this bit is ugly but it works - shrinking the whole rect
        // instead of doing font/padding adjustments separately
        var rt = btn.GetComponent<RectTransform>();
        if (rt != null) rt.localScale = Vector3.one * scale;
    }
}
