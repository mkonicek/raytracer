using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Threading;

namespace Lucid.Base
{
    /// <summary>
    /// Implements client side of Lucid protocol.
    /// Asks LucidServer service for job and tasks and calls its worker to compute results,
    /// then sends the results back.
    /// </summary>
    public class LucidClient
    {
        private ILucidService _lucidServer;
        private JobSlave _worker;
        private Timer _timer;

        public event EventHandler JobStarted;
        private void onJobStarted()
        {
            if (JobStarted != null)
                JobStarted(this, EventArgs.Empty);
        }

        public bool Connected
        {
            get
            {
                return _lucidServer != null;
            }
        }

        private void timer_tick(object state)
        {
            JobSettings serverJobSettings = null;
            // poll for job
            lock (this)
            {
                if (_lucidServer != null)
                {
                    try
                    {
                        serverJobSettings = _lucidServer.GetJob();
                    }
                    catch (CommunicationException)
                    {
                        // happens when new job started or server quit
                        // during computation
                        return;
                    }
                }
            }
            if (serverJobSettings != null)
            {
                disableTimer(_timer);
                // if job ready, run it
                runJob(serverJobSettings);
                enableTimer(_timer);
            }
        }

        void enableTimer(Timer timer)
        {
            timer.Change(2000, 2000);
        }

        void disableTimer(Timer timer)
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public LucidClient(JobSlave worker)
        {
            this._worker = worker;
            _timer = new Timer(timer_tick, null, Timeout.Infinite, Timeout.Infinite);
        }

        public void Connect(string endPointAddress)
        {
            if (this._lucidServer != null)
            {
                Disconnect();
            }

            EndpointAddress serverEndpointAddress;
            try
            {
                serverEndpointAddress = new EndpointAddress(endPointAddress);
            }
            catch
            {
                // bad url
                throw new Exception("Bad server URL: " + endPointAddress);
            }
            Binding binding = new NetTcpBinding(SecurityMode.None, true);
            binding.ReceiveTimeout = TimeSpan.FromSeconds(10);
            binding.SendTimeout = TimeSpan.FromSeconds(10);
            binding.OpenTimeout = TimeSpan.FromSeconds(10);
            var factory = new ChannelFactory<ILucidService>(binding, serverEndpointAddress);

            this._lucidServer = factory.CreateChannel();
            // let server know we are available
            this._lucidServer.RegisterClient();

            Inv.Log.Log.WriteMessage("Connected to server " + endPointAddress);
        }

        public void Disconnect()
        {
            // disconnect was in race condition with runJob, because
            // disconnect could set _lucidServer to null
            // while runJob has been trying to upload results
            lock (this)
            {
                if (this._lucidServer != null)
                {
                    disableTimer(_timer);
                    try
                    {
                        this._lucidServer.UnRegisterClient();
                        ((IChannel)this._lucidServer).Close();
                    }
                    catch (CommunicationException)
                    {
                        // happens when new job started or server quit
                        // during computation
                    }
                    this._lucidServer = null;
                }
            }
        }

        public void StartWorking()
        {
            if (!Connected)
            {
                throw new InvalidOperationException("Cannot work when not connected to server.");
            }
            enableTimer(_timer);
        }

        private void runJob(JobSettings serverJobSettings)
        {
            try
            {
                onJobStarted();

                this._worker.SetJob(serverJobSettings);

                Task task;
                try
                {
                    task = _lucidServer.GetNextTask();
                }
                catch (CommunicationException)
                {
                    return;
                }
                while (task != null && Connected)
                {
                    Inv.Log.Log.WriteMessage("Starting task " + task.Number);
                    _worker.RunTask(task);
                    //Inv.Log.Log.WriteMessage("Uploading task " + task.Number);
                    try
                    {
                        lock (this)
                        {
                            // we could have been disconnected by the main thread
                            if (_lucidServer != null)
                            {
                                _lucidServer.UploadResults(task);
                                task = _lucidServer.GetNextTask();
                            }
                        }
                    }
                    catch (CommunicationException)
                    {
                        // happens when new job started or server quit
                        // during computation
                        return;
                    }
                }
            }
            catch
            {
                if (_lucidServer != null)
                {
                    Disconnect();
                }
            }
        }
    }
}
