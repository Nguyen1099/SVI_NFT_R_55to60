using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SVI_NFT_R
{
    public sealed class SystemFileSystem : IFileSystem
    {
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public IEnumerable<FileInfo> GetFiles(string directory, string searchPattern)
        {
            var dirInfo = new DirectoryInfo(directory);
            if (dirInfo.Exists)
            {
                return dirInfo.GetFiles(searchPattern);
            }
            return Enumerable.Empty<FileInfo>();
        }

        public IEnumerable<DirectoryInfo> GetDirectories(string directory)
        {
            var dirInfo = new DirectoryInfo(directory);
            if (dirInfo.Exists)
            {
                return dirInfo.GetDirectories();
            }
            return Enumerable.Empty<DirectoryInfo>();
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            new DirectoryInfo(path).Delete(recursive);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public void AppendAllText(string path, string contents)
        {
            File.AppendAllText(path, contents);
        }

        public long GetFileSize(string path)
        {
            var info = new FileInfo(path);
            if (info.Exists)
            {
                return info.Length;
            }
            return -1L;
        }
    }
}
