using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Common;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Lucid.Base
{
    /// <summary>
    /// Implements server side of Lucid protocol. Serves job and tasks to clients, if they ask for it.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class LucidServer : ILucidService/*, IDisposable TODO: to make sure service is closed?*/
    {
        private ServiceHost serviceHost = null;
        private JobSettings jobSettings = null;
        private JobMaster workerMaster = null;

        public string LocalAddress { get; set; }


        #region Events
        /// <summary>
        /// Signalizes that new client has connected.
        /// </summary>
        public event ValueEventHandler<IPEndPoint> ClientConnect;
        private void onClientConnect(IPEndPoint clientEndPoint)
        {
            if (this.ClientConnect != null)
                ClientConnect(this, clientEndPoint);
        }

        /// <summary>
        /// Signalizes that client will no longer available.
        /// </summary>
        public event ValueEventHandler<IPEndPoint> ClientDisconnect;
        private void onClientDisconnect(IPEndPoint clientEndPoint)
        {
            if (ClientDisconnect != null)
                ClientDisconnect(this, clientEndPoint);
        }

        /// <summary>
        /// Helper that gets IPEndpoint of current caller.
        /// </summary>
        private IPEndPoint getCurrentCallerEndPoint()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties properties = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return new IPEndPoint(IPAddress.Parse(endpoint.Address), endpoint.Port);
        }


        void master_JobComplete(object sender, EventArgs e)
        {
            // propagate master's event
            onJobComplete();
            // no job set
            this.jobSettings = null;
        }
        /// <summary>
        /// Current job has been completed.
        /// </summary>
        public event EventHandler JobComplete;
        private void onJobComplete()
        {
            if (JobComplete != null)
                JobComplete(this, EventArgs.Empty);
        }


        void master_TaskComplete(object sender, Task taskCompleted)
        {
            // propagate master's event
            onTaskComplete(taskCompleted);
        }
        /// <summary>
        /// A task has been completed.
        /// </summary>
        public event ValueEventHandler<Task> TaskComplete;
        private void onTaskComplete(Task taskCompleted)
        {
            if (TaskComplete != null)
                TaskComplete(this, taskCompleted);
        }


        /// <summary>
        /// Task has been requested by client is being returned.
        /// </summary>
        public event ValueEventHandler<Task> TaskSentToClient;
        private void onTaskSentToClient(Task task)
        {
            if (TaskSentToClient != null)
                TaskSentToClient(this, task);
        }
        #endregion




        #region ILucidService Members

        public void RegisterClient()
        {
            onClientConnect(getCurrentCallerEndPoint());
        }

        public void UnRegisterClient()
        {
            onClientDisconnect(getCurrentCallerEndPoint());
        }

        public Task GetNextTask()
        {
            Task returnedTask = workerMaster.GetNextTask();
            onTaskSentToClient(returnedTask);
            return returnedTask;
        }

        public void UploadResults(Task finishedTask)
        {
            workerMaster.JoinCompletedTask(finishedTask);
        }

        public JobSettings GetJob()
        {
            return this.jobSettings;
        }

        #endregion





        public LucidServer(JobMaster master)
        {
            this.workerMaster = master;
            master.Complete += new EventHandler(master_JobComplete);
            master.TaskComplete += new ValueEventHandler<Task>(master_TaskComplete);
            //OperationDescription od = new OperationDescription("hello", ContractDescription.GetContract(typeof(ILucidService)));
            //od.KnownTypes.Add(jobSettingType);

            Uri baseAddress = new Uri(Constants.GetServerBaseAddress("localhost"));
            this.serviceHost = new ServiceHost(this, baseAddress);

            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None, true);
            //binding.TransferMode = TransferMode.Streamed; // does not work
            //Binding binding = new WSHttpBinding(SecurityMode.None, true);
            //binding.ReceiveTimeout = TimeSpan.MaxValue;   // don't drop connections after long inactivity
            binding.MaxReceivedMessageSize = 2 * 1024 * 1024;

            ServiceEndpoint endpoint = this.serviceHost.AddServiceEndpoint(
                typeof(ILucidService), binding, Constants.GetServerEndpointAddress("localhost"));

            this.serviceHost.CloseTimeout = TimeSpan.FromSeconds(2);

            try
            {
                string localHostName = Dns.GetHostName();   // inv
                IPAddress localHostAddress = Net.GetAddressFromHost(localHostName); // 78...
                LocalAddress = Net.TryGetHostNameFromAddress(localHostAddress);
            }
            catch
            {
                LocalAddress = "localhost";
            }

            // http metadata
            /*ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetUrl = new Uri("http://localhost:13000/Lucid");
            smb.HttpGetEnabled = true;
            this.serviceHost.Description.Behaviors.Add(smb);

            // metadata endpoint for creating the proxy
            serviceHost.AddServiceEndpoint(
                ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexTcpBinding(), "mex");*/
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start()
        {
            this.serviceHost.Open();
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            serviceHost.Close();
        }

        public void StartNewJob(JobSettings job)
        {
            this.jobSettings = job;
            this.workerMaster.ResetForNewJob(job);
        }
    }
}
