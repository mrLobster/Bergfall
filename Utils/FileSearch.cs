using System.Collections.Generic;
using System.IO;

namespace Bergfall.Utils
{
    public static class FileSearch
    {
        public static List<string> GetDirectoryTree(string rootpath)
        {
            List<string> directories = new List<string>();
            directories.AddRange(recursiveDirSearch(rootpath));
            return directories;
        }

        private static string[] recursiveDirSearch(string path)
        {
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in Directory.GetDirectories(path))
            {
                recursiveDirSearch(dir);
            }
            return dirs;
        }
    }
}