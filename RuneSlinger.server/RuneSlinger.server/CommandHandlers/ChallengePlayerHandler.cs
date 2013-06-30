
using RuneSlinger.server.Abstract;
using NHibernate;
using RuneSlinger.Base.Commands;
using RuneSlinger.server.Components;
using System.Linq;

namespace RuneSlinger.server.CommandHandlers
{
    public class ChallengePlayerHandler : ICommandHandler<ChallengePlayerCommand>
    {
        public ISession _database;
        public IApplication _application;

        public ChallengePlayerHandler(ISession database, IApplication application)
        {
            _database = database;
            _application = application;
        }
        
        public void Handle(INetworkedSession session, CommandContext context, ChallengePlayerCommand command)
        {
            var challengeSession = _application.Sessions.Single(s => s.Registry.Get<AuthComponent, uint>(t => t.Id) == command.UserId);

            _application.Registry.Get<LobbyComponent>(lobby =>
                {
                    if (lobby.CreateChallenge(session, challengeSession))
                        return;

                        context.RaiseOperationError("user already challenged");
                });
        }

        
    }
}
