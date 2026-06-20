using SVI_NFT_R.CellData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SVI_NFT_R
{
    public static partial class ExtentionMethods
    {
        public static void CreateInnerID(this CellDataHandler cellData) => cellData.Data.Cell.InnerID = $"{(char)('A' + ((int)cellData.ProcessIndex * 2) + cellData.PositionIndex)}{DateTime.Now:yyyyMMddHHmmssfff}";

        public static bool IsCellExist(this CellDataHandler cellData) => cellData.Data.IsUse;

        public static bool IsCellExist(this OneCell oneCell) => oneCell.IsUse;

        public static IList<CellDataHandler> GetExistCellList(this IEnumerable<CellDataHandler> cells) => (from cell in cells where cell.IsCellExist() select cell).ToArray();

        public static int GetExistCellCount(this IEnumerable<CellDataHandler> cells) => (from cell in cells where cell.IsCellExist() select cell).Count();

        public static bool IsCellExistFromList(this IEnumerable<CellDataHandler> cells) => cells.GetExistCellCount() > 0;

        public static string GetCellID(this CellDataHandler cellData)
        {
            if (cellData.IsCellExist() == true)
            {
                return cellData.Data.Cell.CellID;
            }
            return string.Empty;
        }

        public static string GetCellID(this OneCell cellData)
        {
            if (cellData.IsCellExist() == true)
            {
                return cellData.Cell.CellID;
            }
            return string.Empty;
        }

        public static string GetInspectionKey(this CellDataHandler cellData, bool bUseJobID = true)
        {
            return cellData.Data.GetInspectionKey(bUseJobID);
        }

        public static string GetInspectionKey(this OneCell cellData, bool bUseJobID = true)
        {
            if (cellData.IsCellExist() == true)
            {
                if (
                    bUseJobID == true
                    && cellData.Reader.ReaderResultCode == CCIMDefine.ReaderResultCode.OK
                    && string.IsNullOrWhiteSpace(cellData.Cell.JobID) == false
                    )
                {
                    return cellData.Cell.JobID;
                }
                else if (
                    cellData.Reader.ReaderResultCode == CCIMDefine.ReaderResultCode.OK
                    && string.IsNullOrWhiteSpace(cellData.Cell.CellID) == false
                    )
                {
                    return cellData.Cell.CellID;
                }
                else
                {
                    return cellData.Cell.InnerID;
                }
            }
            return string.Empty;
        }

        public static string GetInnerID(this CellDataHandler cellData)
        {
            if (cellData.IsCellExist() == true)
            {
                return cellData.Data.Cell.InnerID;
            }
            return string.Empty;
        }

        public static string GetParsingInnerID(this CellDataHandler cellData)
        {
            if (cellData.IsCellExist() == true)
            {
                return cellData.Data.Cell.InnerID.Substring(1);
            }
            return "0";
        }

        public static string GetInnerID(this OneCell cellData)
        {
            if (cellData.IsCellExist() == true)
            {
                return cellData.Cell.InnerID;
            }
            return string.Empty;
        }

        public static HashSet<string> GetAllCellID(this IEnumerable<CellDataHandler> cells)
        {
            return new HashSet<string>(from cell in cells where cell.IsCellExist() && !string.IsNullOrWhiteSpace(cell.Data.Cell.CellID) select cell.GetCellID());
        }

        public static HashSet<string> GetAllInnerID(this IEnumerable<CellDataHandler> cells)
        {
            return new HashSet<string>(from cell in cells where cell.IsCellExist() && !string.IsNullOrWhiteSpace(cell.Data.Cell.InnerID) select cell.GetInnerID());
        }

        public static DateTime GetInputDateTime(this CellDataHandler cellData)
        {
            // !!! InnerID를 파싱해서 가장 오래된 셀에 생성 일시를 반환 한다
            const string format = "yyyyMMddHHmmssfff";
            try
            {
                return DateTime.ParseExact(cellData.GetInnerID().Substring(1, format.Length), format, null);
            }
            // System.InvalidOperationException: cells에 셀이 존재하지 않을 때 발생하는 예외
            catch
            {
                // ! 먼저 투입된 셀을 찾기 위한 함수임으로 예외 발생시 시간의 최대 값을 반환한다
                return DateTime.MaxValue;
            }
        }
    }
}
