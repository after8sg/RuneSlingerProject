
using RuneSlinger.Base.Abstract;

namespace RuneSlinger.Base.Commands
{
    public class SendLobbyMessageCommand : ICommand
    {
        public string Message { get; private set;}
        public SendLobbyMessageCommand(string message)
        {
            Message = message;
        }
    }
}
