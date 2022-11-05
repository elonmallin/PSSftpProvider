using System.IO;

namespace PSSftpProvider
{
    public static class StreamExtensions
    {
        public static string ToString(this Stream stream)
        {
            StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
