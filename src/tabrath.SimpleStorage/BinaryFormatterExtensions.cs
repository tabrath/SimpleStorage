using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace tabrath.SimpleStorage
{
    public static class BinaryFormatterExtensions
    {
        public static void Serialize<T>(this BinaryFormatter binaryFormatter, Stream stream, T graph)
        {
            binaryFormatter.Serialize(stream, graph);
        }

        public static T Deserialize<T>(this BinaryFormatter binaryFormatter, Stream stream)
        {
            return (T)binaryFormatter.Deserialize(stream);
        }
    }
}
