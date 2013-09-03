
using RuneSlinger.Base.Abstract;
using System.Collections.Generic;
using RuneSlinger.Base.ValueObjects;
namespace RuneSlinger.Base.Events
{
    public class GameRuneSlotEstablishedEvent : IEvent
    {
        public IEnumerable<RuneSlot> Slots { get; private set; }

        public GameRuneSlotEstablishedEvent(IEnumerable<RuneSlot> slots)
        {
            Slots = slots;
        }


        
    }
}
