using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Test181107.Core
{
    [Serializable]
    public struct Header
    {
        public const int HEADER_SIZE = 512;
        public string Key { get; set; }
        public string Mark { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public long DataLength { get; set; }
        public object State { get; set; }
        public byte[] GetBytes()
        {
            MemoryStream memorystream = new MemoryStream(HEADER_SIZE);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(memorystream, this);
            var buffer = memorystream.GetBuffer();
            if (buffer.Length > HEADER_SIZE)
                throw new Exception($"size of header exceeded value of {nameof(HEADER_SIZE)}");
            return buffer;
        }
        public static Header GetHeader(byte[] arr)
        {
            MemoryStream memorystream = new MemoryStream(arr);
            BinaryFormatter bf = new BinaryFormatter();
            return (Header)bf.Deserialize(memorystream);
        }
        public override string ToString()
        {
            var str = $"key:{Key} from:{From ?? "system"} to:{To ?? "system"} size:{DataLength}";
            if (!string.IsNullOrEmpty(Mark))
                str += $" mark:{Mark}";
            return str;
        }
    }

}
