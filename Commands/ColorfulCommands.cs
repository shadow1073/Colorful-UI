using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using ColorfulUI.Hud;
using ColorfulUI.Settings;
using ColorfulUI.Themes;

namespace ColorfulUI.Commands;

public static class ColorfulCommands
{
    static ConfigEntry<string>? _prefix;
    static string Prefix => _prefix?.Value ?? "/";

    // session mutes, dont persist on purpose
    static readonly HashSet<byte> _muted = new();

    public static void Init(ConfigFile cfg)
    {
        _prefix = cfg.Bind("Commands", "Prefix", "/", "command prefix");
    }

    public static void ClearSession() => _muted.Clear();

    public static bool TryHandle(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw) || !raw.StartsWith(Prefix)) return false;

        var tokens = raw.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var cmd = tokens[0].ToLower();
        bool host = AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost;

        switch (cmd)
        {
            case "/info":
                Say($"ColorfulUI {ColorfulPlugin.Version} - /themes to list themes");
                return true;

            case "/themes":
                Say("themes: " + string.Join(", ", ThemeRegistry.All.ConvertAll(t => t.Name)));
                return true;

            case "/theme":
                if (tokens.Length < 2) { Say("usage: /theme <name>"); return true; }
                if (ThemeRegistry.TryApply(tokens[1]))
                {
                    ColorfulConfig.ActiveThemeIndex.Value = ThemeRegistry.All.FindIndex(t =>
                        t.Name.Equals(tokens[1], StringComparison.OrdinalIgnoreCase));
                    Say($"theme -> {tokens[1]}");
                }
                else Say("unknown theme, try /themes");
                return true;

            case "/hud":
                if (tokens.Length < 2 || !float.TryParse(tokens[1], out var hs))
                { Say("usage: /hud <0.4-1.5>"); return true; }
                HudScaler.SetScale(hs);
                ColorfulConfig.HudScale.Value = hs;
                Say($"hud -> {hs:F2}");
                return true;

            case "/mute":
                if (tokens.Length < 2) { Say("usage: /mute <name|id>"); return true; }
                var mp = FindPlayer(tokens[1]);
                if (mp == null) { Say("player not found"); return true; }
                _muted.Add(mp.PlayerId);
                Say($"muted {mp.Data?.PlayerName}");
                return true;

            case "/unmute":
                if (tokens.Length < 2) { Say("usage: /unmute <name|id>"); return true; }
                var ump = FindPlayer(tokens[1]);
                if (ump == null) { Say("player not found"); return true; }
                _muted.Remove(ump.PlayerId);
                Say($"unmuted {ump.Data?.PlayerName}");
                return true;

            case "/kick":
                if (!host) { Say("host only"); return true; }
                if (tokens.Length < 2) { Say("usage: /kick <name|id>"); return true; }
                var kp = FindPlayer(tokens[1]);
                if (kp == null) { Say("player not found"); return true; }
                AmongUsClient.Instance!.KickPlayer(kp.OwnerId, false);
                Say($"kicked {kp.Data?.PlayerName}");
                return true;

            case "/ban":
                if (!host) { Say("host only"); return true; }
                if (tokens.Length < 2) { Say("usage: /ban <name|id>"); return true; }
                var bp = FindPlayer(tokens[1]);
                if (bp == null) { Say("player not found"); return true; }
                AmongUsClient.Instance!.KickPlayer(bp.OwnerId, true);
                Say($"banned {bp.Data?.PlayerName}");
                return true;

            // kick with a reason that actually gets saved, unlike /kick
            case "/kickr":
                if (!host) { Say("host only"); return true; }
                if (tokens.Length < 3) { Say("usage: /kickr <name|id> <reason>"); return true; }
                var krp = FindPlayer(tokens[1]);
                if (krp == null) { Say("player not found"); return true; }
                string kreason = string.Join(" ", tokens[2..]);
                AmongUsClient.Instance!.KickPlayer(krp.OwnerId, false);
                KickLogger.Record(krp.Data?.PlayerName ?? "?", (int)(krp.Data?.PlayerLevel ?? 0), $"manual:{kreason}");
                Say($"kicked {krp.Data?.PlayerName} ({kreason})");
                return true;

            case "/title":
                if (!host) { Say("host only"); return true; }
                var t = tokens.Length < 2 ? "" : string.Join(" ", tokens[1..]);
                LobbyTitle.Set(t);
                Say(string.IsNullOrEmpty(t) ? "title cleared" : $"title -> {t}");
                return true;

            case "/reset":
                ThemeRegistry.TryApply("Midnight");
                HudScaler.SetScale(0.90f);
                ColorfulConfig.ActiveThemeIndex.Value = 0;
                ColorfulConfig.HudScale.Value = 0.90f;
                Say("reset to defaults");
                return true;

            case "/prefix":
                if (tokens.Length < 2) { Say("usage: /prefix <char>"); return true; }
                if (_prefix != null) _prefix.Value = tokens[1][0].ToString();
                Say($"prefix -> '{tokens[1][0]}'");
                return true;

            case "/players":
                foreach (var p in PlayerControl.AllPlayerControls)
                {
                    if (p?.Data == null) continue;
                    Say($"[{p.PlayerId}] {p.Data.PlayerName} lv{p.Data.PlayerLevel} {(p.Data.IsDead ? "dead" : "alive")}");
                }
                return true;

            // sweep lobby and kick everyone under level N
            // works even if levelgate auto kick is off
            case "/kicklevel":
                if (!host) { Say("host only"); return true; }
                if (tokens.Length < 2 || !int.TryParse(tokens[1], out int thresh))
                { Say("usage: /kicklevel <N>"); return true; }
                int kicked = 0;
                foreach (var p in PlayerControl.AllPlayerControls)
                {
                    if (p == null || p.Data == null || p == PlayerControl.LocalPlayer) continue;
                    int lvl = (int)p.Data.PlayerLevel;
                    if (lvl < thresh)
                    {
                        AmongUsClient.Instance!.KickPlayer(p.OwnerId, false);
                        KickLogger.Record(p.Data.PlayerName, lvl, $"manual:/kicklevel {thresh}");
                        kicked++;
                    }
                }
                Say(kicked == 0 ? $"nobody under level {thresh}" : $"kicked {kicked} under level {thresh}");
                return true;

            // /history = total count, /history name = that player's records
            case "/history":
                if (tokens.Length < 2)
                {
                    Say($"total kicks logged: {KickLogger.TotalCount()}");
                    return true;
                }
                string hname = string.Join(" ", tokens[1..]);
                var records = KickLogger.GetHistory(hname);
                if (records.Count == 0) { Say($"no history for '{hname}'"); return true; }
                Say($"--- {hname} ({records.Count} kicks) ---");
                foreach (var r in records) Say(r);
                return true;

            // /autostart 0 turns it off
            case "/autostart":
                if (!host) { Say("host only"); return true; }
                if (tokens.Length < 2 || !int.TryParse(tokens[1], out int acount))
                {
                    Say(AutoStart.IsArmed ? $"autostart armed at {AutoStart.Target}" : "autostart off  |  usage: /autostart <N>");
                    return true;
                }
                if (acount <= 0) { AutoStart.Disable(); Say("autostart off"); }
                else { AutoStart.Set(acount); Say($"autostart -> {acount} players"); }
                return true;

            // /note name = read, /note name text = write, /note name clear = delete
            case "/note":
                if (tokens.Length < 2) { Say("usage: /note <name> [text|clear]"); return true; }
                string nname = tokens[1];
                if (tokens.Length == 2)
                {
                    var existing = PlayerNotes.Get(nname);
                    Say(existing == null ? $"no note for '{nname}'" : $"[{nname}] {existing}");
                    return true;
                }
                string narg = string.Join(" ", tokens[2..]);
                if (narg.Equals("clear", StringComparison.OrdinalIgnoreCase))
                { PlayerNotes.Delete(nname); Say($"note for '{nname}' deleted"); }
                else
                { PlayerNotes.Set(nname, narg); Say($"note saved for '{nname}'"); }
                return true;

            // blast a message to the whole lobby
            case "/announce":
                if (!host) { Say("host only"); return true; }
                if (tokens.Length < 2) { Say("usage: /announce <text>"); return true; }
                string ann = string.Join(" ", tokens[1..]);
                HudManager.Instance?.Chat?.AddChat(PlayerControl.LocalPlayer, $"[!] {ann}", true);
                return true;

            // /filter add, remove, list
            case "/filter":
                if (!host) { Say("host only"); return true; }
                if (tokens.Length < 2) { Say("usage: /filter add|remove|list [word]"); return true; }
                switch (tokens[1].ToLower())
                {
                    case "add":
                        if (tokens.Length < 3) { Say("usage: /filter add <word>"); break; }
                        WordFilter.Add(string.Join(" ", tokens[2..]));
                        Say($"filter: added '{string.Join(" ", tokens[2..])}'");
                        break;
                    case "remove":
                    case "rm":
                        if (tokens.Length < 3) { Say("usage: /filter remove <word>"); break; }
                        bool rm = WordFilter.Remove(string.Join(" ", tokens[2..]));
                        Say(rm ? $"filter: removed '{string.Join(" ", tokens[2..])}'" : "not in filter");
                        break;
                    case "list":
                        var all = WordFilter.All();
                        Say(all.Count == 0 ? "filter is empty" : $"filter: {string.Join(", ", all)}");
                        break;
                    default:
                        Say("usage: /filter add|remove|list [word]");
                        break;
                }
                return true;

            default:
                return false;
        }
    }

    public static bool IsMuted(byte id) => _muted.Contains(id);

    static PlayerControl? FindPlayer(string nameOrId)
    {
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p?.Data == null) continue;
            if (string.Equals(p.Data.PlayerName, nameOrId, StringComparison.OrdinalIgnoreCase)) return p;
            if (byte.TryParse(nameOrId, out var id) && p.PlayerId == id) return p;
        }
        return null;
    }

    // feedback messages, only local player sees these
    // AddLocalChat was removed in 2025.10.14 so we use AddChat with false
    static void Say(string text)
    {
        if (HudManager.Instance?.Chat == null) return;
        HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"[ColorfulUI] {text}", false);
    }
}
