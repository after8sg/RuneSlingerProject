
using RuneSlinger.Base.Abstract;
namespace RuneSlinger.Base.Commands
{
    public class PlaceRuneCommand : ICommand<PlaceRuneResponse>
    {
        public uint RuneId { get; private set; }
        public uint SlotId { get; private set; }

        public PlaceRuneCommand(uint runeId, uint slotId)
        {
            RuneId = runeId;
            SlotId = slotId;
        }

        
    }

    public class PlaceRuneResponse : ICommandResponse
    {
        public bool WasPlaced { get; private set; }
        public PlaceRuneResponse(bool wasPlaced)
        {
            WasPlaced = wasPlaced;
        }

    }
}
