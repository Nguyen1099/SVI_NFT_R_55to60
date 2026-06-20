using System.Collections.Generic;

namespace SVI_NFT_R.TactTime
{
    /// <summary>
    /// 택타임 매니저
    /// </summary>
    public static class Manager
    {
        private static List<Tact> mTactList = new List<Tact>();
        private static object mRemoveLock = new object();

        /// <summary>
        /// 새로운 택타임 객체를 생성 합니다.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>해당 InnerID에 대한 정보가 이미 존재 할 경우 null 반환</returns>
        public static Tact AddTactOrNull(string[] ID)
        {
            Tact result = null;
            var tactArray = mTactList.ToArray();
            do
            {
                if (
                    null == ID
                    || 0 == ID.Length
                    )
                {
                    break;
                }

                // Duplicate Validation
                bool bDuplicate = false;
                foreach (var item in tactArray)
                {
                    for (int i = 0; i < item.InnerID.Length; i++)
                    {
                        for (int j = 0; j < ID.Length; j++)
                        {
                            if (
                                false == string.IsNullOrWhiteSpace(item.InnerID[i])
                                && false == string.IsNullOrWhiteSpace(ID[j])
                                && item.InnerID[i] == ID[j]
                                )
                            {
                                bDuplicate = true;
                                result = item;
                                break;
                            }
                        }
                        if (null != result)
                        {
                            break;
                        }
                    }
                    if (null != result)
                    {
                        break;
                    }

                    for (int i = 0; i < item.CellID.Length; i++)
                    {
                        for (int j = 0; j < ID.Length; j++)
                        {
                            if (
                                false == string.IsNullOrWhiteSpace(item.CellID[i])
                                && false == string.IsNullOrWhiteSpace(ID[j])
                                && item.CellID[i] == ID[j]
                                )
                            {
                                bDuplicate = true;
                                result = item;
                                break;
                            }
                        }
                        if (null != result)
                        {
                            break;
                        }
                    }
                    if (null != result)
                    {
                        break;
                    }
                }

                if (true == bDuplicate)
                {
                    break;
                }

                lock (mRemoveLock)
                {
                    result = new Tact()
                    {
                        InnerID = ID
                    };
                    mTactList.Add(result);
                }

            } while (false);
            return result;
        }

        /// <summary>
        /// 새로운 택타임 객체를 불러 옵니다.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>해당 InnerID에 대한 정보가 없을 경우 null 반환</returns>
        public static Tact GetTactOrNull(string[] ID)
        {
            Tact result = null;
            var tactArray = mTactList.ToArray();
            do
            {
                if (
                    null == ID
                    || 0 == ID.Length
                    )
                {
                    break;
                }

                // Duplicate Validation
                bool bDuplicate = false;
                foreach (var item in tactArray)
                {
                    for (int i = 0; i < item.InnerID.Length; i++)
                    {
                        for (int j = 0; j < ID.Length; j++)
                        {
                            if (
                                false == string.IsNullOrWhiteSpace(item.InnerID[i])
                                && false == string.IsNullOrWhiteSpace(ID[j])
                                && item.InnerID[i] == ID[j]
                                )
                            {
                                bDuplicate = true;
                                result = item;
                                break;
                            }
                        }
                        if (null != result)
                        {
                            break;
                        }
                    }
                    if (null != result)
                    {
                        break;
                    }

                    for (int i = 0; i < item.CellID.Length; i++)
                    {
                        for (int j = 0; j < ID.Length; j++)
                        {
                            if (
                                false == string.IsNullOrWhiteSpace(item.CellID[i])
                                && false == string.IsNullOrWhiteSpace(ID[j])
                                && item.CellID[i] == ID[j]
                                )
                            {
                                bDuplicate = true;
                                result = item;
                                break;
                            }
                        }
                        if (null != result)
                        {
                            break;
                        }
                    }
                    if (null != result)
                    {
                        break;
                    }
                }

                if (false == bDuplicate)
                {
                    break;
                }

                // Delete Timeout Data
                lock (mRemoveLock)
                {
                    foreach (var item in tactArray)
                    {
                        if (true == item.IsTimeoutData)
                        {
                            mTactList.Remove(item);
                        }
                    }
                }
            } while (false);
            return result;
        }

        /// <summary>
        /// 택타임 객체를 삭제합니다.
        /// </summary>
        /// <param name="innerID"></param>
        public static void Delete(string[] innerID)
        {
            if (
                null == innerID
                || 0 == innerID.Length
                )
            {
                return;
            }

            var tactArray = mTactList.ToArray();
            lock (mRemoveLock)
            {
                foreach (var item in tactArray)
                {
                    for (int i = 0; i < item.InnerID.Length; i++)
                    {
                        for (int j = 0; j < innerID.Length; j++)
                        {
                            if (
                                false == string.IsNullOrWhiteSpace(item.InnerID[i])
                                && false == string.IsNullOrWhiteSpace(innerID[j])
                                && item.InnerID[i] == innerID[j]
                                )
                            {
                                mTactList.Remove(item);
                                return;
                            }
                        }
                    }
                }
            }
        }

    }
}
