
using System.Collections.Generic;
using RuneSlinger.server.Abstract;

namespace RuneSlinger.server.Components
{
    public class RuneGame
    {
        private readonly IEnumerable<INetworkedSession> _sessions;

        public RuneGame(IEnumerable<INetworkedSession> sessions)
        {
            _sessions = sessions;
        }

        
    }
}
