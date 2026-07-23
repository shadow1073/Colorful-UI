using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ColorfulUI;

// logs kicks to disk so i can actually remember who i kicked and why lol 
public static class KickLogger
{
    // persistentDataPath is where among us saves stuff on android, should be fine
    static readonly string LogPath = Path.Combine(
        UnityEngine.Application.persistentDataPath,
        "ColorfulUI_KickHistory.txt");

    public static void Record(string name, int level, string reason)
    {
        try
        {
            // timestamp|name|level|reason
            var line = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}|{Clean(name)}|{level}|{Clean(reason)}";
            File.AppendAllText(LogPath, line + "\n", Encoding.UTF8);
        }
        catch
        {
            // dont crash the host over a log write, thats stupid
        }
    }

    public static int TotalCount()
    {
        return ReadAll().Count;
    }

    // returns list of formatted strings for the /history command
    public static List<string> GetHistory(string name)
    {
        var out_ = new List<string>();
        foreach (var r in ReadAll())
        {
            if (string.Equals(r.name, name, StringComparison.OrdinalIgnoreCase))
                out_.Add($"{r.ts}  lv{r.lvl}  {r.reason}");
        }
        return out_;
    }

    // yeah this is a tuple deal with it
    static List<(string ts, string name, int lvl, string reason)> ReadAll()
    {
        var list = new List<(string, string, int, string)>();
        if (!File.Exists(LogPath)) return list;
        try
        {
            foreach (var line in File.ReadAllLines(LogPath, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var p = line.Split('|');
                if (p.Length < 4) continue;
                int.TryParse(p[2], out int lvl);
                list.Add((p[0], p[1], lvl, p[3]));
            }
        }
        catch { }
        return list;
    }

    // strip pipes so a player named "foo|auto" doesnt corrupt the file lmao
    static string Clean(string s) => (s ?? "unknown").Replace("|", "_").Replace("\n", "").Replace("\r", "");
}
