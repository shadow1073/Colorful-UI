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

        var label = GameStartManager.Instance.GameRoomName;
        if (label == null) return;

        label.text = string.IsNullOrEmpty(_title)
            ? GameManager.Instance?.LogicOptions?.GetGameCode() ?? ""
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
