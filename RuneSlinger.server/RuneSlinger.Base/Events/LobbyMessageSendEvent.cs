using RuneSlinger.Base.Abstract;

namespace RuneSlinger.Base.Events
{
    public class LobbyMessageSendEvent : IEvent
    {
        public uint UserId { get; private set; }
        public string Message { get; private set; }

        public LobbyMessageSendEvent(uint userId, string message)
        {
            UserId = userId;
            Message = message;
        }

    }
}
