using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace tabrath.SimpleStorage
{
    public static class SimpleStorage
    {
        private static readonly BinaryFormatter binaryFormatter = new BinaryFormatter();

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

        public static void Write<T>(string filename, T obj, CompressionAlgorithm compressionAlgorithm = CompressionAlgorithm.None)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            if (obj == null)
                throw new ArgumentNullException("obj");

            using (var stream = File.Create(filename))
            {
                Write(stream, obj, compressionAlgorithm);
            }
        }

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

        public static T Read<T>(string filename, CompressionAlgorithm compressionAlgorithm = CompressionAlgorithm.None)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("filename");

            using (var stream = File.OpenRead(filename))
            {
                return Read<T>(stream, compressionAlgorithm);
            }
        }

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
    }

    public enum CompressionAlgorithm
    {
        Deflate,
        GZip,
        None
    }
}
