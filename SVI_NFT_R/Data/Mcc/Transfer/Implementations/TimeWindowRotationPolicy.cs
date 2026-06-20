using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SVI_NFT_R
{
    public sealed class TimeWindowRotationPolicy : IRotationPolicy
    {
        public string ResolveActionFilePath(DateTime now, string lastActionPath, int loggingMinutes, string baseDir, string equipmentId)
        {
            return Resolve(now, lastActionPath, loggingMinutes, baseDir, "T", equipmentId);
        }

        public string ResolveIndexFilePath(DateTime now, string lastIndexPath, int loggingMinutes, string baseDir, string equipmentId)
        {
            return Resolve(now, lastIndexPath, loggingMinutes, baseDir, "index", equipmentId);
        }

        private static string Resolve(DateTime now, string lastPath, int loggingMinutes, string baseDir, string prefix, string equipmentId)
        {
            if (string.IsNullOrEmpty(lastPath))
            {
                if (!Directory.Exists(baseDir))
                {
                    Directory.CreateDirectory(baseDir);
                }
                var last = new DirectoryInfo(baseDir).GetFiles("*.csv")
                    .OrderByDescending(f => f.Name)
                    .FirstOrDefault();
                if (last != null)
                {
                    lastPath = last.FullName;
                }
                else
                {
                    lastPath = Path.Combine(baseDir, string.Format("{0}-{1}-{2:yyyyMMddHHmm00}.csv", prefix, equipmentId, now));
                }
            }


            if (loggingMinutes >= 60)
            {
                return Path.Combine(baseDir, string.Format("{0}-{1}-{2:yyyyMMddHH0000}.csv", prefix, equipmentId, now));
            }
            else
            {
                var name = Path.GetFileNameWithoutExtension(lastPath);
                var parts = name.Split('-');
                var timeToken = parts.Length >= 3 ? parts[2] : now.ToString("yyyyMMddHHmmss");
                var lastTime = DateTime.ParseExact(timeToken, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                if (now - lastTime > TimeSpan.FromMinutes(loggingMinutes))
                {
                    return Path.Combine(baseDir, string.Format("{0}-{1}-{2:yyyyMMddHHmm00}.csv", prefix, equipmentId, now));
                }
                return lastPath;
            }
        }
    }
}
