using RuneSlinger.server.Abstract;
using RuneSlinger.server.Components;
using RuneSlinger.Base.Commands;

namespace RuneSlinger.server.CommandHandlers
{
    public class SendLobbyMessageHandler : ICommandHandler<SendLobbyMessageCommand>
    {
        private readonly IApplication _application;

        public SendLobbyMessageHandler(IApplication application)
        {
            _application = application;
        }

        public void Handle(INetworkedSession session, CommandContext context, SendLobbyMessageCommand command)
        {
            _application.Registry.Get<LobbyComponent>(lobby => lobby.SendMessage(session, command.Message));
        }
    }
}
