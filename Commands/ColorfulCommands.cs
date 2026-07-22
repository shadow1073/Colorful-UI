using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using ColorfulUI.Hud;
using ColorfulUI.Settings;
using ColorfulUI.Themes;

namespace ColorfulUI.Commands;

public static class ColorfulCommands
{
    private static ConfigEntry<string>? _prefix;
    private static string Prefix => _prefix?.Value ?? "/";

    private static readonly HashSet<byte> _mutedPlayers = new();

    public static void Init(ConfigFile cfg)
    {
        _prefix = cfg.Bind("Commands", "Prefix", "/", "command prefix char");
    }

    public static void ClearSession()
    {
        _mutedPlayers.Clear();
    }

    public static bool TryHandle(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return false;
        if (!raw.StartsWith(Prefix)) return false;

        var tokens = raw.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var command = tokens[0].ToLowerInvariant();
        var host = AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost;

        switch (command)
        {
            case "/info":
                SendLocal($"ColorfulUI {ColorfulPlugin.Version} - use /themes to see all themes");
                return true;

            case "/themes":
                SendLocal("Themes: " + string.Join(", ", ThemeRegistry.All.ConvertAll(t => t.Name)));
                return true;

            case "/theme":
                if (tokens.Length < 2) { SendLocal("Usage: /theme <name>"); return true; }
                if (ThemeRegistry.TryApply(tokens[1]))
                {
                    ColorfulConfig.ActiveThemeIndex.Value = ThemeRegistry.All.FindIndex(t =>
                        t.Name.Equals(tokens[1], StringComparison.OrdinalIgnoreCase));
                    SendLocal($"theme: {tokens[1]}");
                }
                else
                    SendLocal("unknown theme, try /themes");
                return true;

            case "/hud":
                if (tokens.Length < 2 || !float.TryParse(tokens[1], out var hs))
                { SendLocal("Usage: /hud <0.4-1.5>"); return true; }
                HudScaler.SetScale(hs);
                ColorfulConfig.HudScale.Value = hs;
                SendLocal($"hud scale: {hs:F2}");
                return true;

            case "/mute":
                if (tokens.Length < 2) { SendLocal("Usage: /mute <name|id>"); return true; }
                var muteTarget = FindPlayer(tokens[1]);
                if (muteTarget == null) { SendLocal("player not found"); return true; }
                _mutedPlayers.Add(muteTarget.PlayerId);
                SendLocal($"muted {muteTarget.Data?.PlayerName}");
                return true;

            case "/unmute":
                if (tokens.Length < 2) { SendLocal("Usage: /unmute <name|id>"); return true; }
                var unmuteTarget = FindPlayer(tokens[1]);
                if (unmuteTarget == null) { SendLocal("player not found"); return true; }
                _mutedPlayers.Remove(unmuteTarget.PlayerId);
                SendLocal($"unmuted {unmuteTarget.Data?.PlayerName}");
                return true;

            case "/kick":
                if (!host) { SendLocal("host only"); return true; }
                if (tokens.Length < 2) { SendLocal("Usage: /kick <name|id>"); return true; }
                var kickTarget = FindPlayer(tokens[1]);
                if (kickTarget == null) { SendLocal("player not found"); return true; }
                AmongUsClient.Instance!.KickPlayer(kickTarget.GetClientId(), false);
                SendLocal($"kicked {kickTarget.Data?.PlayerName}");
                return true;

            case "/ban":
                if (!host) { SendLocal("host only"); return true; }
                if (tokens.Length < 2) { SendLocal("Usage: /ban <name|id>"); return true; }
                var banTarget = FindPlayer(tokens[1]);
                if (banTarget == null) { SendLocal("player not found"); return true; }
                AmongUsClient.Instance!.KickPlayer(banTarget.GetClientId(), true);
                SendLocal($"banned {banTarget.Data?.PlayerName}");
                return true;

            case "/title":
                if (!host) { SendLocal("host only"); return true; }
                var title = tokens.Length < 2 ? "" : string.Join(" ", tokens[1..]);
                LobbyTitle.Set(title);
                if (string.IsNullOrEmpty(title))
                    SendLocal("title cleared");
                else
                    SendLocal($"title -> {title}");
                return true;

            case "/reset":
                ThemeRegistry.TryApply("Midnight");
                HudScaler.SetScale(0.90f);
                ColorfulConfig.ActiveThemeIndex.Value = 0;
                ColorfulConfig.HudScale.Value = 0.90f;
                SendLocal("reset");
                return true;

            case "/prefix":
                if (tokens.Length < 2) { SendLocal("Usage: /prefix <char>"); return true; }
                if (_prefix != null) _prefix.Value = tokens[1][0].ToString();
                SendLocal($"prefix: '{tokens[1][0]}'");
                return true;

            case "/players":
                ListPlayers();
                return true;

            //case "/reload":
            //    ThemeRegistry.All.Clear();
            //    ThemeRegistry.Init();
            //    SendLocal($"reloaded {ThemeRegistry.All.Count} themes");
            //    return true;

            default:
                return false;
        }
    }

    public static bool IsMuted(byte playerId) => _mutedPlayers.Contains(playerId);

    private static PlayerControl? FindPlayer(string nameOrId)
    {
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p?.Data == null) continue;
            if (string.Equals(p.Data.PlayerName, nameOrId, StringComparison.OrdinalIgnoreCase)) return p;
            if (byte.TryParse(nameOrId, out var id) && p.PlayerId == id) return p;
        }
        return null;
    }

    private static void ListPlayers()
    {
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p?.Data == null) continue;
            SendLocal($"[{p.PlayerId}] {p.Data.PlayerName} - {(p.Data.IsDead ? "dead" : "alive")}");
        }
    }

    private static void SendLocal(string text)
    {
        if (HudManager.Instance?.Chat == null) return;
        HudManager.Instance.Chat.AddLocalChat(text, "ColorfulUI");
    }
}
