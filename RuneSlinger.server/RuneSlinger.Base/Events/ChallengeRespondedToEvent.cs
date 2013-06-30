
using RuneSlinger.Base.Abstract;
using RuneSlinger.Base.ValueObjects;

namespace RuneSlinger.Base.Events
{
    public class ChallengeRespondedToEvent : IEvent
    {
        public ChallengeResponse Response { get; private set; }

        public ChallengeRespondedToEvent(ChallengeResponse response)
        {
            Response = response;
        }
    }
}
