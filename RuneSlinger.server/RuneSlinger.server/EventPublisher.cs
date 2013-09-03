
using RuneSlinger.Base.Abstract;
using RuneSlinger.server.Abstract;
using System.Collections.Generic;
using System;
using RuneSlinger.Base;
using Photon.SocketServer;
namespace RuneSlinger.server
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ISerializer _serializer;
        private readonly List<Tuple<INetworkedSession, IEvent>> _events;

        public EventPublisher(ISerializer serializer)
        {
            _serializer = serializer;
            _events = new List<Tuple<INetworkedSession, IEvent>>();
        }
        
        public void Commit()
        {
            foreach (var @event in _events)
            {
                ((PeerBase)@event.Item1).SendEvent(
                new EventData(
                    (byte)RuneEventCode.SendEvent,
                    new Dictionary<byte, object>
                    {
                        {(byte) RuneEventCodeParameter.EventType, @event.Item2.GetType().AssemblyQualifiedName},
                        {(byte) RuneEventCodeParameter.EventBytes,_serializer.Serialize(@event.Item2)}
                    })
                    , new SendParameters { Unreliable = false });
            }
        }

        public void Publish(IEvent @event, INetworkedSession session)
        {
            _events.Add(Tuple.Create(session, @event));
        }

        public void Publish(IEvent @event, IEnumerable<INetworkedSession> sessions)
        {
            foreach (var session in sessions)
                Publish(@event, session);
        }
    }
}
