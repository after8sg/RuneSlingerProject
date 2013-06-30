
using RuneSlinger.server.Abstract;
using RuneSlinger.Base.Commands;
using NHibernate;
using RuneSlinger.server.Components;

namespace RuneSlinger.server.CommandHandlers
{
    public class RespondToChallengeHandler : ICommandHandler<RespondToChallengeCommand>
    {
        public readonly ISession _database;
        public readonly IApplication _application;

        public RespondToChallengeHandler(ISession database, IApplication application)
        {
            _database = database;
            _application = application;
        }

        public void Handle(INetworkedSession session, CommandContext context, RespondToChallengeCommand command)
        {
            session.Registry.Get<ChallengeComponent>(challenge =>
                {
                    challenge.Respond(command.Response);
                });
        }

        
    }
}
