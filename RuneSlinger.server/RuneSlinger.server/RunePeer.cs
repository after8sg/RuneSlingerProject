//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using log4net;
using System.Collections.Generic;
using System.Linq;

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
            if (operationRequest.OperationCode != 0)
            {
                log.WarnFormat("Peer sent unknown opcode: {0}", operationRequest.OperationCode);
                return;
            }
            var message = (string)operationRequest.Parameters[0];
            log.DebugFormat("Get message from client: {0}", message);

            var eventData = new EventData(0, new Dictionary<byte, object> { { 0, message } });
            var parameters = new SendParameters { Unreliable = false };
            foreach (var peer in _application.Peers.Where(t => t != this))
            {
                peer.SendEvent(eventData,parameters);
            }
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            //photon telling client has disconnected
            log.InfoFormat("Peer disconnected {0}:{1}", reasonCode, reasonDetail);
            _application.DestroyPeer(this);
        }


    }
}
