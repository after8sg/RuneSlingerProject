
using UnityEngine;
using RuneSlinger.Base.Events;
using System.Collections.Generic;
using Assets.Code;
using System.Linq;

public class GameResultGO : MonoBehaviour
{
    private GameEndedEvent _event;
    private Dictionary<uint, GameSession> _gameSessions;

    public void Initialise(GameEndedEvent @event, IEnumerable<GameSession> sessions)
    {
        _event = @event;
        _gameSessions = sessions.ToDictionary(k => k.UserId, v => v);
    }

    public void OnGUI()
    {
        GUILayout.BeginVertical();
        {
            GUILayout.Label(string.Format("Game status: {0}", _event.Status));

            if (_event.WinnerUserId != null)
                GUILayout.Label(string.Format("Winner : {0}", _gameSessions[_event.WinnerUserId.Value].Username));

            GUILayout.Label("Scores");

            foreach (var score in _event.Scores)
                GUILayout.Label(string.Format("{0} : {1}", _gameSessions[score.Key].Username, score.Value));
        }
        GUILayout.EndVertical();
    }

}

