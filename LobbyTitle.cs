using HarmonyLib;
using TMPro;

namespace ColorfulUI;

public static class LobbyTitle
{
    private static string _title = "";

    public static void Set(string title)
    {
        _title = title ?? "";
        Apply();
    }

    private static void Apply()
    {
        if (GameStartManager.Instance == null) return;

        // GameRoomName field was renamed/removed in 2025.10.14 so we find
        // the label by searching children for a TMP with "room" or "code" in
        // the name - fragile but the best we can do without the actual field
        TextMeshPro? label = null;
        foreach (var tmp in GameStartManager.Instance.GetComponentsInChildren<TextMeshPro>(true))
        {
            var n = tmp.gameObject.name.ToLowerInvariant();
            if (n.Contains("room") || n.Contains("code") || n.Contains("name"))
            {
                label = tmp;
                break;
            }
        }
        if (label == null) return;

        label.text = string.IsNullOrEmpty(_title)
            ? GameCode.IntToGameName(AmongUsClient.Instance.GameId)
            : _title;
    }

    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
    private static class StartPatch
    {
        static void Postfix() => Apply();
    }

    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
    private static class UpdatePatch
    {
        static void Postfix() => Apply();
    }
}
