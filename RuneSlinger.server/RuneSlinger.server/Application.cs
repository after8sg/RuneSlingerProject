﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System.IO;
using ExitGames.Logging.Log4Net;
using Photon.SocketServer;
using log4net;
using log4net.Config;
using System.Collections.Generic;

namespace RuneSlinger.server
{
    public class Application : ApplicationBase
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(Application));
        private readonly List<RunePeer> _peers;

        public IEnumerable<RunePeer> Peers { get { return _peers; } }

        public Application()
        {
            _peers = new List<RunePeer>();
        }

        public void DestroyPeer(RunePeer peer)
        {
            _peers.Remove(peer);
        }

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            //log.InfoFormat("Peer created at {0}:{1}", initRequest.RemoteIP, initRequest.RemotePort);
            //return new RunePeer(initRequest);

            var peer = new RunePeer(this,initRequest);
            _peers.Add(peer);
            return peer;
        }

        protected override void Setup()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(BinaryPath, "log4net.config")));
            ExitGames.Logging.LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);

            log.Info("------ Application started for RuneSlinger ------");
        }

        protected override void TearDown()
        {
            log.Info("------- Application end ------");
        }

    }
}
