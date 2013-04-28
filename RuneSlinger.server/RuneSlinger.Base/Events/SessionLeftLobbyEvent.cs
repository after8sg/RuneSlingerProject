using RuneSlinger.Base.Abstract;

namespace RuneSlinger.Base.Events
{
    public class SessionLeftLobbyEvent : IEvent
    {
        public uint UserId { get; private set; }

        public SessionLeftLobbyEvent(uint userId)
        {
            UserId = userId;
        }
    }
}
