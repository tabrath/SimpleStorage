using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace tabrath.SimpleStorage
{
    /// <summary>
    /// Provides methods for writing and reading objects to/from a stream or file.
    /// </summary>
    public static class SimpleStorage
    {
        private static readonly BinaryFormatter binaryFormatter = new BinaryFormatter();

        /// <summary>
        /// Write an object to a stream, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Destination stream.</param>
        /// <param name="obj">Object graph.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm.</param>
        public static void Write<T>(Stream stream, T obj, CompressionAlgorithm compressionAlgorithm = CompressionAlgorithm.None)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (obj == null)
                throw new ArgumentNullException("obj");
            
            if (compressionAlgorithm != CompressionAlgorithm.None)
            {
                Compress<T>(stream, obj, compressionAlgorithm);
            }
            else
            {
                lock (binaryFormatter)
                {
                    binaryFormatter.Serialize(stream, obj);
                }
            }
        }

        /// <summary>
        /// Write an object to a stream asynchronously, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Destination stream.</param>
        /// <param name="obj">Object graph.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        public static async Task WriteAsync<T>(Stream stream, T obj, CompressionAlgorithm compressionAlgorithm, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            if (stream == null)
                throw new ArgumentNullException("stream");

            if (obj == null)
                throw new ArgumentNullException("obj");

            if (compressionAlgorithm != CompressionAlgorithm.None)
            {
                await CompressAsync<T>(stream, obj, compressionAlgorithm, cancellationToken);
            }
            else
            {
                await binaryFormatter.SerializeAsync<T>(stream, obj, cancellationToken);
            }
        }

        /// <summary>
        /// Write an object to a file, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">Destination file.</param>
        /// <param name="obj">Object graph.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm</param>
        public static void Write<T>(string filename, T obj, CompressionAlgorithm compressionAlgorithm = CompressionAlgorithm.None)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            if (obj == null)
                throw new ArgumentNullException("obj");

            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.WriteThrough))
            {
                Write(stream, obj, compressionAlgorithm);
            }
        }

        /// <summary>
        /// Write an object to a file asynchronously, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">Destination file.</param>
        /// <param name="obj">Object graph.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        public static async Task WriteAsync<T>(string filename, T obj, CompressionAlgorithm compressionAlgorithm, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            if (obj == null)
                throw new ArgumentNullException("obj");

            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
            {
                await WriteAsync(stream, obj, compressionAlgorithm, cancellationToken);
            }
        }

        /// <summary>
        /// Read an object from a stream, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Source stream.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm.</param>
        /// <returns>Object</returns>
        public static T Read<T>(Stream stream, CompressionAlgorithm compressionAlgorithm = CompressionAlgorithm.None)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (compressionAlgorithm != CompressionAlgorithm.None)
            {
                return Decompress<T>(stream, compressionAlgorithm);
            }
            else
            {
                lock (binaryFormatter)
                {
                    return binaryFormatter.Deserialize<T>(stream);
                }
            }
        }

        /// <summary>
        /// Read an object from a stream asynchronously, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Source stream.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Object</returns>
        public static Task<T> ReadAsync<T>(Stream stream, CompressionAlgorithm compressionAlgorithm, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            if (stream == null)
                throw new ArgumentNullException("stream");

            if (compressionAlgorithm != CompressionAlgorithm.None)
            {
                return DecompressAsync<T>(stream, compressionAlgorithm, cancellationToken);
            }
            else
            {
                return binaryFormatter.DeserializeAsync<T>(stream, cancellationToken);
            }
        }

        /// <summary>
        /// Read an object from a file, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">Source file.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm.</param>
        /// <returns>Object</returns>
        public static T Read<T>(string filename, CompressionAlgorithm compressionAlgorithm = CompressionAlgorithm.None)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan))
            {
                return Read<T>(stream, compressionAlgorithm);
            }
        }

        /// <summary>
        /// Read an object from a file asynchronously, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">Source file.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Object</returns>
        public static Task<T> ReadAsync<T>(string filename, CompressionAlgorithm compressionAlgorithm, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous))
            {
                return ReadAsync<T>(stream, compressionAlgorithm, cancellationToken);
            }
        }

        /// <summary>
        /// Compress an object to a stream, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Destination stream.</param>
        /// <param name="obj">Object graph.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm.</param>
        public static void Compress<T>(Stream stream, T obj, CompressionAlgorithm compressionAlgorithm)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (obj == null)
                throw new ArgumentNullException("obj");

            Stream compressor;

            switch (compressionAlgorithm)
            {
                case CompressionAlgorithm.Deflate:
                    compressor = new DeflateStream(stream, CompressionLevel.Fastest, true);
                    break;

                case CompressionAlgorithm.GZip:
                    compressor = new GZipStream(stream, CompressionLevel.Fastest, true);
                    break;

                case CompressionAlgorithm.None:
                    throw new Exception("Decompression without a compression algorithm is, uh, not available.");

                default:
                    throw new Exception("Unknown compression algorithm.");
            }

            try
            {
                lock (binaryFormatter)
                {
                    binaryFormatter.Serialize<T>(compressor, obj);
                }
            }
            finally
            {
                if (compressor != null)
                    compressor.Dispose();
            }
        }

        /// <summary>
        /// Compress an object to a stream asynchronously, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Destination stream.</param>
        /// <param name="obj">Object graph.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        public static async Task CompressAsync<T>(Stream stream, T obj, CompressionAlgorithm compressionAlgorithm, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            if (stream == null)
                throw new ArgumentNullException("stream");

            if (obj == null)
                throw new ArgumentNullException("obj");

            Stream compressor;

            switch (compressionAlgorithm)
            {
                case CompressionAlgorithm.Deflate:
                    compressor = new DeflateStream(stream, CompressionLevel.Fastest, true);
                    break;

                case CompressionAlgorithm.GZip:
                    compressor = new GZipStream(stream, CompressionLevel.Fastest, true);
                    break;

                case CompressionAlgorithm.None:
                    throw new Exception("Decompression without a compression algorithm is, uh, not available.");

                default:
                    throw new Exception("Unknown compression algorithm.");
            }

            try
            {
                await binaryFormatter.SerializeAsync<T>(compressor, obj, cancellationToken);
            }
            finally
            {
                if (compressor != null)
                    compressor.Dispose();
            }
        }

        /// <summary>
        /// Decompress a stream to an object, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Source stream.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm.</param>
        /// <returns>Object</returns>
        public static T Decompress<T>(Stream stream, CompressionAlgorithm compressionAlgorithm)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            Stream decompressor;
            T result;

            switch (compressionAlgorithm)
            {
                case CompressionAlgorithm.Deflate:
                    decompressor = new DeflateStream(stream, CompressionMode.Decompress, true);
                    break;

                case CompressionAlgorithm.GZip:
                    decompressor = new GZipStream(stream, CompressionMode.Decompress, true);
                    break;

                case CompressionAlgorithm.None:
                    throw new Exception("Decompression without a compression algorithm is, uh, not available.");

                default:
                    throw new Exception("Unknown compression algorithm.");
            }

            try
            {
                lock (binaryFormatter)
                {
                    result = binaryFormatter.Deserialize<T>(decompressor);
                }
            }
            finally
            {
                if (decompressor != null)
                    decompressor.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Decompress a stream to an object asynchronously, with optional compression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Source stream.</param>
        /// <param name="compressionAlgorithm">Compression Algorithm.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Object</returns>
        public static async Task<T> DecompressAsync<T>(Stream stream, CompressionAlgorithm compressionAlgorithm, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            if (stream == null)
                throw new ArgumentNullException("stream");

            Stream decompressor;
            T result;

            switch (compressionAlgorithm)
            {
                case CompressionAlgorithm.Deflate:
                    decompressor = new DeflateStream(stream, CompressionMode.Decompress, true);
                    break;

                case CompressionAlgorithm.GZip:
                    decompressor = new GZipStream(stream, CompressionMode.Decompress, true);
                    break;

                case CompressionAlgorithm.None:
                    throw new Exception("Decompression without a compression algorithm is, uh, not available.");

                default:
                    throw new Exception("Unknown compression algorithm.");
            }

            try
            {
                result = await binaryFormatter.DeserializeAsync<T>(decompressor, cancellationToken);
            }
            finally
            {
                if (decompressor != null)
                    decompressor.Dispose();
            }

            return result;
        }
    }

    public enum CompressionAlgorithm
    {
        Deflate,
        GZip,
        None
    }
}
