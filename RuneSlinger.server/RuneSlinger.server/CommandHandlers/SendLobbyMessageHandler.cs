using RuneSlinger.server.Abstract;
using RuneSlinger.server.Components;
using RuneSlinger.Base.Commands;
using RuneSlinger.server.Services;

namespace RuneSlinger.server.CommandHandlers
{
    public class SendLobbyMessageHandler : ICommandHandler<SendLobbyMessageCommand>
    {
        private readonly LobbyService _lobby;

        public SendLobbyMessageHandler(LobbyService lobby)
        {
            _lobby = lobby;

        }

        public void Handle(INetworkedSession session, CommandContext context, SendLobbyMessageCommand command)
        {
            _lobby.SendMessage(session, command.Message);
            
        }
    }
}
