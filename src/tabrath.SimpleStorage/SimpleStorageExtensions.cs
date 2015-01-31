using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace tabrath.SimpleStorage
{
    // Extension class for any object to support writing to disk.
    public static class SimpleStorageExtensions
    {
        /// <summary>
        /// Write object to file, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="filename">Destination file.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm.</param>
        public static void Write<T>(this T obj, string filename, CompressionAlgorithm compressionAlgorithm = CompressionAlgorithm.None)
        {
            SimpleStorage.Write<T>(filename, obj, compressionAlgorithm);
        }

        /// <summary>
        /// Write object to file asynchronously, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="filename">Destination file.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public static Task WriteAsync<T>(this T obj, string filename, CompressionAlgorithm compressionAlgorithm, CancellationToken cancellationToken)
        {
            return SimpleStorage.WriteAsync<T>(filename, obj, compressionAlgorithm, cancellationToken);
        }

        /// <summary>
        /// Read object from stream, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Source stream.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm.</param>
        public static T Read<T>(this Stream stream, CompressionAlgorithm compressionAlgorithm = CompressionAlgorithm.None)
        {
            return SimpleStorage.Read<T>(stream, compressionAlgorithm);
        }

        /// <summary>
        /// Read object from stream asynchronously, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Source file.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public static Task<T> ReadAsync<T>(this Stream stream, CompressionAlgorithm compressionAlgorithm, CancellationToken cancellationToken)
        {
            return SimpleStorage.ReadAsync<T>(stream, compressionAlgorithm, cancellationToken);
        }
    }
}
