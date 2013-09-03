
using RuneSlinger.Base.Abstract;
using RuneSlinger.Base.ValueObjects;
using System.Collections.Generic;
namespace RuneSlinger.Base.Events
{
    public class GameEndedEvent : IEvent
    {
        public GameEndedStatus Status { get; private set; }
        public uint? WinnerUserId { get; private set; }
        public Dictionary<uint, uint> Scores { get; private set; }

        public GameEndedEvent(GameEndedStatus status, uint? winnerUserId, Dictionary<uint,uint> scores)
        {
            Status = status;
            WinnerUserId = winnerUserId;
            Scores = scores;
        }




        
    }
}
