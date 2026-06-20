using System.Collections.Generic;
using System.IO;

namespace SVI_NFT_R
{
    public interface IFileSystem
    {
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        IEnumerable<FileInfo> GetFiles(string directory, string searchPattern);
        IEnumerable<DirectoryInfo> GetDirectories(string directory);
        void DeleteDirectory(string path, bool recursive);
        bool FileExists(string path);
        void AppendAllText(string path, string contents);
        long GetFileSize(string path);
    }
}
