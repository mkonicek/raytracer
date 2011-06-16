using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace Lucid.Base
{
    // will be in the client part of the lib

    // will work this way:
    // - client opens COMMAND TCP connection to server (on known port) for 2-way comm.
    // ...
    // - server sends scene config with list of files with sizes
    // ...
    // - server sends: "start downloading files - port 123"
    // ...
    // - client opens new connection to FileSender on port 123
    // client downloads all files needed:
    // {
    //   client sends fileName (no path, file has to be in same folder as scene)
    //   server responds by file content
    //   when client has whole file, repeat: -> client sends filename, server responds etc. etc.
    // }
    // - client closes connection -> files are downloaded

    delegate void ServeClientDelegate(TcpClient client);

    public class FileServer
    {
        /// <summary>
        /// Path from which serve files.
        /// </summary>
        public string FilePath { get; set; }

        TcpListener server = new TcpListener(Constants.FilePort);
        static readonly int bufferSize = 1024 * 1024;

        public FileServer(string path)
        {
            this.FilePath = path;
            // receive timeout for server must be infinite!
            server.Server.ReceiveTimeout = 0;
            server.Server.SendTimeout = 0;
        }

        /// <summary>
        /// Serves a given client according to protocol.
        /// </summary>
        void ServeClient(TcpClient client)
        {
            Stream stream = client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            StreamReader reader = new StreamReader(stream, Encoding.ASCII);

            while (client.Connected)
            {
                string fileName = null;
                try
                {
                    fileName = reader.ReadLine();
                }
                catch
                {
                    // connection can be dropped and receive could fail
                }
                if (fileName == null)
                    return;
                //char[] b = new char[120];
                //string b = reader.ReadToEnd();// Read(b, 0, 120);  // readtoend works how??
                //string fileName = b.
                string fullFileName = Path.Combine(this.FilePath, fileName);

                FileInfo file = new FileInfo(fullFileName);
                Inv.Log.Log.WriteMessage("Sending file " + file.FullName + " to client " + client.Client.RemoteEndPoint);
                if (file.Exists)
                {
                    try
                    {
                        // send file size
                        writer.Write((long)file.Length);
                        Stream fs = file.OpenRead();
                        using (BinaryReader fr = new BinaryReader(fs))
                        {
                            byte[] buffer = new byte[FileServer.bufferSize];
                            int read = fr.Read(buffer, 0, FileServer.bufferSize);
                            // could be simplified using server.Server.SendFile ?
                            while (read > 0)
                            {
                                writer.Write(buffer, 0, read);
                                read = fr.Read(buffer, 0, FileServer.bufferSize);
                            }
                        }
                    }
                    catch
                    {
                        // connection can be dropped and send could fail
                    }
                    Inv.Log.Log.WriteMessage("File sent: " + file.Name);
                }
                else
                {
                    Inv.Log.Log.WriteMessage("File not found: " + file.Name);
                    // send -1. No file found
                    writer.Write((long)-1);
                }
            }
        }

        public void Start()
        {
            // don't block
            ParallelExec.Parallel.Start(delegate()
            {
                // start listening for incoming connections
                server.Start();
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    ServeClientDelegate serveClientAsyncDelegate = ServeClient;
                    // serve clients in parallel
                    serveClientAsyncDelegate.BeginInvoke(client, null, null);
                    //serveClient.Invoke(client);
                }
            });
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            server.Stop();
        }

        // poslat vsem co jsou pripojeni
        public void SendFileToAllConnected()
        {
            // napad: prijemci dal rozesilaji, forma multicastu
            // S -- c1 - c3
            //   \   \_ c5
            //    \   
            //     c2 - c4
            // na to by ale prijemci museli byt zaroven i servery
            // to porusuje myslenku, ze klienti vubec jsou ciste klienti a NEPOSLOUCHAJI
        }
    }
}
