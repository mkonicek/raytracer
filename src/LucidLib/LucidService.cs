using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Lucid.Base
{
    [ServiceContract(Namespace = Constants.LucidNamespace)]
    public interface ILucidService
    {
        /// <summary>
        /// Lets server know that new client is available.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void RegisterClient();

        /// <summary>
        /// Lets server know that client will no longer be available.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void UnRegisterClient();

        /// <summary>
        /// Gets next job (whole work settings).
        /// </summary>
        /// <returns></returns>
        [UseNetDataContractSerializer]
        [OperationContract]
        JobSettings GetJob();

        /// <summary>
        /// Returns next task to be computed.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Task GetNextTask();

        /// <summary>
        /// Sends computed results back to server.
        /// </summary>
        /// <param name="finishedTask">Task with Result filled.</param>
        [UseNetDataContractSerializer]
        [OperationContract(IsOneWay=true)]
        void UploadResults(Task finishedTask);
    }
}
