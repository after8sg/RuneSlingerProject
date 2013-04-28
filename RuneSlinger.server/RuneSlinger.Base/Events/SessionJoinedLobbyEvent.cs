
using RuneSlinger.Base.Abstract;
using RuneSlinger.Base.ValueObjects;

namespace RuneSlinger.Base.Events
{
    public class SessionJoinedLobbyEvent : IEvent
    {
        public LobbySession Session {get;private set;}
        public SessionJoinedLobbyEvent(LobbySession session)
        {
            Session = session;
        }
    }
}
