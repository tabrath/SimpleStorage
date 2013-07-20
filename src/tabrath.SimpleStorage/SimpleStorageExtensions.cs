
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
    }
}
