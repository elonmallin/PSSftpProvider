using System.Text.RegularExpressions;

namespace PSSftpProvider
{
    public static class StringExtensions
    {
        public static string RemoveHost(this string path)
        {
            return Regex.Replace(path, ".*://.*/", "");
        }

        public static string ForwardSlash(this string path)
        {
            return path.Replace("\\", "/");
        }

        public static string ToLocal(this string path)
        {
            return path
                .RemoveHost()
                .ForwardSlash();
        }
    }
}
