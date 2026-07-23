using HarmonyLib;
using TMPro;

namespace ColorfulUI;

public static class LobbyTitle
{
    private static string _title = "";
    private static string _originalText = "";   // remembered on first apply so we can restore it

    public static void Set(string title)
    {
        _title = title ?? "";
        Apply();
    }

    private static TextMeshPro? FindLabel()
    {
        if (GameStartManager.Instance == null) return null;

        foreach (var tmp in GameStartManager.Instance.GetComponentsInChildren<TextMeshPro>(true))
        {
            var n = tmp.gameObject.name.ToLowerInvariant();
            if (n.Contains("room") || n.Contains("code") || n.Contains("name"))
                return tmp;
        }
        return null;
    }

    private static void Apply()
    {
        var label = FindLabel();
        if (label == null) return;

        // grab the game's original text the first time we see the label
        if (string.IsNullOrEmpty(_originalText))
            _originalText = label.text;

        label.text = string.IsNullOrEmpty(_title) ? _originalText : _title;
    }

    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
    private static class StartPatch
    {
        // reset cached original each lobby so we dont carry stale text across games
        static void Postfix()
        {
            _originalText = "";
            Apply();
        }
    }

    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
    private static class UpdatePatch
    {
        static void Postfix()
        {
            // no custom title set, nothing to override - skip the child scan
            if (string.IsNullOrEmpty(_title)) return;
            Apply();
        }
    }
}
