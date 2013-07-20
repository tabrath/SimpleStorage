using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
