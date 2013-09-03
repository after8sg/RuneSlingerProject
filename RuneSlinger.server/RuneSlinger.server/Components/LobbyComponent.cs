
using RuneSlinger.server.Abstract;
using System.Collections.Generic;
using System.Linq;
using RuneSlinger.Base.Events;
using RuneSlinger.Base.ValueObjects;
using System;
using RuneSlinger.server.ValueObjects;

namespace RuneSlinger.server.Components
{
    public class LobbyComponent
    {
        private readonly HashSet<INetworkedSession> _sessions;
        
        public IEnumerable<INetworkedSession> Sessions { get { return _sessions; } }

        public LobbyComponent()
        {
            _sessions = new HashSet<INetworkedSession>();
            
            //_sessionInGame = new HashSet<INetworkedSession>();
        }

        //public void LeaveGame(INetworkedSession session)
        //{
        //    if (!_sessionInGame.Remove(session))
        //        throw new InvalidOperationException("You are not in a game anymore");
        //}

        public bool Contains(INetworkedSession session)
        {
            return _sessions.Contains(session);
        }

        public void Join(INetworkedSession session)
        {
            
            _sessions.Add(session);

            
        }

        public void Leave(INetworkedSession session)
        {
            if (!_sessions.Remove(session))
                throw new OperationException("You were not in the lobby to begin with");

           
        }

        
        public bool IsChallengeValid(INetworkedSession challenger,INetworkedSession challenged)
        {
           return !challenger.Registry.Has<RuneGame>() &&
               !challenged.Registry.Has<RuneGame>() &&
                !challenged.Registry.Has<ChallengeComponent>() &&
                !challenger.Registry.Has<ChallengeComponent>();
        }

        

    }
}
