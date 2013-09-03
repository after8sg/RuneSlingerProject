
using RuneSlinger.Base.Abstract;
namespace RuneSlinger.Base.Events
{
    public class RunePlacedEvent : IEvent
    {
        public uint RuneId { get; private set; }
        public uint SlotId { get; private set; }

        public RunePlacedEvent(uint runeId, uint slotId)
        {
            RuneId = runeId;
            SlotId = slotId;
        }

        
    }
}
