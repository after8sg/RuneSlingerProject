
using RuneSlinger.server.Abstract;
using RuneSlinger.server.Components;
using RuneSlinger.server.Services;
using RuneSlinger.server.Command;
using RuneSlinger.Base.Events;

namespace RuneSlinger.server.CommandHandlers
{
    public class DisconnectHandler : ICommandHandler<DisconnectCommand>
    {
        private readonly IApplication _application;
        private readonly IEventPublisher _events;
        private readonly ChallengeService _challengeService;

        public DisconnectHandler(IApplication application,IEventPublisher events,ChallengeService challengeService)
        {
            _application = application;
            _events = events;
            _challengeService = challengeService;
        }

        public void Handle(INetworkedSession session, CommandContext context, DisconnectCommand command)
        {
            _application.Registry.Get<LobbyComponent>(lobby =>
                {
                    if (!lobby.Contains(session))
                        return;

                    lobby.Leave(session);
                    _events.Publish(new SessionLeftLobbyEvent(session.Auth.Id), lobby.Sessions);
                    
                    session.Registry.TryGet<ChallengeComponent>(challenge =>
                    {
                        _challengeService.Abort(session);
                    });
                });
        }

    }
}
