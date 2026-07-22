using ColorfulUI.Commands;
using HarmonyLib;

namespace ColorfulUI.Patches;

[HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
public static class ChatSendPatch
{
    static bool Prefix(ChatController __instance)
    {
        var text = __instance.freeChatField?.Text;
        if (string.IsNullOrEmpty(text)) return true;

        if (ColorfulCommands.TryHandle(text))
        {
            __instance.freeChatField.Clear();
            return false;
        }

        return true;
    }
}

[HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
public static class ChatReceivePatch
{
    static bool Prefix(PlayerControl sourcePlayer)
    {
        if (sourcePlayer == null) return true;
        return !ColorfulCommands.IsMuted(sourcePlayer.PlayerId);
    }
}
