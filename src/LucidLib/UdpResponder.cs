using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using ParallelExec;

namespace Lucid.Base
{
    /// <summary>
    /// Responds to UDP pings. Responds only to clients with chosen application identifiers.
    /// </summary>
    public class UdpResponder
    {
        string applicationName = string.Empty;
        UdpClient udpClient = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationName">Your application identifier.</param>
        public UdpResponder(string applicationName)
        {
            this.applicationName = applicationName;
            this.udpClient = new UdpClient(Constants.BroadcastPort);
        }

        /// <summary>
        /// Starts listening asynchronously.
        /// </summary>
        public void Start()
        {
            Parallel.Start(delegate()
            {
                while (true)
                {
                    IPEndPoint broadcasterEndPoint = null;
                    byte[] dataGram = udpClient.Receive(ref broadcasterEndPoint);
                    string broadcastSent = Encoding.UTF8.GetString(dataGram);
                    if (broadcastSent == applicationName)
                    {
                        byte[] reply = Encoding.ASCII.GetBytes("hello " + applicationName);
                        // reply
                        udpClient.Send(reply, reply.Length, broadcasterEndPoint);
                    }
                }
            });
        }
    }
}
