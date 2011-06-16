using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Base
{
    public class Constants
    {
        /// <summary>
        /// File server port.
        /// </summary>
        public static readonly int FilePort = 8132;
        /// <summary>
        /// Server broadcast port.
        /// </summary>
        public static readonly int BroadcastPort = 8317;
        /// <summary>
        /// Lucid server port.
        /// </summary>
        public static readonly int ServicePort = 8600;

        /// <summary>
        /// For WCF command serialization.
        /// </summary>
        public const string LucidNamespace = "http://coding-time.blogspot.com";

        private static readonly string lucidRel = "/Lucid";
        private static readonly string endpointRel = "/Server";

        /// <summary>
        /// Gets lucid server base address.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static string GetServerBaseAddress(string host)
        {
            return "net.tcp://" + host + ":" + Constants.ServicePort + Constants.lucidRel;
        }

        /// <summary>
        /// Gets lucid server endpoint address.
        /// </summary>
        public static string GetServerEndpointAddress(string host)
        {
            return GetServerBaseAddress(host) + endpointRel;
        }

        //public static readonly int MetadataPort = 8601;
        //public static readonly string BaseAddress = GetServerBaseAddress("localhost");
        //public static readonly string EndpointAddress = GetServerEndpointAddress("localhost");
        //public static readonly string MetadataAddress = "http://localhost:" + Constants.MetadataPort + Constants.lucidRel + Constants.endpointRel;
    }
}
