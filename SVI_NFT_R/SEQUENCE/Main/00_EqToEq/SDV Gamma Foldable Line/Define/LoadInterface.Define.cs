using EqToEq;
using System.Diagnostics;

namespace SVI_NFT_R.EqToEq.SdvGammaFoldableLine
{
    partial class LoadInterface
    {
        private sealed class Define : LoadInterfaceSignalController
        {
            public Define(int position)
            {
                EEqToEqUiGroup key = 0;
                switch (position)
                {
                    case 0:
                        Name = "Load";
                        UpperBits = Address.Bits.UpperToSelf;
                        UpperWords = Address.Words.UpperToSelf;
                        SelfBits = Address.Bits.SelfToUpper;
                        SelfWords = Address.Words.SelfToUpper;
                        key = EEqToEqUiGroup.Load;
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }

                // UI 그룹 정보 등록
                EqToEqUiDataManager.Groups[key] = new EqToEqUiGroup()
                {
                    GroupName = key.ToString(),
                    LeftName = "Upper",
                    RightName = "Self"
                };
                EqToEqUiDataManager.Groups[key].AddLeftBits(UpperBits, bIsReadOnly: true);
                EqToEqUiDataManager.Groups[key].AddLeftDatas(UpperWords, bIsReadOnly: true);
                EqToEqUiDataManager.Groups[key].AddRightBits(SelfBits, bIsReadOnly: false);
                EqToEqUiDataManager.Groups[key].AddRightDatas(SelfWords, bIsReadOnly: false);
            }

            public void DryRunSignalClear()
            {
                UpperBits.SendAble.Value = false;
                UpperBits.SendStart.Value = false;
                UpperBits.SendComplete.Value = false;
                UpperBits.SendVacuumOnP1.Value = false;
                UpperBits.SendVacuumOnP2.Value = false;
                UpperBits.SendCellP1.Value = false;
                UpperBits.SendCellP2.Value = false;
                UpperBits.EmsSafe.Value = true;
                UpperBits.InterlockSafe.Value = true;
                UpperBits.InterlockDoorClosed.Value = true;

                SelfBits.ReceiveAble.Value = false;
                SelfBits.ReceiveStart.Value = false;
                SelfBits.ReceiveComplete.Value = false;
                SelfBits.ReceiveVacuumOnP1.Value = false;
                SelfBits.ReceiveVacuumOnP2.Value = false;
                SelfBits.ReceiveCellP1.Value = false;
                SelfBits.ReceiveCellP2.Value = false;
            }
        }

        private Define mDefine;
    }
}
