
using RuneSlinger.server.Abstract;
using System.Collections;
using System.Collections.Generic;
using RuneSlinger.server.Components;
using RuneSlinger.Base.Events;
using System.Linq;

namespace RuneSlinger.server.Services
{
    public class RuneGameService
    {
        private readonly IEventPublisher _events;

        public RuneGameService(IEventPublisher events)
        {
            _events = events;
        }

        public bool PlaceRune(INetworkedSession session, uint runeId, uint slotId)
        {
            var wasPlaced = false;

            session.Registry.Get<RuneGame>(game =>
                {
                    if (!game.PlaceRune(session, runeId, slotId))
                        return;

                        _events.Publish(new RunePlacedEvent(runeId, slotId), game.Sessions.Where(t => t != session));
                        wasPlaced = true;
                });

            return wasPlaced;
        }

        public void CreateGame(IEnumerable<INetworkedSession> sessions)
        {
            //create new rune game here after publish the above event
            var game = new RuneGame(sessions);

            foreach (var session in sessions)
                session.Registry.Set(game);

            _events.Publish(new GameParametersEstablishedEvent(game.BoardWidth, game.BoardHeight),game.Sessions);

            _events.Publish(new GameRuneSlotEstablishedEvent(game.Slots.Select(t => t.Slot)), game.Sessions);
            _events.Publish(new GameRuneEstablishedEvent(game.Runes.Select(t => t.Rune)), game.Sessions);
            _events.Publish(new GameStartedEvent(), game.Sessions);
        }

    }
}
