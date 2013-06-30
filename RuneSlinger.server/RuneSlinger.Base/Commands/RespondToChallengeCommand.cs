
using RuneSlinger.Base.Abstract;
using RuneSlinger.Base.ValueObjects;

namespace RuneSlinger.Base.Commands
{
    public class RespondToChallengeCommand : ICommand
    {
        public ChallengeResponse Response { get; private set; }
        public RespondToChallengeCommand(ChallengeResponse response)
        {
            Response = response;
        }

    }
}
