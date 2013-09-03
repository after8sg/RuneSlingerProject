
using RuneSlinger.server.Abstract;
using RuneSlinger.server.Components;
using RuneSlinger.Base.Commands;
using RuneSlinger.server.Services;

namespace RuneSlinger.server.CommandHandlers
{
    public class PlaceRuneHandler : ICommandHandler<PlaceRuneCommand>
    {
        private readonly RuneGameService _gameService;

        public PlaceRuneHandler(RuneGameService gameService)
        {
            _gameService = gameService;
        }

        public void Handle(INetworkedSession session, CommandContext context, PlaceRuneCommand command)
        {
            context.SetResponse(new PlaceRuneResponse(_gameService.PlaceRune(session, command.RuneId, command.SlotId)));
        }

    }
}
