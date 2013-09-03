
using RuneSlinger.server.ValueObjects;
using RuneSlinger.server.Abstract;
using RuneSlinger.Base.ValueObjects;
using System;
using RuneSlinger.Base.Events;

namespace RuneSlinger.server.Components
{
    public class ChallengeComponent
    {
                public INetworkedSession CurSession { get; private set; }
        public INetworkedSession OtherSession { get; private set; }
        public ChallengeDirection Direction { get; private set; }

        public ChallengeComponent(INetworkedSession curSession,INetworkedSession otherSession,ChallengeDirection direction)
        {
             CurSession = curSession;
            OtherSession = otherSession;
            Direction = direction;
        }
        
        public void Destroy()
        {
            CurSession.Registry.Remove<ChallengeComponent>();
            OtherSession.Registry.Remove<ChallengeComponent>();
        }
    }
}
