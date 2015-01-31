using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

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
        /// Serialize object graph to a stream asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binaryFormatter"></param>
        /// <param name="stream">Destination stream.</param>
        /// <param name="graph">Object graph.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        public static Task SerializeAsync<T>(this BinaryFormatter binaryFormatter, Stream stream, T graph, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            return Task.Factory.StartNew(() => binaryFormatter.Serialize(stream, graph));
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

        /// <summary>
        /// Deserialize a stream to an object asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binaryFormatter"></param>
        /// <param name="stream">Source stream.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Deserialized object.</returns>
        public static Task<T> DeserializeAsync<T>(this BinaryFormatter binaryFormatter, Stream stream, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            return Task.Factory.StartNew<T>(() => (T)binaryFormatter.Deserialize(stream));
        }
    }
}
