
using RuneSlinger.server.Abstract;
using NHibernate;
using RuneSlinger.Base.Commands;
using RuneSlinger.server.Components;
using System.Linq;
using RuneSlinger.server.Services;

namespace RuneSlinger.server.CommandHandlers
{
    public class ChallengePlayerHandler : ICommandHandler<ChallengePlayerCommand>
    {
        private readonly IApplication _application;
        private readonly LobbyService _lobby;

        public ChallengePlayerHandler(LobbyService lobby,IApplication application)
        {
            _lobby = lobby;
            _application = application;
        }
        
        public void Handle(INetworkedSession session, CommandContext context, ChallengePlayerCommand command)
        {
            var challengeSession = _application.Sessions.Single(s => s.Auth.Id == command.UserId);
            if (!_lobby.ChallengePlayer(session, challengeSession))
                context.RaiseOperationError("user already challenged");
            
        }

        
    }
}
