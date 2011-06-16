using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inv.Log
{
    public static class Log
    {
        public static List<ILogger> Loggers { get; set; }

        static Log()
        {
            Loggers = new List<ILogger>();
        }

        public static void WriteMessage(string message)
        {
            foreach (ILogger logger in Loggers)
            {
                logger.WriteMessage(message);
            }
        }
    }
}
