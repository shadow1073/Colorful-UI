using ColorfulUI.Settings;
using ColorfulUI.Themes;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace ColorfulUI.Hud;

public class VoteTallyOverlay : MonoBehaviour {
    static readonly UpdateThrottleLite throttle = new(0.25f); //votes dont need instant updates, quarter sec is plenty
    string cachedText = "";
    // int debugCounter = 0; // was using this to test throttle timing, can prob delete but whatever

    public static void Register()
    {
        ClassInjector.RegisterTypeInIl2Cpp<VoteTallyOverlay>();
    }

    void OnGUI()
    {
        if (!ColorfulConfig.ShowVoteTally.Value) return;

        var meeting = MeetingHud.Instance;
        if(meeting == null) return;

        if (throttle.ShouldUpdate())
        {
            // best guess at real field names here (PlayerVoteArea list on
            // MeetingHud, VotedFor per player) - matches the general pattern
            // other client mods use for this, not personally verified. if this
            // breaks on update thats probably why
            int voted = 0; int total = 0;
            foreach (var area in meeting.playerStates)
            {
                total++;
                if (area.VotedFor != byte.MaxValue) voted++;
            }
            cachedText = $"Votes: {voted}/{total}";
        }

        var theme = ThemeRegistry.Active;
        GUI.contentColor = theme.OverlayAccent;
        GUI.Box(new Rect(Screen.width / 2f - 60, 50, 120, 30), cachedText);
        GUI.contentColor = Color.white;
    }
}

// same idea as the other UpdateThrottle class, just didnt want a dependency
// on the Overlays namespace we removed earlier - tiny duplicate, not a big deal
public class UpdateThrottleLite
{
    readonly float interval;
    float next;
    public UpdateThrottleLite(float seconds) { interval = seconds; }
    public bool ShouldUpdate()
    {
        if (Time.unscaledTime < next) return false;
        next = Time.unscaledTime + interval;
        return true;
    }
}
