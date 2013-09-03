
using System.Collections.Generic;
using RuneSlinger.server.Abstract;
using RuneSlinger.Base.Abstract;
using RuneSlinger.Base.ValueObjects;
using RuneSlinger.Base.Events;
using System.Linq;
using System;

namespace RuneSlinger.server.Components
{
    public class RuneGame
    {
        public uint BoardWidth { get; private set; }
        public uint BoardHeight { get; private set; }
        public IEnumerable<ServerRune> Runes { get { return _runes.Values; } }
        public IEnumerable<ServerSlot> Slots { get { return _runeSlots.Values; } }
        public IEnumerable<INetworkedSession> Sessions { get { return Sessions; } }

        private readonly IEnumerable<INetworkedSession> _sessions;

        private readonly Dictionary<uint, ServerRune> _runes;
        private readonly Dictionary<uint, ServerSlot> _runeSlots;
        private bool _isRunning;

        public RuneGame(IEnumerable<INetworkedSession> sessions)
        {
            _sessions = sessions;
            _runes = new Dictionary<uint, ServerRune>();
            _runeSlots = new Dictionary<uint, ServerSlot>();
            Initialise();
        }

        private void Initialise()
        {

            BoardWidth = 2;
            BoardHeight = 2;

            
            
                        
            uint slotTypeIndex = 0;
            for (uint x = 0; x < BoardWidth; x++)
            {
                for (uint y = 0; y < BoardHeight; y++)
                {
                    var runeIndex = slotTypeIndex++ % 9;

                    _runeSlots.Add(slotTypeIndex, new ServerSlot(new RuneSlot(slotTypeIndex, RuneType.Numeric, runeIndex)));
                    _runes.Add(slotTypeIndex,new ServerRune(new Rune(slotTypeIndex,RuneType.Numeric,runeIndex)));

                    
                }
            }

            

            _isRunning = true;
        }

        //private void PublishToAll(IEvent @event)
        //{
            
        //    foreach (var session in _sessions)
        //        session.Publish(@event);
        //}

        public bool PlaceRune(INetworkedSession session, uint runeId, uint slotId)
        {
            if (!_isRunning)
                throw new InvalidOperationException();

            var rune = _runes[runeId];
            var slot = _runeSlots[slotId];

            if (rune.PlacedBy != null || slot.PlacedBy != null)
                throw new InvalidOperationException();

            if (rune.Rune.RuneTypeIndex != slot.Slot.RuneTypeIndex || rune.Rune.Type != slot.Slot.Type)
                return false;

            rune.PlacedBy = session;
            slot.PlacedBy = session;
            

            //var isGameOver = _runes.All(t => t.Value.PlacedBy != null);
            //if (isGameOver)
            //{
            //    _isRunning = false;

            //    var placeRunesBySession = _runes.GroupBy(b => b.Value.PlacedBy).OrderByDescending(b => b.Count());
            //    var winner = placeRunesBySession.First().Key;
            //    var wasDraw = placeRunesBySession.All(t => t.Count() == placeRunesBySession.First().Count());
            //    var scores = placeRunesBySession.ToDictionary(
            //        t => t.Key.Auth.Id,
            //        t => (uint)t.Count());

            //    if (wasDraw)
            //        PublishToAll(new GameEndedEvent(GameEndedStatus.Draw, null,scores));
            //    else
            //        PublishToAll(new GameEndedEvent(GameEndedStatus.PlayerWon, winner.Auth.Id,scores
            //            ));
            //}

            return true;

        }
    }
}
