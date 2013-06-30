
using RuneSlinger.Base.Abstract;

namespace RuneSlinger.Base.Events
{
    public class ChallengeCreatedEvent : IEvent
    {
        public uint ChallengerUserId { get; private set; }

        public ChallengeCreatedEvent(uint challengerUserId)
        {
            ChallengerUserId = challengerUserId;
        }


        
    }
}
