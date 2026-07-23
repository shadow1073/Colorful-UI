using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HarmonyLib;

namespace ColorfulUI;

// host word filter - auto kicks players who say banned words in chat I don't think no one will use this tho
// words stored in ColorfulUI_WordFilter.txt, one per line
// kicks get logged to kick history too
public static class WordFilter
{
    static readonly string File_ = Path.Combine(
        UnityEngine.Application.persistentDataPath,
        "ColorfulUI_WordFilter.txt");

    static readonly HashSet<string> _words = new(StringComparer.OrdinalIgnoreCase);
    static bool _loaded;

    public static void Init() => Load();

    public static bool Add(string w)
    {
        EnsureLoaded();
        bool added = _words.Add(w.Trim().ToLower());
        if (added) Save();
        return added;
    }

    public static bool Remove(string w)
    {
        EnsureLoaded();
        bool removed = _words.Remove(w.Trim().ToLower());
        if (removed) Save();
        return removed;
    }

    public static IReadOnlyCollection<string> All()
    {
        EnsureLoaded();
        return _words;
    }

    // returns first bad word found or null
    public static string? Check(string msg)
    {
        EnsureLoaded();
        if (string.IsNullOrWhiteSpace(msg)) return null;
        string lower = msg.ToLower();
        foreach (var w in _words)
            if (lower.Contains(w)) return w;
        return null;
    }

    [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
    static class ChatPatch
    {
        static void Prefix(PlayerControl sourcePlayer, string chatText)
        {
            if (AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost) return;
            if (sourcePlayer == null || sourcePlayer == PlayerControl.LocalPlayer) return;

            var bad = Check(chatText);
            if (bad == null) return;

            AmongUsClient.Instance.KickPlayer(sourcePlayer.OwnerId, false);
            KickLogger.Record(
                sourcePlayer.Data?.PlayerName ?? "unknown",
                (int)(sourcePlayer.Data?.PlayerLevel ?? 0),
                $"word-filter:{bad}");
        }
    }

    static void EnsureLoaded() { if (!_loaded) Load(); }

    static void Load()
    {
        _words.Clear();
        _loaded = true;
        if (!File.Exists(File_)) return;
        try
        {
            foreach (var line in File.ReadAllLines(File_, Encoding.UTF8))
            {
                var w = line.Trim().ToLower();
                if (!string.IsNullOrEmpty(w)) _words.Add(w);
            }
        }
        catch { }
    }

    static void Save()
    {
        try { File.WriteAllText(File_, string.Join("\n", _words) + "\n", Encoding.UTF8); }
        catch { }
    }
}
