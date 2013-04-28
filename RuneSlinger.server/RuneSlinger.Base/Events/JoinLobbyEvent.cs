using System.Collections.Generic;
using RuneSlinger.Base.Abstract;
using RuneSlinger.Base.ValueObjects;

namespace RuneSlinger.Base.Events
{
    public class JoinLobbyEvent : IEvent
    {
        
        public IEnumerable<LobbySession> Sessions {get;set;}

        public JoinLobbyEvent(IEnumerable<LobbySession> session)
        {
            Sessions = session;
        }

    }
}
