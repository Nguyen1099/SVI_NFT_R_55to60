using EqToEq;
using System.Diagnostics;

namespace SVI_NFT_R.EqToEq.SdvGammaFoldableLine
{
    partial class UnloadInterface
    {
        private sealed class Define : UnloadInterfaceSignalController
        {
            public Define(int position)
            {
                EEqToEqUiGroup key = 0;
                switch (position)
                {
                    case 0:
                        Name = "Unload";
                        LowerBits = Address.Bits.LowerToSelf;
                        LowerWords = Address.Words.LowerToSelf;
                        SelfBits = Address.Bits.SelfToLower;
                        SelfWords = Address.Words.SelfToLower;
                        key = EEqToEqUiGroup.Unload;
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }

                // UI 그룹 정보 등록
                EqToEqUiDataManager.Groups[key] = new EqToEqUiGroup()
                {
                    GroupName = key.ToString(),
                    LeftName = "Self",
                    RightName = "Lower"
                };
                EqToEqUiDataManager.Groups[key].AddLeftBits(SelfBits, bIsReadOnly: false);
                EqToEqUiDataManager.Groups[key].AddLeftDatas(SelfWords, bIsReadOnly: false);
                EqToEqUiDataManager.Groups[key].AddRightBits(LowerBits, bIsReadOnly: true);
                EqToEqUiDataManager.Groups[key].AddRightDatas(LowerWords, bIsReadOnly: true);
            }

            public void DryRunSignalClear()
            {
                SelfBits.SendAble.Value = false;
                SelfBits.SendStart.Value = false;
                SelfBits.SendComplete.Value = false;
                SelfBits.SendVacuumOffP1.Value = false;
                SelfBits.SendVacuumOffP2.Value = false;
                SelfBits.SendCellP1.Value = false;
                SelfBits.SendCellP2.Value = false;

                LowerBits.ReceiveAble.Value = false;
                LowerBits.ReceiveStart.Value = false;
                LowerBits.ReceiveComplete.Value = false;
                LowerBits.ReceiveVacuumOffP1.Value = false;
                LowerBits.ReceiveVacuumOffP2.Value = false;
                LowerBits.ReceiveCellP1.Value = false;
                LowerBits.ReceiveCellP2.Value = false;
                LowerBits.EmsSafe.Value = true;
                LowerBits.InterlockSafe.Value = true;
                LowerBits.InterlockDoorClosed.Value = true;
            }
        }

        private Define mDefine;
    }
}
