using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TransferCommon
{
    public static class SerializeHelp
    {
        public static byte[] Serialize<T>(T t)
        {
            MemoryStream mStream = new MemoryStream();
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(mStream, t);
            return mStream.GetBuffer();
        }
        public static T Deserialize<T>(byte[] b)
        {
            BinaryFormatter bFormatter = new BinaryFormatter();
            return (T)bFormatter.Deserialize(new MemoryStream(b));
        }
    }
}
