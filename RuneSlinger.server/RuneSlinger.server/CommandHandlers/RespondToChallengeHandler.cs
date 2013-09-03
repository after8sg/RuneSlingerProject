
using RuneSlinger.server.Abstract;
using RuneSlinger.Base.Commands;
using NHibernate;
using RuneSlinger.server.Components;
using RuneSlinger.server.Services;

namespace RuneSlinger.server.CommandHandlers
{
    public class RespondToChallengeHandler : ICommandHandler<RespondToChallengeCommand>
    {
        
        private readonly ChallengeService _challengeService;

        public RespondToChallengeHandler(ChallengeService challengeService)
        {
            _challengeService = challengeService;

        }

        public void Handle(INetworkedSession session, CommandContext context, RespondToChallengeCommand command)
        {
            _challengeService.Respond(session, command.Response);
        }

        
    }
}
