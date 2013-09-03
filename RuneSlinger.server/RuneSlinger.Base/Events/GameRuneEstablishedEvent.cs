
using RuneSlinger.Base.Abstract;
using System.Collections.Generic;
using RuneSlinger.Base.ValueObjects;

namespace RuneSlinger.Base.Events
{
    public class GameRuneEstablishedEvent : IEvent
    {
        public IEnumerable<Rune> Runes { get; private set; }

        public GameRuneEstablishedEvent(IEnumerable<Rune> runes)
        {
            Runes = runes;
        }


        
    }
}
