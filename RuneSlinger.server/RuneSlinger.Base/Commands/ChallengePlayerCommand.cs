
using RuneSlinger.Base.Abstract;

namespace RuneSlinger.Base.Commands
{
    public class ChallengePlayerCommand : ICommand
    {
        public uint UserId { get; private set; }
        public ChallengePlayerCommand(uint userid)
        {
            UserId = userid;
        }
    }
}
