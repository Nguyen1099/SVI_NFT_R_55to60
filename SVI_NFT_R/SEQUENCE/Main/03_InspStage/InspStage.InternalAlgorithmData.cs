using SVI_NFT_R.CellData;
using System.Collections.Generic;
using System.Linq;

namespace SVI_NFT_R
{
    public partial class InspStage
    {
        internal sealed class InternalAlgorithmData
        {
            public int PositionIndex { get; private set; } = 0;
            public OneCell Data { get; private set; }
            public bool IsAlgorithmFinished { get; private set; } = false;

            public static InternalAlgorithmData[] CreateArrayFromCellDataHandler(IEnumerable<CellDataHandler> cellDataHandlers)
            {
                return cellDataHandlers.GetExistCellList()
                    .Select(i => new InternalAlgorithmData(i))
                    .ToArray();
            }

            public static InternalAlgorithmData[] CreateArrayFromCellDataHandler(CellDataHandler cellDataHandler)
            {
                if (cellDataHandler.IsCellExist() == false)
                {
                    return null;
                }
                return new InternalAlgorithmData[] { new InternalAlgorithmData(cellDataHandler) };
            }

            public bool TrySetAlgorithmFinished(string cellId)
            {
                if (Data.GetCellID() != cellId)
                {
                    return false;
                }

                IsAlgorithmFinished = true;
                return true;
            }

            private InternalAlgorithmData(CellDataHandler cellDataHandler)
            {
                PositionIndex = cellDataHandler.PositionIndex;
                Data = cellDataHandler.Data.DeepClone();
            }
        }
    }
}
