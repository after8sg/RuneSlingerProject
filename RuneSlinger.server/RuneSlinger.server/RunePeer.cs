//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System;
using RuneSlinger.Base;
using NHibernate.Linq;
using RuneSlinger.server.Entities;
using RuneSlinger.server.ValueObjects;

namespace RuneSlinger.server
{
    public class RunePeer : PeerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RunePeer));
        private readonly Application _application;

        public RunePeer(Application application,InitRequest initRequest) : base(initRequest.Protocol,initRequest.PhotonPeer)
        {
            _application = application;
            log.InfoFormat("Peer created at {0}:{1}", initRequest.RemoteIP, initRequest.RemotePort);

            //SendEvent(new EventData(
            //    0, 
            //    new Dictionary<byte, object> 
            //    {
            //        {0,"Peer created"} 
            //    }), 
            //    new SendParameters 
            //    { 
            //        Unreliable = false 
            //    });
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            //invoke when client tell server to do something
            //if (operationRequest.OperationCode != 0)
            //{
            //    log.WarnFormat("Peer sent unknown opcode: {0}", operationRequest.OperationCode);
            //    return;
            //}
            //var message = (string)operationRequest.Parameters[0];
            //log.DebugFormat("Get message from client: {0}", message);

            //var eventData = new EventData(0, new Dictionary<byte, object> { { 0, message } });
            //var parameters = new SendParameters { Unreliable = false };
            //foreach (var peer in _application.Peers.Where(t => t != this))
            //{
            //    peer.SendEvent(eventData,parameters);
            //}


            //user registration
            // user authentication
            // sending a message
            using (var session = _application.OpenSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    try
                    {
                        var opCode = (RuneOperationCode)operationRequest.OperationCode;
                        if (opCode == RuneOperationCode.Register)
                        {
                            var username = (string)operationRequest.Parameters[(byte)RuneOperationCodeParameter.Username];
                            var password = (string)operationRequest.Parameters[(byte)RuneOperationCodeParameter.Password];
                            var email = (string)operationRequest.Parameters[(byte)RuneOperationCodeParameter.Email];
                            Register(session, username, password, email);
                        }
                        else if (opCode == RuneOperationCode.Login)
                        {
                            var password = (string)operationRequest.Parameters[(byte)RuneOperationCodeParameter.Password];
                            var email = (string)operationRequest.Parameters[(byte)RuneOperationCodeParameter.Email];
                            Login(session, password, email);
                        }
                        else if (opCode == RuneOperationCode.SendMessage)
                        {
                            var message = (string)operationRequest.Parameters[(byte)RuneOperationCodeParameter.Message];
                            SendMessage(session, message);
                        }
                        else
                        {
                            SendOperationResponse(new OperationResponse((byte)RuneOperationResponse.Invalid), sendParameters);
                        }

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        SendOperationResponse(new OperationResponse((byte)RuneOperationResponse.FatalError), sendParameters);
                        trans.Rollback();
                        log.ErrorFormat("Error processing operation {0} : {1}", operationRequest.OperationCode, ex);
                    }
                }
            }

            

        }

        private void SendMessage(NHibernate.ISession session, string message)
        {
            
        }

        private void Login(NHibernate.ISession session, string password, string email)
        {
            var user = session.Query<User>().SingleOrDefault(s => s.Email == email);
            if (user == null || !user.Password.EqualsPlainText(password))
            {
                SendError("Email or password is incorrect");
                return;
            }

            SendSuccess();

        }

        private void Register(NHibernate.ISession session, string username, string password, string email)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
            {
                SendError("All fields are required");
                return;
            }

            if (username.Length > 128)
            {
                SendError("Username must be less than 128 characters long");
                return;
            }

            if (email.Length > 200)
            {
                SendError("Email must be less than 200 characters long");
                return;
            }

            //Isession.query need to add "using nhibernate.linq"
            if (session.Query<User>().Any(t => t.Username == username || t.Email == email) )
            {
                SendError("Username and email must be unique");
                return;
            }

            var user = new User
            {
                Username = username,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                Password = HashedPassword.fromPlainText(password)
            };

            session.Save(user);
            SendSuccess();
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            //photon telling client has disconnected
            log.InfoFormat("Peer disconnected {0}:{1}", reasonCode, reasonDetail);
            _application.DestroyPeer(this);
        }

        private void SendSuccess()
        {
            SendOperationResponse(new OperationResponse((byte)RuneOperationResponse.Success), new SendParameters { Unreliable = false });
        }

        private void SendError(string message)
        {
            SendOperationResponse(
                new OperationResponse(
                    (byte)RuneOperationResponse.Error, 
                    new Dictionary<byte,object>
                    {
                        {(byte)RuneOperationResponseParameter.ErrorMessage, message}
                    }
                    )
               , new SendParameters
                {
                    Unreliable = false
                });
        }

    }
}
