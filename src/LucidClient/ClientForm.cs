using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Lucid.Raytracing;
using System.Drawing.Imaging;
using Lucid.Base;
using System.Net;
using Inv.Log;

namespace Lucid.Client
{
    public partial class ClientForm : Form
    {
        UdpBroadcaster broadcaster;
        RaytracingSlave worker;
        LucidClient lucidClient;
        string _btnScanOrigText;

        public ClientForm()
        {
            InitializeComponent();
            _btnScanOrigText = btnScanServers.Text;
            try
            {
                Log.Loggers.Add(new TextBoxLogger(txtLog));

                broadcaster = new UdpBroadcaster();
                broadcaster.OnBroadcastReply += broadcaster_reply;

                worker = new RaytracingSlave();
                lucidClient = new LucidClient(worker);
                EventHandler m = (object sender, EventArgs e) => { txtLog.Clear(); };
                lucidClient.JobStarted += (sender, e) => { this.Invoke(m); };
            }
            catch (Exception ex)
            {
                Inv.Common.Exceptions.ExceptionDialog(ex.Message);
            }
        }

        private void broadcaster_reply(object sender, IPEndPoint replierEndPoint)
        {
            //lsbServers.Items.Add(Dns.GetHostEntry(replierEndPoint.Address).HostName);
            lsbServers.Items.Add(Inv.Common.Net.TryGetHostNameFromAddress(replierEndPoint.Address));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            throw new Exception("down");
        }

        private void btnScanServers_Click(object sender, EventArgs e)
        {
            try
            {
                lsbServers.Items.Clear();
                btnScanServers.Text = "Searching...";
                btnScanServers.Enabled = false;
                broadcaster.SearchNetwork(Lucid.Raytracing.Constants.AppName, 500);
            }
            catch (Exception ex)
            {
                Inv.Common.Exceptions.ExceptionDialog(ex.Message);
            }
            finally
            {
                btnScanServers.Text = _btnScanOrigText;
                btnScanServers.Enabled = true;
            }
        }

        private void lsbServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsbServers.SelectedItem != null)
            {
                txtServer.Text = lsbServers.SelectedItem.ToString();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtServer.Text))
                {
                    throw new Exception("Please specify server URL.");
                }
                txtLog.Clear();
                lucidClient.Connect(Lucid.Base.Constants.GetServerEndpointAddress(txtServer.Text));
                worker.DownloadHost = txtServer.Text;
                lucidClient.StartWorking();
            }
            catch(Exception ex)
            {
                Inv.Common.Exceptions.ExceptionDialog(ex.Message);
            }
        }
    }
}