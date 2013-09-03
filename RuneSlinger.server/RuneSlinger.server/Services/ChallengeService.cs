
using RuneSlinger.server.Abstract;
using RuneSlinger.Base.ValueObjects;
using RuneSlinger.server.Components;
using RuneSlinger.server.ValueObjects;
using System;
using System.Linq;
using RuneSlinger.Base.Events;

namespace RuneSlinger.server.Services
{
    public class ChallengeService
    {
        private readonly IEventPublisher _events;
        private readonly IApplication _application;
        private readonly RuneGameService _runeGameService;

        public ChallengeService(IEventPublisher events,IApplication application,RuneGameService runeGameService)
        {
            _events = events;
            _application = application;
            _runeGameService = runeGameService;

        }

        public void Abort(INetworkedSession session)
        {

            session.Registry.Get<ChallengeComponent>(challenge =>
                {
                    challenge.Destroy();
                    _events.Publish(new ChallengeRespondedToEvent(ChallengeResponse.Aborted),challenge.OtherSession);
                }
            );

           
        }

        public void Respond(INetworkedSession responder, ChallengeResponse response)
        {
            
            responder.Registry.Get<ChallengeComponent>(challenge =>
            {
                if (challenge.Direction == ChallengeDirection.Challenged)
                    throw new InvalidOperationException("You cannot respond to a challenge that you created");

                _events.Publish(new ChallengeRespondedToEvent(response),challenge.OtherSession);

                if (response == ChallengeResponse.Rejected)
                {
                    challenge.Destroy();
                }
                else if (response == ChallengeResponse.Accepted)
                {


                    CreateGame(challenge);

                    
                }

            });
        }

        private void CreateGame(ChallengeComponent challenge)
        {
            var sessionInNewGame = new[] { challenge.CurSession, challenge.OtherSession };


            _application.Registry.Get<LobbyComponent>(lobby =>
                {
                    
                    var sessionsJoinedEvent = new SessionJoinedGameEvent(sessionInNewGame.Select(s => s.Auth.Id).ToList());
                    _events.Publish(sessionsJoinedEvent, lobby.Sessions);
                    
                });

            _runeGameService.CreateGame(sessionInNewGame);
            challenge.Destroy();
        }
    }
}
