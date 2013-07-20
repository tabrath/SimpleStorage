
namespace tabrath.SimpleStorage
{
    public static class SimpleStorageExtensions
    {
        public static void Write<T>(this T obj, string filename, CompressionAlgorithm compressionAlgorithm = CompressionAlgorithm.None)
        {
            SimpleStorage.Write<T>(filename, obj, compressionAlgorithm);
        }
    }
}
