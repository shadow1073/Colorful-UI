using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ColorfulUI;

// persistent notes per player name
// useful for remembering who was good, who was a troll, etc
// stored as name|note, one per line
// TODO: maybe index by friend code instead of name someday
//       right now if someone renames the note wont match, whatever
public static class PlayerNotes
{
    static readonly string File_ = Path.Combine(
        UnityEngine.Application.persistentDataPath,
        "ColorfulUI_PlayerNotes.txt");

    static Dictionary<string, string> _notes = new(StringComparer.OrdinalIgnoreCase);
    static bool _loaded;

    public static void Init() => Load();

    public static void Set(string name, string note)
    {
        EnsureLoaded();
        _notes[Scrub(name)] = Scrub(note);
        Save();
    }

    public static void Delete(string name)
    {
        EnsureLoaded();
        _notes.Remove(Scrub(name));
        Save();
    }

    // returns null if no note
    public static string? Get(string name)
    {
        EnsureLoaded();
        return _notes.TryGetValue(Scrub(name), out var n) ? n : null;
    }

    public static int Count()
    {
        EnsureLoaded();
        return _notes.Count;
    }

    static void EnsureLoaded() { if (!_loaded) Load(); }

    static void Load()
    {
        _notes.Clear();
        _loaded = true;
        if (!File.Exists(File_)) return;
        try
        {
            foreach (var line in File.ReadAllLines(File_, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                int i = line.IndexOf('|');
                if (i < 0) continue;
                _notes[line[..i]] = line[(i + 1)..];
            }
        }
        catch { }
    }

    static void Save()
    {
        try
        {
            var sb = new StringBuilder();
            foreach (var kv in _notes)
                sb.AppendLine($"{kv.Key}|{kv.Value}");
            File.WriteAllText(File_, sb.ToString(), Encoding.UTF8);
        }
        catch { } // whatever
    }

    // pipe would break the file format
    static string Scrub(string s) => (s ?? "").Replace("|", "_").Replace("\n", "").Trim();
}
