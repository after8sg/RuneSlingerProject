
using RuneSlinger.Base.Abstract;
using System.Collections.Generic;

namespace RuneSlinger.Base.Events
{
    public class SessionJoinedGameEvent : IEvent
    {
        public IEnumerable<uint> UserIds { get; private set; }

        public SessionJoinedGameEvent(IEnumerable<uint> userIds)
        {
            UserIds = userIds;
        }

    }
}
