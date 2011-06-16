using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Inv.Common
{
    public class Net
    {
        /// <summary>
        /// Tries to resolve host name from IP address. Returns given address on failure.
        /// </summary>
        public static string TryGetHostNameFromAddress(IPAddress address)
        {
            string result;
            try
            {
                result = Dns.GetHostEntry(address).HostName;
            }
            catch
            {
                result = address.ToString();
            }
            return result;
        }

        /// <summary>
        /// Gets IP addres of given host.
        /// </summary>
        public static IPAddress GetAddressFromHost(string hostNameOrAddress)
        {
            IPAddress[] hostAddresses;
            try
            {
                hostAddresses = Dns.GetHostEntry(hostNameOrAddress).AddressList;
            }
            catch
            {
                // unable to resolve
                return null;
            }
            // we want only IPv4 addresses
            var q = from add in hostAddresses
                    where add.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                    select add;
            if (q.Count() == 0)
                return null;
            return q.First();
        }
    }
}
