using System.Collections.Generic;
using System.IO;

namespace Pamux.GameDev.Lib.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> EnumerateDirectories(this string path)
        {
            return Directory.EnumerateDirectories(path);
        }

        public static IEnumerable<string> EnumerateFiles(this string path, string searchPattern)
        {
            return Directory.EnumerateFiles(path, searchPattern);
        }

        public static void RemoveDirectoryRecursively(this string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
        }

        public static void EnsureDirectory(this string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }


    }
}
