using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace SVI_NFT_R
{
    public static partial class Utils
    {
        /// <summary>
        /// 입력한 폴더를 ZIP파일으로 압축 합니다
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="zipPath"></param>
        public static void CompressZipByIO(string sourcePath, string zipPath)
        {
            var filelist = getFileListRecursive(sourcePath, new List<string>());
            using (FileStream fileStream = new FileStream(zipPath, FileMode.Create, FileAccess.ReadWrite))
            {
                using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                {
                    foreach (string file in filelist)
                    {
                        string path = file.Substring(sourcePath.Length + 1);
                        zipArchive.CreateEntryFromFile(file, path);
                    }
                }
            }
        }

        /// <summary>
        /// [재귀함수] 디렉토리내 파일 검색
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="fileList"></param>
        /// <returns></returns>
        private static List<string> getFileListRecursive(string rootPath, List<string> fileList)
        {
            if (fileList == null)
            {
                return null;
            }
            var attr = File.GetAttributes(rootPath);
            // 해당 path가 디렉토리이면
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                var dirInfo = new DirectoryInfo(rootPath);
                // 하위 모든 디렉토리는
                foreach (var dir in dirInfo.GetDirectories())
                {
                    // 재귀로 통하여 list를 취득한다.
                    getFileListRecursive(dir.FullName, fileList);
                }
                // 하위 모든 파일은
                foreach (var file in dirInfo.GetFiles())
                {
                    // 재귀를 통하여 list를 취득한다.
                    getFileListRecursive(file.FullName, fileList);
                }
            }
            // 해당 path가 파일이면 (재귀를 통해 들어온 경로)
            else
            {
                var fileInfo = new FileInfo(rootPath);
                // 리스트에 full path를 저장한다.
                fileList.Add(fileInfo.FullName);
            }
            return fileList;
        }
    }

}
