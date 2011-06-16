using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Inv.Common;

namespace Lucid.Base
{
    /// <summary>
    /// Searches local network, raising event on each reply.
    /// </summary>
    public class UdpBroadcaster
    {
        UdpClient udpClient = null;

        public event ValueEventHandler<IPEndPoint> OnBroadcastReply;
        private void onBroadcastReply(IPEndPoint replierEndPoint)
        {
            if (OnBroadcastReply != null)
                OnBroadcastReply(this, replierEndPoint);
        }

        public UdpBroadcaster(int receiveTimeoutMs)
        {
            udpClient = new UdpClient { EnableBroadcast = true };
            udpClient.Client.ReceiveTimeout = receiveTimeoutMs;
        }

        public UdpBroadcaster()
            : this(500)
        {
        }

        public void SearchNetwork(string applicationIdentifier, int timeoutMs)
        {
            udpClient.Client.ReceiveTimeout = timeoutMs;
            SearchNetwork(applicationIdentifier);
        }

        public void SearchNetwork(string applicationIdentifier)
        {
            byte[] dataGram = Encoding.UTF8.GetBytes(applicationIdentifier);
            udpClient.Send(dataGram, dataGram.Length, new IPEndPoint(IPAddress.Broadcast, Constants.BroadcastPort));

            while (true)
            {
                IPEndPoint replierEndPoint = null;
                byte[] reply = null;
                try
                {
                    reply = udpClient.Receive(ref replierEndPoint);
                }
                catch (SocketException)
                {
                    // timeout - end waiting for replies
                    break;
                }
                string replySent = Encoding.UTF8.GetString(reply);
                if (replySent == "hello " + applicationIdentifier)
                {
                    onBroadcastReply(replierEndPoint);
                }
            }
        }
    }
}
