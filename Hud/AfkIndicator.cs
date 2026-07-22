using System.Collections.Generic;
using ColorfulUI.Settings;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace ColorfulUI.Hud;

public class AfkIndicator : MonoBehaviour
{
    const float AFK_THRESHOLD = 20f; // seconds. 20 felt right in testing, could prob be higher tbh

    static readonly UpdateThrottleLite throttle = new(1f); // checking positions every frame is pointless

    class Tracked {
        public Vector2 lastPos;
        public float lastMoveTime;
    }

    Dictionary<byte, Tracked> tracked = new Dictionary<byte, Tracked>();

    public static void Register()
    {
        ClassInjector.RegisterTypeInIl2Cpp<AfkIndicator>();
    }

    void Update()
    {
        if (!throttle.ShouldUpdate()) return;
        if (GameData.Instance == null) return;

        foreach (var player in GameData.Instance.AllPlayers)
        {
            if (player == null || player.IsDead) continue;
            var pc = player.Object;
            if (pc == null) continue;

            Vector2 pos = pc.transform.position;

            if (!tracked.ContainsKey(player.PlayerId))
            {
                tracked[player.PlayerId] = new Tracked { lastPos = pos, lastMoveTime = Time.unscaledTime };
                continue;
            }

            var t = tracked[player.PlayerId];
            // moved more than a hair since last check - reset the timer
            if ((pos - t.lastPos).sqrMagnitude > 0.01f) {
                t.lastPos = pos;
                t.lastMoveTime = Time.unscaledTime;
            }
        }
    }

    void OnGUI()
    {
        if (!ColorfulConfig.ShowAfkBadges.Value) return;
        if (GameData.Instance == null) return;

        foreach (var player in GameData.Instance.AllPlayers)
        {
            if (player == null) continue;
            if (player.IsDead) continue;
            if (!tracked.TryGetValue(player.PlayerId, out var t)) continue;
            if (Time.unscaledTime - t.lastMoveTime < AFK_THRESHOLD) continue;

            var pc = player.Object;
            if (pc == null) continue;

            // world to screen - camera reference is a guess at how the real game
            // exposes its main camera, didnt actually test this part yet
            Vector3 screenPos = Camera.main.WorldToScreenPoint(pc.transform.position + Vector3.up * 0.5f);
            screenPos.y = Screen.height - screenPos.y;

            GUI.contentColor = Color.yellow;
            GUI.Label(new Rect(screenPos.x - 15, screenPos.y - 15, 30, 20), "AFK");
            GUI.contentColor = Color.white;
        }
    }
}
