using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inv.Log
{
    public interface ILogger
    {
        void WriteMessage(string message);
    }
}
