using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Lucid.Base
{
    /// <summary>
    /// Downloads files from FileServer.
    /// </summary>
    public class FileClient
    {
        /// <summary>
        /// Path to save downloaded files to.
        /// </summary>
        public string Path;

        TcpClient client = new TcpClient();
        static readonly int bufferSize = 1024 * 1024;

        public FileClient(string path)
        {
            this.Path = path;
        }

        public void DownloadFilesFrom(IPAddress ip, IEnumerable<LucidFileInfo> files)
        {
            //try
            {
                client = new TcpClient();
                client.Connect(new IPEndPoint(ip, Constants.FilePort));
            }
            //catch
            {
                //   throw new Exception("Unable to connect to server at " + ip.ToString());
            }
            try
            {
                client.ReceiveTimeout = 30000;
                client.SendTimeout = 30000;
                Stream stream = client.GetStream();
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    StreamWriter writer = new StreamWriter(stream, Encoding.ASCII);
                    foreach (LucidFileInfo file in files)
                    {
                        Inv.Log.Log.WriteMessage("Downloading file " + file.FileName);
                        // why did't this work at my home PC?
                        //writer.Write(file.FileName + "\n");
                        // workaround
                        byte[] fileNameBytes = Encoding.ASCII.GetBytes(file.FileName + "\n");
                        stream.Write(fileNameBytes, 0, fileNameBytes.Length);

                        System.IO.Directory.CreateDirectory(this.Path);
                        string saveFileName = System.IO.Path.Combine(this.Path, file.FileName);
                        if (!File.Exists(saveFileName))
                        {
                            // download only if file not present
                            FileStream fs = File.Open(saveFileName, FileMode.OpenOrCreate, FileAccess.Write);
                            using (BinaryWriter fw = new BinaryWriter(fs))
                            {
                                try
                                {
                                    // first read file size
                                    long fileSize = reader.ReadInt64();
                                    if (fileSize == -1)
                                        throw new Exception(string.Format("File {0} does not exist on server.", file.FileName));

                                    byte[] buffer = new byte[FileClient.bufferSize];
                                    int totalRead = 0;
                                    int read = 0;
                                    // read file
                                    do
                                    {
                                        read = reader.Read(buffer, 0, (int)Math.Min(fileSize - read, FileClient.bufferSize));
                                        totalRead += read;
                                        fw.Write(buffer, 0, read);
                                    }
                                    while (totalRead < fileSize && read > 0);
                                }
                                catch (IOException ex)
                                {
                                    throw new Exception(
                                        string.Format("Error to downloading file {0} from server.", file.FileName));
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                // if done or anything goes wrong
                // disconnect to signal that we are done downloading
                client.Client.Close();
            }
        }
    }
}
