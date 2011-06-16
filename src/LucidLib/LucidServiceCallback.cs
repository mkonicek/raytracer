using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Lucid.Base
{
    public interface ILucidServiceCallback
    {
        /// <summary>
        /// Server signalizes that clients should ask for new job.
        /// </summary>
        [OperationContract(IsOneWay=true)]
        void JobStarted();
    }
}
