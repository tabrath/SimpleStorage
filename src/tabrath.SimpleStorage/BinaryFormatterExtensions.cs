using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace tabrath.SimpleStorage
{
    // Extension class for System.Runtime.Serialization.Formatters.BinaryFormatter
    public static class BinaryFormatterExtensions
    {
        /// <summary>
        /// Serialize object graph to a stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binaryFormatter"></param>
        /// <param name="stream">Destination stream.</param>
        /// <param name="graph">Object graph.</param>
        public static void Serialize<T>(this BinaryFormatter binaryFormatter, Stream stream, T graph)
        {
            binaryFormatter.Serialize(stream, graph);
        }

        /// <summary>
        /// Deserialize a stream to an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binaryFormatter"></param>
        /// <param name="stream">Source stream.</param>
        /// <returns>Deserialized object.</returns>
        public static T Deserialize<T>(this BinaryFormatter binaryFormatter, Stream stream)
        {
            return (T)binaryFormatter.Deserialize(stream);
        }
    }
}
