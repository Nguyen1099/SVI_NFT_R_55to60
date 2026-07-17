using System.Collections.Generic;
using System.Diagnostics;

namespace Mcc
{
    public static class MccLogExtentionMethods
    {
        public static MccLogProvider CreateMccLogProvider(this IMccLogItem logItem)
        {
            return new MccLogProvider(new IMccLogItem[] { logItem }, false);
        }

        public static MccLogProvider CreateMccLogProvider(this IMccLogItem[] logItems, bool bShouldWriteExistCell = false)
        {
            return new MccLogProvider(logItems, bShouldWriteExistCell);
        }

        public static void BatchWriteStart(this IEnumerable<IMccLogItem> items)
        {
            Debug.Assert(items != null);

            foreach (var item in items)
            {
                item.WriteStart();
            }
        }

        public static void BatchWriteEnd(this IEnumerable<IMccLogItem> items)
        {
            Debug.Assert(items != null);

            foreach (var item in items)
            {
                item.WriteEnd();
            }
        }

        public static void BatchWriteStartExist(this IEnumerable<IMccLogItem> items)
        {
            Debug.Assert(items != null);

            foreach (var item in items)
            {
                item.WriteStartExist();
            }
        }

        public static void BatchWriteEndExist(this IEnumerable<IMccLogItem> items)
        {
            Debug.Assert(items != null);

            foreach (var item in items)
            {
                item.WriteEndExist();
            }
        }
    }
}
