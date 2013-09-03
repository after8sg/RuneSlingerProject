
using RuneSlinger.server.Abstract;
using System;
using RuneSlinger.server.Components;
using RuneSlinger.Base.Events;
using RuneSlinger.Base.ValueObjects;
using System.Linq;
using RuneSlinger.server.ValueObjects;

namespace RuneSlinger.server.Services
{
    public class LobbyService
    {
        private readonly IApplication _application;
        private readonly IEventPublisher _events;

        public LobbyService(IApplication application,IEventPublisher events)
        {
            _application = application;
            _events = events;
        }

        public void Join(INetworkedSession session)
        {
            if (!session.Auth.IsAuthenticated)
                throw new InvalidOperationException();

            _application.Registry.Get<LobbyComponent>(lobby =>
                {
                    if (lobby.Contains(session))
                        throw new InvalidOperationException();

                    _events.Publish(new JoinLobbyEvent(lobby.Sessions.Select(t => new LobbySession(t.Auth.Id, t.Auth.Username)).ToList()), session);

                    lobby.Join(session);

                    var lobbySession = new LobbySession(session.Auth.Id, session.Auth.Username);

                    _events.Publish(new SessionJoinedLobbyEvent(lobbySession), lobby.Sessions);

                });
        }


        public void SendMessage(INetworkedSession session, string message)
        {
            _application.Registry.Get<LobbyComponent>(lobby =>
            {
                if (!lobby.Contains(session))
                    throw new InvalidOperationException("Cannot send a lobby message if you are not in the lobby");


                foreach (var existingSession in lobby.Sessions.Where(t => t != session))
                    _events.Publish(new LobbyMessageSendEvent(session.Auth.Id, message), existingSession);
            });

            
        }

        public bool ChallengePlayer(INetworkedSession session, INetworkedSession challengedSession)
        {
            var ok = false;

            _application.Registry.Get<LobbyComponent>(lobby =>
            {
                if (!lobby.IsChallengeValid(session, challengedSession))
                    return;

                session.Registry.Set(new ChallengeComponent( session, challengedSession, ChallengeDirection.Challenged));
                challengedSession.Registry.Set(new ChallengeComponent(challengedSession, session, ChallengeDirection.Challenger));

            
                _events.Publish(new ChallengeCreatedEvent(session.Auth.Id),challengedSession);
                ok = true;
            });

            return ok;
        }
    }
}

