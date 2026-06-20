using System;
using System.IO;

namespace SVI_NFT_R
{
    public sealed class DefaultPathBuilder : IPathBuilder
    {
        private readonly IMccConfig mConfig;

        public DefaultPathBuilder(IMccConfig cfg)
        {
            mConfig = cfg;
        }

        public string BuildLocalTDir(DateTime dt)
        {
            return Path.Combine(
            mConfig.LocalPath,
            mConfig.BasicPath,
            mConfig.EquipmentCategoryName,
            mConfig.EquipmentId,
            string.Format("{0:yyMMdd}", dt),
            "T");
        }

        public string BuildLocalIndexDir()
        {
            return Path.Combine(
            mConfig.LocalPath,
            mConfig.BasicPath,
            mConfig.EquipmentCategoryName,
            mConfig.EquipmentId,
            "index");
        }

        public string BuildRemoteFromLocal(string localDir)
        {
            var pivot = localDir.IndexOf(mConfig.BasicPath, StringComparison.OrdinalIgnoreCase);
            if (pivot < 0)
            {
                return localDir.Replace('\\', '/') + "/";
            }
            var suffix = localDir.Substring(pivot).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return suffix.Replace('\\', '/') + "/";
        }

        public string BuildActionFileName(DateTime dt, string equipmentId)
        {
            return string.Format("{0}-{1}-{2:yyyyMMddHHmm00}.csv", "T", equipmentId, dt);
        }

        public string BuildIndexFileName(DateTime dt, string equipmentId)
        {
            return string.Format("{0}-{1}-{2:yyyyMMddHHmm00}.csv", "index", equipmentId, dt);
        }
    }
}
