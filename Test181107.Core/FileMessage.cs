using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Test181107.Core
{
    public class FileMessage : MessagePayload, IDataHandler
    {
        private const int FILE_BATCH_MAX_SIZE = 1024;
        FileStream fileStream;
        public FileMessage(FileStream stream, string fileName, string from)
        {
            this.fileStream = stream;
            var length = stream.Length;
            Header = new Header() { Key = MessageKeys.File, DataLength = length, State = fileName, From = from };
        }
        public FileMessage(Header header)
        {
            Header = header;
        }

        public void Receive(NetworkStream networkStream)
        {
            var directoryPath = Directory.CreateDirectory("TempFile").FullName;
            var reserverPath = Path.Combine(directoryPath, Guid.NewGuid() + Header.State.ToString());
            var fileWriter = new FileWriter(reserverPath);
            fileWriter.Start();

            var remainSize = Header.DataLength;
            while (remainSize != 0)
            {
                var readSize = FILE_BATCH_MAX_SIZE > remainSize ? (int)remainSize : FILE_BATCH_MAX_SIZE;
                remainSize -= readSize;
                var batch = new FileBatch() { Bytes = ReceiveBatch(networkStream, readSize), Size = readSize };
                fileWriter.bytesList.Enqueue(batch);
            }
            fileWriter.Stop();
        }

        public void Transfer(NetworkStream networkStream)
        {
            fileStream.CopyTo(networkStream);
            fileStream.Dispose();
        }
        public override string ToString()
        {
            return base.ToString() + " filename:" + Header.State;
        }
    }
    public class FileWriter
    {
        public ConcurrentQueue<FileBatch> bytesList = new ConcurrentQueue<FileBatch>();
        FileStream fileStream;
        bool canBeCanceled;
        public FileWriter(string filePath)
        {
            fileStream = File.Create(filePath);
        }
        public async void Start() => await Task.Run(() =>
        {
            FileBatch batch;
            while (true)
            {
                if (bytesList.TryDequeue(out batch))
                {
                    fileStream.Write(batch.Bytes, 0, batch.Size);
                    fileStream.Flush();
                }
                else if (canBeCanceled)
                {
                    fileStream.Dispose();
                    break;
                }
                else
                {
                    Thread.Sleep(100);
                    continue;
                }
            }
        });

        public void Stop()
        {
            canBeCanceled = true;
        }
    }
    public struct FileBatch
    {
        public byte[] Bytes { get; set; }
        public int Size { get; set; }
    }
}
