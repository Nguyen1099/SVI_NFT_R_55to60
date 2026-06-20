using SVI_NFT_R.DEVICE.Nachi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CFormSetupNachiOffsetData : CFormCommon, CFormInterface
    {
        private Dictionary<EOffset, string> mResourceOffsets = null;
        private string mResourceTitleSelectItem;
        private bool mbParameterChanged = false;
        private readonly CDocument mDocument;
        private ProcessDataInformation[] mSelectProcessItems = null;
        private CConfig.RobotModelParameter mSelectModelParameter = null;
        private ImageButton[] mBtnRobotIndex;
        private Panel[] mPnlToolBases;
        private SpeedButton[] mBtnToolTitles;
        private SpeedButton[][] mBtnToolData;
        private string[] mUnitNames;
        private const int MAX_PAGE_ITEM_COUNT = 4;
        private int mPageCount = 0;
        private int mPageIndex = -1;
        private int mSelectProcessItemIndex = 0;
        private CProcessMotion.ERobot mSelectRobotIndex = 0;
        private readonly Dictionary<CProcessMotion.ERobot, ProcessDataInformation[]> mRobotProcessItems = new Dictionary<CProcessMotion.ERobot, ProcessDataInformation[]>();
        /// <![CDATA[
        /// 'mStandardizedMap'은 표준 맵이 유지되는 이상 수정할 필요 없음
        /// ]]>
        private readonly ProcessDataInformation[] mStandardizedMap = new ProcessDataInformation[]
        {
            new ProcessDataInformation(processIndex: ERobotProcess.P1)
            {
                ToolOffests = new Dictionary<EOffset, CConfig.ERobotOffset[]>()
                {
                    [EOffset.T1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P1_T1_X, CConfig.ERobotOffset.P1_T1_Y, CConfig.ERobotOffset.P1_T1_Z, CConfig.ERobotOffset.P1_T1_Rz },
                    [EOffset.T2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P1_T2_X, CConfig.ERobotOffset.P1_T2_Y, CConfig.ERobotOffset.P1_T2_Z, CConfig.ERobotOffset.P1_T2_Rz },
                    [EOffset.T3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P1_T3_X, CConfig.ERobotOffset.P1_T3_Y, CConfig.ERobotOffset.P1_T3_Z, CConfig.ERobotOffset.P1_T3_Rz },
                    [EOffset.T4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P1_T4_X, CConfig.ERobotOffset.P1_T4_Y, CConfig.ERobotOffset.P1_T4_Z, CConfig.ERobotOffset.P1_T4_Rz },
                    [EOffset.T12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P1_T12_X, CConfig.ERobotOffset.P1_T12_Y, CConfig.ERobotOffset.P1_T12_Z, CConfig.ERobotOffset.P1_T12_Rz },
                    [EOffset.T34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P1_T34_X, CConfig.ERobotOffset.P1_T34_Y, CConfig.ERobotOffset.P1_T34_Z, CConfig.ERobotOffset.P1_T34_Rz },
                    [EOffset.Crossing_T1_S2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P1_T1_S2_X, CConfig.ERobotOffset.P1_T1_S2_Y, CConfig.ERobotOffset.P1_T1_S2_Z, CConfig.ERobotOffset.P1_T1_S2_Rz },
                    [EOffset.Crossing_T2_S1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P1_T2_S1_X, CConfig.ERobotOffset.P1_T2_S1_Y, CConfig.ERobotOffset.P1_T2_S1_Z, CConfig.ERobotOffset.P1_T2_S1_Rz },
                    [EOffset.Crossing_T3_S4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P1_T3_S4_X, CConfig.ERobotOffset.P1_T3_S4_Y, CConfig.ERobotOffset.P1_T3_S4_Z, CConfig.ERobotOffset.P1_T3_S4_Rz },
                    [EOffset.Crossing_T4_S3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P1_T4_S3_X, CConfig.ERobotOffset.P1_T4_S3_Y, CConfig.ERobotOffset.P1_T4_S3_Z, CConfig.ERobotOffset.P1_T4_S3_Rz },
                    [EOffset.Crossing_T12_S34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P1_T12_S34_X, CConfig.ERobotOffset.P1_T12_S34_Y, CConfig.ERobotOffset.P1_T12_S34_Z, CConfig.ERobotOffset.P1_T12_S34_Rz },
                }
            },
            new ProcessDataInformation(processIndex: ERobotProcess.P2)
            {
                ToolOffests = new Dictionary<EOffset, CConfig.ERobotOffset[]>()
                {
                    [EOffset.T1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P2_T1_X, CConfig.ERobotOffset.P2_T1_Y, CConfig.ERobotOffset.P2_T1_Z, CConfig.ERobotOffset.P2_T1_Rz },
                    [EOffset.T2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P2_T2_X, CConfig.ERobotOffset.P2_T2_Y, CConfig.ERobotOffset.P2_T2_Z, CConfig.ERobotOffset.P2_T2_Rz },
                    [EOffset.T3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P2_T3_X, CConfig.ERobotOffset.P2_T3_Y, CConfig.ERobotOffset.P2_T3_Z, CConfig.ERobotOffset.P2_T3_Rz },
                    [EOffset.T4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P2_T4_X, CConfig.ERobotOffset.P2_T4_Y, CConfig.ERobotOffset.P2_T4_Z, CConfig.ERobotOffset.P2_T4_Rz },
                    [EOffset.T12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P2_T12_X, CConfig.ERobotOffset.P2_T12_Y, CConfig.ERobotOffset.P2_T12_Z, CConfig.ERobotOffset.P2_T12_Rz },
                    [EOffset.T34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P2_T34_X, CConfig.ERobotOffset.P2_T34_Y, CConfig.ERobotOffset.P2_T34_Z, CConfig.ERobotOffset.P2_T34_Rz },
                    // + Align은 교차 동작 없음
                }
            },
            new ProcessDataInformation(processIndex: ERobotProcess.P3)
            {
                ToolOffests = new Dictionary<EOffset, CConfig.ERobotOffset[]>()
                {
                    [EOffset.T1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P3_T1_X, CConfig.ERobotOffset.P3_T1_Y, CConfig.ERobotOffset.P3_T1_Z, CConfig.ERobotOffset.P3_T1_Rz },
                    [EOffset.T2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P3_T2_X, CConfig.ERobotOffset.P3_T2_Y, CConfig.ERobotOffset.P3_T2_Z, CConfig.ERobotOffset.P3_T2_Rz },
                    [EOffset.T3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P3_T3_X, CConfig.ERobotOffset.P3_T3_Y, CConfig.ERobotOffset.P3_T3_Z, CConfig.ERobotOffset.P3_T3_Rz },
                    [EOffset.T4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P3_T4_X, CConfig.ERobotOffset.P3_T4_Y, CConfig.ERobotOffset.P3_T4_Z, CConfig.ERobotOffset.P3_T4_Rz },
                    [EOffset.T12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P3_T12_X, CConfig.ERobotOffset.P3_T12_Y, CConfig.ERobotOffset.P3_T12_Z, CConfig.ERobotOffset.P3_T12_Rz },
                    [EOffset.T34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P3_T34_X, CConfig.ERobotOffset.P3_T34_Y, CConfig.ERobotOffset.P3_T34_Z, CConfig.ERobotOffset.P3_T34_Rz },
                    // + MCR은 교차 동작 없음
                }
            },
            new ProcessDataInformation(processIndex: ERobotProcess.P4)
            {
                ToolOffests = new Dictionary<EOffset, CConfig.ERobotOffset[]>()
                {
                    [EOffset.T1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P4_T1_X, CConfig.ERobotOffset.P4_T1_Y, CConfig.ERobotOffset.P4_T1_Z, CConfig.ERobotOffset.P4_T1_Rz },
                    [EOffset.T2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P4_T2_X, CConfig.ERobotOffset.P4_T2_Y, CConfig.ERobotOffset.P4_T2_Z, CConfig.ERobotOffset.P4_T2_Rz },
                    [EOffset.T3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P4_T3_X, CConfig.ERobotOffset.P4_T3_Y, CConfig.ERobotOffset.P4_T3_Z, CConfig.ERobotOffset.P4_T3_Rz },
                    [EOffset.T4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P4_T4_X, CConfig.ERobotOffset.P4_T4_Y, CConfig.ERobotOffset.P4_T4_Z, CConfig.ERobotOffset.P4_T4_Rz },
                    [EOffset.T12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P4_T12_X, CConfig.ERobotOffset.P4_T12_Y, CConfig.ERobotOffset.P4_T12_Z, CConfig.ERobotOffset.P4_T12_Rz },
                    [EOffset.T34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P4_T34_X, CConfig.ERobotOffset.P4_T34_Y, CConfig.ERobotOffset.P4_T34_Z, CConfig.ERobotOffset.P4_T34_Rz },
                    [EOffset.Crossing_T1_S2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P4_T1_S2_X, CConfig.ERobotOffset.P4_T1_S2_Y, CConfig.ERobotOffset.P4_T1_S2_Z, CConfig.ERobotOffset.P4_T1_S2_Rz },
                    [EOffset.Crossing_T2_S1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P4_T2_S1_X, CConfig.ERobotOffset.P4_T2_S1_Y, CConfig.ERobotOffset.P4_T2_S1_Z, CConfig.ERobotOffset.P4_T2_S1_Rz },
                    [EOffset.Crossing_T3_S4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P4_T3_S4_X, CConfig.ERobotOffset.P4_T3_S4_Y, CConfig.ERobotOffset.P4_T3_S4_Z, CConfig.ERobotOffset.P4_T3_S4_Rz },
                    [EOffset.Crossing_T4_S3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P4_T4_S3_X, CConfig.ERobotOffset.P4_T4_S3_Y, CConfig.ERobotOffset.P4_T4_S3_Z, CConfig.ERobotOffset.P4_T4_S3_Rz },
                    [EOffset.Crossing_T12_S34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P4_T12_S34_X, CConfig.ERobotOffset.P4_T12_S34_Y, CConfig.ERobotOffset.P4_T12_S34_Z, CConfig.ERobotOffset.P4_T12_S34_Rz },
                    [EOffset.Crossing_T34_S12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P4_T34_S12_X, CConfig.ERobotOffset.P4_T34_S12_Y, CConfig.ERobotOffset.P4_T34_S12_Z, CConfig.ERobotOffset.P4_T34_S12_Rz },
                }
            },
            new ProcessDataInformation(processIndex: ERobotProcess.P5)
            {
                ToolOffests = new Dictionary<EOffset, CConfig.ERobotOffset[]>()
                {
                    [EOffset.T1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P5_T1_X, CConfig.ERobotOffset.P5_T1_Y, CConfig.ERobotOffset.P5_T1_Z, CConfig.ERobotOffset.P5_T1_Rz },
                    [EOffset.T2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P5_T2_X, CConfig.ERobotOffset.P5_T2_Y, CConfig.ERobotOffset.P5_T2_Z, CConfig.ERobotOffset.P5_T2_Rz },
                    [EOffset.T3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P5_T3_X, CConfig.ERobotOffset.P5_T3_Y, CConfig.ERobotOffset.P5_T3_Z, CConfig.ERobotOffset.P5_T3_Rz },
                    [EOffset.T4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P5_T4_X, CConfig.ERobotOffset.P5_T4_Y, CConfig.ERobotOffset.P5_T4_Z, CConfig.ERobotOffset.P5_T4_Rz },
                    [EOffset.T12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P5_T12_X, CConfig.ERobotOffset.P5_T12_Y, CConfig.ERobotOffset.P5_T12_Z, CConfig.ERobotOffset.P5_T12_Rz },
                    [EOffset.T34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P5_T34_X, CConfig.ERobotOffset.P5_T34_Y, CConfig.ERobotOffset.P5_T34_Z, CConfig.ERobotOffset.P5_T34_Rz },
                    [EOffset.Crossing_T1_S2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P5_T1_S2_X, CConfig.ERobotOffset.P5_T1_S2_Y, CConfig.ERobotOffset.P5_T1_S2_Z, CConfig.ERobotOffset.P5_T1_S2_Rz },
                    [EOffset.Crossing_T2_S1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P5_T2_S1_X, CConfig.ERobotOffset.P5_T2_S1_Y, CConfig.ERobotOffset.P5_T2_S1_Z, CConfig.ERobotOffset.P5_T2_S1_Rz },
                    [EOffset.Crossing_T3_S4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P5_T3_S4_X, CConfig.ERobotOffset.P5_T3_S4_Y, CConfig.ERobotOffset.P5_T3_S4_Z, CConfig.ERobotOffset.P5_T3_S4_Rz },
                    [EOffset.Crossing_T4_S3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P5_T4_S3_X, CConfig.ERobotOffset.P5_T4_S3_Y, CConfig.ERobotOffset.P5_T4_S3_Z, CConfig.ERobotOffset.P5_T4_S3_Rz },
                    [EOffset.Crossing_T12_S34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P5_T12_S34_X, CConfig.ERobotOffset.P5_T12_S34_Y, CConfig.ERobotOffset.P5_T12_S34_Z, CConfig.ERobotOffset.P5_T12_S34_Rz },
                    [EOffset.Crossing_T34_S12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P5_T34_S12_X, CConfig.ERobotOffset.P5_T34_S12_Y, CConfig.ERobotOffset.P5_T34_S12_Z, CConfig.ERobotOffset.P5_T34_S12_Rz },
                }
            },
            new ProcessDataInformation(processIndex: ERobotProcess.P6)
            {
                ToolOffests = new Dictionary<EOffset, CConfig.ERobotOffset[]>()
                {
                    [EOffset.T1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P6_T1_X, CConfig.ERobotOffset.P6_T1_Y, CConfig.ERobotOffset.P6_T1_Z, CConfig.ERobotOffset.P6_T1_Rz },
                    [EOffset.T2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P6_T2_X, CConfig.ERobotOffset.P6_T2_Y, CConfig.ERobotOffset.P6_T2_Z, CConfig.ERobotOffset.P6_T2_Rz },
                    [EOffset.T3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P6_T3_X, CConfig.ERobotOffset.P6_T3_Y, CConfig.ERobotOffset.P6_T3_Z, CConfig.ERobotOffset.P6_T3_Rz },
                    [EOffset.T4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P6_T4_X, CConfig.ERobotOffset.P6_T4_Y, CConfig.ERobotOffset.P6_T4_Z, CConfig.ERobotOffset.P6_T4_Rz },
                    [EOffset.T12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P6_T12_X, CConfig.ERobotOffset.P6_T12_Y, CConfig.ERobotOffset.P6_T12_Z, CConfig.ERobotOffset.P6_T12_Rz },
                    [EOffset.T34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P6_T34_X, CConfig.ERobotOffset.P6_T34_Y, CConfig.ERobotOffset.P6_T34_Z, CConfig.ERobotOffset.P6_T34_Rz },
                    [EOffset.Crossing_T1_S2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P6_T1_S2_X, CConfig.ERobotOffset.P6_T1_S2_Y, CConfig.ERobotOffset.P6_T1_S2_Z, CConfig.ERobotOffset.P6_T1_S2_Rz },
                    [EOffset.Crossing_T2_S1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P6_T2_S1_X, CConfig.ERobotOffset.P6_T2_S1_Y, CConfig.ERobotOffset.P6_T2_S1_Z, CConfig.ERobotOffset.P6_T2_S1_Rz },
                    [EOffset.Crossing_T3_S4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P6_T3_S4_X, CConfig.ERobotOffset.P6_T3_S4_Y, CConfig.ERobotOffset.P6_T3_S4_Z, CConfig.ERobotOffset.P6_T3_S4_Rz },
                    [EOffset.Crossing_T4_S3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P6_T4_S3_X, CConfig.ERobotOffset.P6_T4_S3_Y, CConfig.ERobotOffset.P6_T4_S3_Z, CConfig.ERobotOffset.P6_T4_S3_Rz },
                    [EOffset.Crossing_T12_S34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P6_T12_S34_X, CConfig.ERobotOffset.P6_T12_S34_Y, CConfig.ERobotOffset.P6_T12_S34_Z, CConfig.ERobotOffset.P6_T12_S34_Rz },
                    [EOffset.Crossing_T34_S12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P6_T34_S12_X, CConfig.ERobotOffset.P6_T34_S12_Y, CConfig.ERobotOffset.P6_T34_S12_Z, CConfig.ERobotOffset.P6_T34_S12_Rz },
                }
            },
            new ProcessDataInformation(processIndex: ERobotProcess.P7)
            {
                ToolOffests = new Dictionary<EOffset, CConfig.ERobotOffset[]>()
                {
                    [EOffset.T1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P7_T1_X, CConfig.ERobotOffset.P7_T1_Y, CConfig.ERobotOffset.P7_T1_Z, CConfig.ERobotOffset.P7_T1_Rz },
                    [EOffset.T2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P7_T2_X, CConfig.ERobotOffset.P7_T2_Y, CConfig.ERobotOffset.P7_T2_Z, CConfig.ERobotOffset.P7_T2_Rz },
                    [EOffset.T3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P7_T3_X, CConfig.ERobotOffset.P7_T3_Y, CConfig.ERobotOffset.P7_T3_Z, CConfig.ERobotOffset.P7_T3_Rz },
                    [EOffset.T4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P7_T4_X, CConfig.ERobotOffset.P7_T4_Y, CConfig.ERobotOffset.P7_T4_Z, CConfig.ERobotOffset.P7_T4_Rz },
                    [EOffset.T12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P7_T12_X, CConfig.ERobotOffset.P7_T12_Y, CConfig.ERobotOffset.P7_T12_Z, CConfig.ERobotOffset.P7_T12_Rz },
                    [EOffset.T34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P7_T34_X, CConfig.ERobotOffset.P7_T34_Y, CConfig.ERobotOffset.P7_T34_Z, CConfig.ERobotOffset.P7_T34_Rz },
                    [EOffset.Crossing_T1_S2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P7_T1_S2_X, CConfig.ERobotOffset.P7_T1_S2_Y, CConfig.ERobotOffset.P7_T1_S2_Z, CConfig.ERobotOffset.P7_T1_S2_Rz },
                    [EOffset.Crossing_T2_S1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P7_T2_S1_X, CConfig.ERobotOffset.P7_T2_S1_Y, CConfig.ERobotOffset.P7_T2_S1_Z, CConfig.ERobotOffset.P7_T2_S1_Rz },
                    [EOffset.Crossing_T3_S4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P7_T3_S4_X, CConfig.ERobotOffset.P7_T3_S4_Y, CConfig.ERobotOffset.P7_T3_S4_Z, CConfig.ERobotOffset.P7_T3_S4_Rz },
                    [EOffset.Crossing_T4_S3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P7_T4_S3_X, CConfig.ERobotOffset.P7_T4_S3_Y, CConfig.ERobotOffset.P7_T4_S3_Z, CConfig.ERobotOffset.P7_T4_S3_Rz },
                    [EOffset.Crossing_T12_S34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P7_T12_S34_X, CConfig.ERobotOffset.P7_T12_S34_Y, CConfig.ERobotOffset.P7_T12_S34_Z, CConfig.ERobotOffset.P7_T12_S34_Rz },
                    [EOffset.Crossing_T34_S12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P7_T34_S12_X, CConfig.ERobotOffset.P7_T34_S12_Y, CConfig.ERobotOffset.P7_T34_S12_Z, CConfig.ERobotOffset.P7_T34_S12_Rz },
                }
            },
            new ProcessDataInformation(processIndex: ERobotProcess.P8)
            {
                ToolOffests = new Dictionary<EOffset, CConfig.ERobotOffset[]>()
                {
                    [EOffset.T1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P8_T1_X, CConfig.ERobotOffset.P8_T1_Y, CConfig.ERobotOffset.P8_T1_Z, CConfig.ERobotOffset.P8_T1_Rz },
                    [EOffset.T2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P8_T2_X, CConfig.ERobotOffset.P8_T2_Y, CConfig.ERobotOffset.P8_T2_Z, CConfig.ERobotOffset.P8_T2_Rz },
                    [EOffset.T3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P8_T3_X, CConfig.ERobotOffset.P8_T3_Y, CConfig.ERobotOffset.P8_T3_Z, CConfig.ERobotOffset.P8_T3_Rz },
                    [EOffset.T4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P8_T4_X, CConfig.ERobotOffset.P8_T4_Y, CConfig.ERobotOffset.P8_T4_Z, CConfig.ERobotOffset.P8_T4_Rz },
                    [EOffset.T12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P8_T12_X, CConfig.ERobotOffset.P8_T12_Y, CConfig.ERobotOffset.P8_T12_Z, CConfig.ERobotOffset.P8_T12_Rz },
                    [EOffset.T34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P8_T34_X, CConfig.ERobotOffset.P8_T34_Y, CConfig.ERobotOffset.P8_T34_Z, CConfig.ERobotOffset.P8_T34_Rz },
                    [EOffset.Crossing_T1_S2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P8_T1_S2_X, CConfig.ERobotOffset.P8_T1_S2_Y, CConfig.ERobotOffset.P8_T1_S2_Z, CConfig.ERobotOffset.P8_T1_S2_Rz },
                    [EOffset.Crossing_T2_S1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P8_T2_S1_X, CConfig.ERobotOffset.P8_T2_S1_Y, CConfig.ERobotOffset.P8_T2_S1_Z, CConfig.ERobotOffset.P8_T2_S1_Rz },
                    [EOffset.Crossing_T3_S4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P8_T3_S4_X, CConfig.ERobotOffset.P8_T3_S4_Y, CConfig.ERobotOffset.P8_T3_S4_Z, CConfig.ERobotOffset.P8_T3_S4_Rz },
                    [EOffset.Crossing_T4_S3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P8_T4_S3_X, CConfig.ERobotOffset.P8_T4_S3_Y, CConfig.ERobotOffset.P8_T4_S3_Z, CConfig.ERobotOffset.P8_T4_S3_Rz },
                    [EOffset.Crossing_T12_S34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P8_T12_S34_X, CConfig.ERobotOffset.P8_T12_S34_Y, CConfig.ERobotOffset.P8_T12_S34_Z, CConfig.ERobotOffset.P8_T12_S34_Rz },
                    [EOffset.Crossing_T34_S12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P8_T34_S12_X, CConfig.ERobotOffset.P8_T34_S12_Y, CConfig.ERobotOffset.P8_T34_S12_Z, CConfig.ERobotOffset.P8_T34_S12_Rz },
                }
            },
            new ProcessDataInformation(processIndex: ERobotProcess.P9)
            {
                ToolOffests = new Dictionary<EOffset, CConfig.ERobotOffset[]>()
                {
                    [EOffset.T1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P9_T1_X, CConfig.ERobotOffset.P9_T1_Y, CConfig.ERobotOffset.P9_T1_Z, CConfig.ERobotOffset.P9_T1_Rz },
                    [EOffset.T2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P9_T2_X, CConfig.ERobotOffset.P9_T2_Y, CConfig.ERobotOffset.P9_T2_Z, CConfig.ERobotOffset.P9_T2_Rz },
                    [EOffset.T3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P9_T3_X, CConfig.ERobotOffset.P9_T3_Y, CConfig.ERobotOffset.P9_T3_Z, CConfig.ERobotOffset.P9_T3_Rz },
                    [EOffset.T4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P9_T4_X, CConfig.ERobotOffset.P9_T4_Y, CConfig.ERobotOffset.P9_T4_Z, CConfig.ERobotOffset.P9_T4_Rz },
                    [EOffset.T12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P9_T12_X, CConfig.ERobotOffset.P9_T12_Y, CConfig.ERobotOffset.P9_T12_Z, CConfig.ERobotOffset.P9_T12_Rz },
                    [EOffset.T34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P9_T34_X, CConfig.ERobotOffset.P9_T34_Y, CConfig.ERobotOffset.P9_T34_Z, CConfig.ERobotOffset.P9_T34_Rz },
                    [EOffset.Crossing_T1_S2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P9_T1_S2_X, CConfig.ERobotOffset.P9_T1_S2_Y, CConfig.ERobotOffset.P9_T1_S2_Z, CConfig.ERobotOffset.P9_T1_S2_Rz },
                    [EOffset.Crossing_T2_S1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P9_T2_S1_X, CConfig.ERobotOffset.P9_T2_S1_Y, CConfig.ERobotOffset.P9_T2_S1_Z, CConfig.ERobotOffset.P9_T2_S1_Rz },
                    [EOffset.Crossing_T3_S4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P9_T3_S4_X, CConfig.ERobotOffset.P9_T3_S4_Y, CConfig.ERobotOffset.P9_T3_S4_Z, CConfig.ERobotOffset.P9_T3_S4_Rz },
                    [EOffset.Crossing_T4_S3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P9_T4_S3_X, CConfig.ERobotOffset.P9_T4_S3_Y, CConfig.ERobotOffset.P9_T4_S3_Z, CConfig.ERobotOffset.P9_T4_S3_Rz },
                    [EOffset.Crossing_T12_S34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P9_T12_S34_X, CConfig.ERobotOffset.P9_T12_S34_Y, CConfig.ERobotOffset.P9_T12_S34_Z, CConfig.ERobotOffset.P9_T12_S34_Rz },
                    [EOffset.Crossing_T34_S12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P9_T34_S12_X, CConfig.ERobotOffset.P9_T34_S12_Y, CConfig.ERobotOffset.P9_T34_S12_Z, CConfig.ERobotOffset.P9_T34_S12_Rz },
                }
            },
            new ProcessDataInformation(processIndex: ERobotProcess.P10)
            {
                ToolOffests = new Dictionary<EOffset, CConfig.ERobotOffset[]>()
                {
                    [EOffset.T1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P10_T1_X, CConfig.ERobotOffset.P10_T1_Y, CConfig.ERobotOffset.P10_T1_Z, CConfig.ERobotOffset.P10_T1_Rz },
                    [EOffset.T2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P10_T2_X, CConfig.ERobotOffset.P10_T2_Y, CConfig.ERobotOffset.P10_T2_Z, CConfig.ERobotOffset.P10_T2_Rz },
                    [EOffset.T3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P10_T3_X, CConfig.ERobotOffset.P10_T3_Y, CConfig.ERobotOffset.P10_T3_Z, CConfig.ERobotOffset.P10_T3_Rz },
                    [EOffset.T4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P10_T4_X, CConfig.ERobotOffset.P10_T4_Y, CConfig.ERobotOffset.P10_T4_Z, CConfig.ERobotOffset.P10_T4_Rz },
                    [EOffset.T12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P10_T12_X, CConfig.ERobotOffset.P10_T12_Y, CConfig.ERobotOffset.P10_T12_Z, CConfig.ERobotOffset.P10_T12_Rz },
                    [EOffset.T34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P10_T34_X, CConfig.ERobotOffset.P10_T34_Y, CConfig.ERobotOffset.P10_T34_Z, CConfig.ERobotOffset.P10_T34_Rz },
                    [EOffset.Crossing_T1_S2] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P10_T1_S2_X, CConfig.ERobotOffset.P10_T1_S2_Y, CConfig.ERobotOffset.P10_T1_S2_Z, CConfig.ERobotOffset.P10_T1_S2_Rz },
                    [EOffset.Crossing_T2_S1] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P10_T2_S1_X, CConfig.ERobotOffset.P10_T2_S1_Y, CConfig.ERobotOffset.P10_T2_S1_Z, CConfig.ERobotOffset.P10_T2_S1_Rz },
                    [EOffset.Crossing_T3_S4] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P10_T3_S4_X, CConfig.ERobotOffset.P10_T3_S4_Y, CConfig.ERobotOffset.P10_T3_S4_Z, CConfig.ERobotOffset.P10_T3_S4_Rz },
                    [EOffset.Crossing_T4_S3] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P10_T4_S3_X, CConfig.ERobotOffset.P10_T4_S3_Y, CConfig.ERobotOffset.P10_T4_S3_Z, CConfig.ERobotOffset.P10_T4_S3_Rz },
                    [EOffset.Crossing_T12_S34] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P10_T12_S34_X, CConfig.ERobotOffset.P10_T12_S34_Y, CConfig.ERobotOffset.P10_T12_S34_Z, CConfig.ERobotOffset.P10_T12_S34_Rz },
                    [EOffset.Crossing_T34_S12] = new CConfig.ERobotOffset[] { CConfig.ERobotOffset.P10_T34_S12_X, CConfig.ERobotOffset.P10_T34_S12_Y, CConfig.ERobotOffset.P10_T34_S12_Z, CConfig.ERobotOffset.P10_T34_S12_Rz },
                }
            },
        };

        private enum ECoordinateSystem
        {
            XAxisR0 = 0,
            XAxisR90,
            XAxisR180,
            XAxisR270,
        }

        private enum EOffset
        {
            T1 = 0,
            T2,
            T3,
            T4,
            T12,
            T34,
            Crossing_T1_S2,
            Crossing_T2_S1,
            Crossing_T3_S4,
            Crossing_T4_S3,
            Crossing_T12_S34,
            Crossing_T34_S12,
        }

        [Serializable]
        private class ProcessDataInformation
        {
            public bool IsUsing { get; set; } = false;
            /// <summary>
            /// 다국어 처리를 위한 컴포넌트 아이디 (네이밍 규칙: $"mResourceProcessData[{RobotIndex}][{ProcessIndex}]")
            /// </summary>
            public string Uiid => $"mResourceProcessData[{RobotIndex}][{ProcessIndex}]";
            public string Name { get; set; }
            public CProcessMotion.ERobot RobotIndex { get; set; } = 0;
            public ERobotProcess ProcessIndex { get; private set; }
            public Dictionary<EOffset, CConfig.ERobotOffset[]> ToolOffests { get; set; }
            public ECoordinateSystem CoordinateSystem { get; set; }
            public List<EOffset> DisplayItems { get; set; }

            public ProcessDataInformation(ERobotProcess processIndex)
            {
                ProcessIndex = processIndex;
            }
        }

        public CFormSetupNachiOffsetData(CDocument objDocument)
        {
            mDocument = objDocument;

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeAutoScale();
        }

        private void CFormSetupMonitor_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void CFormSetupMonitor_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeInitialize();
        }

        public bool Initialize()
        {
            bool bReturn = false;

            do
            {
                if (false == InitializeForm())
                {
                    break;
                }

                bReturn = true;
            } while (false);

            return bReturn;
        }

        public void DeInitialize()
        {
        }

        public bool InitializeForm()
        {
            bool bReturn = false;

            do
            {
                // 유저 권한 레벨에 따른 버튼 상태 변경 델리게이트 생성
                base.m_delegateSetResourceControl = new DelegateSetResourceControl(SetResourceControl);
                {
                    // 로봇 선택 버튼을 생성한다
                    var robots = Enum.GetValues(typeof(CProcessMotion.ERobot));
                    mBtnRobotIndex = new ImageButton[robots.Length];
                    for (int i = 0; i < mBtnRobotIndex.Length; i++)
                    {
                        mBtnRobotIndex[i] = new ImageButton();
                        mBtnRobotIndex[i].Name = $"{nameof(mBtnRobotIndex)}_{i}";
                        mBtnRobotIndex[i].Size = BtnRobotIndex00.Size;
                        mBtnRobotIndex[i].Margin = BtnRobotIndex00.Margin;
                        mBtnRobotIndex[i].Tag = (CProcessMotion.ERobot)robots.GetValue(i);
                        mBtnRobotIndex[i].Click += BtnRobotIndex_Click;
                        PnlRobotIndexBase.Controls.Add(mBtnRobotIndex[i]);
                    }
                }
                BtnProcessSelect00.Tag = ERobotProcess.P1;
                BtnProcessSelect01.Tag = ERobotProcess.P2;
                BtnProcessSelect02.Tag = ERobotProcess.P3;
                BtnProcessSelect03.Tag = ERobotProcess.P4;
                BtnProcessSelect04.Tag = ERobotProcess.P5;
                BtnProcessSelect05.Tag = ERobotProcess.P6;
                BtnProcessSelect06.Tag = ERobotProcess.P7;
                BtnProcessSelect07.Tag = ERobotProcess.P8;
                BtnProcessSelect08.Tag = ERobotProcess.P9;
                BtnProcessSelect09.Tag = ERobotProcess.P10;
                mPnlToolBases = new Panel[]
                {
                    PnlSelectItemToolBase1,
                    PnlSelectItemToolBase2,
                    PnlSelectItemToolBase3,
                    PnlSelectItemToolBase4
                };
                mBtnToolTitles = new SpeedButton[]
                {
                    BtnSelectToolTitle1,
                    BtnSelectToolTitle2,
                    BtnSelectToolTitle3,
                    BtnSelectToolTitle4
                };
                mBtnToolData = new SpeedButton[][]
                {
                    new SpeedButton[] { BtnSelectToolX1, BtnSelectToolY1, BtnSelectToolZ1, BtnSelectToolRz1 },
                    new SpeedButton[] { BtnSelectToolX2, BtnSelectToolY2, BtnSelectToolZ2, BtnSelectToolRz2 },
                    new SpeedButton[] { BtnSelectToolX3, BtnSelectToolY3, BtnSelectToolZ3, BtnSelectToolRz3 },
                    new SpeedButton[] { BtnSelectToolX4, BtnSelectToolY4, BtnSelectToolZ4, BtnSelectToolRz4 },
                };
                for (int i = 0; i < mBtnToolData.Length; i++)
                {
                    mBtnToolData[i][0].Tag = 0 + (i * 10);
                    mBtnToolData[i][1].Tag = 1 + (i * 10);
                    mBtnToolData[i][2].Tag = 2 + (i * 10);
                    mBtnToolData[i][3].Tag = 3 + (i * 10);
                }
                mUnitNames = new string[]
                {
                    CDefine.UNIT_MILLIMETER,
                    CDefine.UNIT_MILLIMETER,
                    CDefine.UNIT_MILLIMETER,
                    CDefine.UNIT_ANGULAR
                };
                // 로봇 프로세스 맵 초기화
                SetRobotProcessMapInitialize();
                mbParameterChanged = false;
                selectRobot(0);
                // 버튼 색상 정의
                SetButtonColor();
                // 타이머 외부에서 제어
                timer.Interval = 100;
                timer.Enabled = false;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <![CDATA[
        /// 로봇 구성에 맞도록 아래를 수정해야함
        /// - SetRobotProcessMapInitialize() 함수만 수정해야함
        /// - SetRobotProcessMapData() 함수는 변경하지 않음
        /// - P2(Align), P3(MCR)는 교차 동작이 없음
        /// - 로봇맵 공통화로 P1~P10까지 미리 정의되어 있어 고정이지만 실제 설비에서 사용하는 포지션만 표시하도록 구성함
        /// - 표시하지 않을 항목은 'DisplayOffsets'에 정의함 (예를 들어 설비에서 교차 동작을 지원하지 않으면 변수 초기화시 사용하는 부분만 리스트에 넣어줌)
        /// ]]>
        private void SetRobotProcessMapInitialize()
        {
            CProcessMotion.ERobot robotIndex;
            List<ProcessDataInformation> usingProcessMap;
            mRobotProcessItems.Clear();
            // IN_ROBOT
            {
                // + [수정] 로봇 인덱스를 정의함
                robotIndex = CProcessMotion.ERobot.IN_ROBOT;
                // + [수정] 표시할 포지션과 옵셋 종류를 정의함
                usingProcessMap = new List<ProcessDataInformation>()
                {
                    new ProcessDataInformation(ERobotProcess.P1) { DisplayItems = new List<EOffset>() { /*EOffset.T1, EOffset.T2*/ }, CoordinateSystem = ECoordinateSystem.XAxisR180 },
                    new ProcessDataInformation(ERobotProcess.P3) { DisplayItems = new List<EOffset>() { /*EOffset.T1, EOffset.T2, EOffset.T12*/ }, CoordinateSystem = ECoordinateSystem.XAxisR180 },
                    new ProcessDataInformation(ERobotProcess.P4) { DisplayItems = new List<EOffset>() { EOffset.T1, EOffset.T2 }, CoordinateSystem = ECoordinateSystem.XAxisR180 },
                };
                // + [고정] 맵 데이터 초기화
                SetRobotProcessMapData(robotIndex, usingProcessMap);
            }
            // OUT_ROBOT
            {
                // + [수정] 로봇 인덱스를 정의함
                robotIndex = CProcessMotion.ERobot.OUT_ROBOT;
                // + [수정] 표시할 포지션과 옵셋 종류를 정의함
                usingProcessMap = new List<ProcessDataInformation>()
                {
                    new ProcessDataInformation(ERobotProcess.P1) { DisplayItems = new List<EOffset>() { /*EOffset.T1, EOffset.T2, EOffset.T12*/ }, CoordinateSystem = ECoordinateSystem.XAxisR180 },
                    new ProcessDataInformation(ERobotProcess.P4) { DisplayItems = new List<EOffset>() { /*EOffset.T1, EOffset.T2*/ }, CoordinateSystem = ECoordinateSystem.XAxisR180 },
                };
                // + [고정] 맵 데이터 초기화
                SetRobotProcessMapData(robotIndex, usingProcessMap);
            }
            //+로봇이 추가될 때[Sample]
            //// OUT_ROBOT
            //{
            //    // + [수정] 로봇 인덱스를 정의함
            //    robotIndex = CProcessMotion.ERobot.OUT_ROBOT;
            //    // + [수정] 표시할 포지션과 옵셋 종류를 정의함
            //    usingProcessMap = new List<ProcessDataInformation>()
            //    {
            //        new ProcessDataInformation(ERobotProcess.P1) { DisplayItems = new List<EOffset>() { EOffset.T1 }, CoordinateSystem = ECoordinateSystem.XAxisR180 },
            //        new ProcessDataInformation(ERobotProcess.P4) { DisplayItems = new List<EOffset>() { EOffset.T1 }, CoordinateSystem = ECoordinateSystem.XAxisR180 },
            //        new ProcessDataInformation(ERobotProcess.P5) { DisplayItems = new List<EOffset>() { EOffset.T1 }, CoordinateSystem = ECoordinateSystem.XAxisR180 }
            //    };
            //    // + [고정] 맵 데이터 초기화
            //    SetRobotProcessMapData(robotIndex, usingProcessMap);
            //}
        }

        private void SetRobotProcessMapData(CProcessMotion.ERobot robotIndex, List<ProcessDataInformation> usingProcessMap)
        {
            // + 표준 맵핑 깊은 복사
            mRobotProcessItems[robotIndex] = mStandardizedMap.DeepClone();
            // + 입력된 변수에 맞게 초기화
            foreach (var processItemPair in usingProcessMap)
            {
                var usingItem = mRobotProcessItems[robotIndex].First(item => item.ProcessIndex == processItemPair.ProcessIndex);
                usingItem.IsUsing = true;
                usingItem.RobotIndex = robotIndex;
                usingItem.CoordinateSystem = processItemPair.CoordinateSystem;
                usingItem.DisplayItems = processItemPair.DisplayItems;
            }
        }

        private void SetButtonColor()
        {
            // 버튼 색 변경
            SetButtonBackColor(BtnTitle, m_colorLabel);
            SetButtonBackColor(BtnSelectProcessTitle, m_colorLabel);
            SetButtonBackColor(BtnSelectItemTitle, m_colorLabel);
            SetButtonBackColor(BtnSelectToolTitle1, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolTitle2, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolTitle3, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolTitle4, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolXTitle1, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolYTitle1, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolZTitle1, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolRzTitle1, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolXTitle2, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolYTitle2, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolZTitle2, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolRzTitle2, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolXTitle3, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolYTitle3, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolZTitle3, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolRzTitle3, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolXTitle4, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolYTitle4, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolZTitle4, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolRzTitle4, m_colorLabelSub);
            SetButtonBackColor(BtnTitleCoordinateSystem, m_colorLabelSub);
            SetButtonBackColor(BtnOverrideSpeedTitle, m_colorLabelSub);
            SetButtonBackColor(BtnRecipeNoTitle, m_colorLabelSub);
            SetButtonBackColor(BtnSelectToolX1, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolY1, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolZ1, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolRz1, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolX2, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolY2, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolZ2, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolRz2, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolX3, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolY3, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolZ3, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolRz3, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolX4, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolY4, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolZ4, m_colorLabelData);
            SetButtonBackColor(BtnSelectToolRz4, m_colorLabelData);
            SetButtonBackColor(BtnOverrideSpeed, m_colorLabelData);
            SetButtonBackColor(BtnRecipeNo, m_colorLabelData);

            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons)
            {
                if (
                    null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == btn.Name.Equals(BtnRecipeNo.Name)
                    && false == btn.Name.Equals(BtnOverrideSpeed.Name)
                    && false == btn.Name.Equals(BtnSelectToolX1.Name)
                    && false == btn.Name.Equals(BtnSelectToolY1.Name)
                    && false == btn.Name.Equals(BtnSelectToolZ1.Name)
                    && false == btn.Name.Equals(BtnSelectToolRz1.Name)
                    && false == btn.Name.Equals(BtnSelectToolX2.Name)
                    && false == btn.Name.Equals(BtnSelectToolY2.Name)
                    && false == btn.Name.Equals(BtnSelectToolZ2.Name)
                    && false == btn.Name.Equals(BtnSelectToolRz2.Name)
                    && false == btn.Name.Equals(BtnSelectToolX3.Name)
                    && false == btn.Name.Equals(BtnSelectToolY3.Name)
                    && false == btn.Name.Equals(BtnSelectToolZ3.Name)
                    && false == btn.Name.Equals(BtnSelectToolRz3.Name)
                    && false == btn.Name.Equals(BtnSelectToolX4.Name)
                    && false == btn.Name.Equals(BtnSelectToolY4.Name)
                    && false == btn.Name.Equals(BtnSelectToolZ4.Name)
                    && false == btn.Name.Equals(BtnSelectToolRz4.Name)
                    )
                {
                    btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                    btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                    btn.BackColorChanged += NonClickableButton_BackColorChanged;
                    btn.Cursor = Cursors.Default;
                }
            }
        }

        private void SetResourceControl()
        {
            // 현재 유저 권한 레벨 받아옴
            CUserInformation objUserInformation = mDocument.GetUserInformation();

            // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == mDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                // 설정값 조회만 가능한 상태
                SetControlButtonEnable(Controls, false);
                foreach (ImageButton btn in mBtnRobotIndex)
                {
                    btn.Enabled = true;
                }
                BtnProcessSelect00.Enabled = true;
                BtnProcessSelect01.Enabled = true;
                BtnProcessSelect02.Enabled = true;
                BtnProcessSelect03.Enabled = true;
                BtnProcessSelect04.Enabled = true;
                BtnProcessSelect05.Enabled = true;
                BtnProcessSelect06.Enabled = true;
                BtnProcessSelect07.Enabled = true;
                BtnProcessSelect08.Enabled = true;
                BtnProcessSelect09.Enabled = true;
                BtnToolPageFirst.Enabled = true;
                BtnToolPagePrivious.Enabled = true;
                BtnToolPageNext.Enabled = true;
                BtnToolPageLast.Enabled = true;
            }
            // 설비 정지 상태
            else
            {
                // 권한 레벨에 따라 원하는 방향으로 사용
                switch (objUserInformation.m_eAuthorityLevel)
                {
                    case CDefine.EUserAuthorityLevel.OPERATOR:
                        // 설정값 조회만 가능한 상태
                        SetControlButtonEnable(Controls, false);
                        foreach (ImageButton btn in mBtnRobotIndex)
                        {
                            btn.Enabled = true;
                        }
                        BtnProcessSelect00.Enabled = true;
                        BtnProcessSelect01.Enabled = true;
                        BtnProcessSelect02.Enabled = true;
                        BtnProcessSelect03.Enabled = true;
                        BtnProcessSelect04.Enabled = true;
                        BtnProcessSelect05.Enabled = true;
                        BtnProcessSelect06.Enabled = true;
                        BtnProcessSelect07.Enabled = true;
                        BtnProcessSelect08.Enabled = true;
                        BtnProcessSelect09.Enabled = true;
                        BtnToolPageFirst.Enabled = true;
                        BtnToolPagePrivious.Enabled = true;
                        BtnToolPageNext.Enabled = true;
                        BtnToolPageLast.Enabled = true;
                        break;
                    case CDefine.EUserAuthorityLevel.ENGINEER:
                        SetControlButtonEnable(Controls, true);
                        break;
                    case CDefine.EUserAuthorityLevel.MASTER:
                        SetControlButtonEnable(Controls, true);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <returns></returns>
        public bool SetChangeLanguage()
        {
            if (mResourceOffsets == null)
            {
                mResourceOffsets = new Dictionary<EOffset, string>();
                foreach (EOffset item in Enum.GetValues(typeof(EOffset)))
                {
                    mResourceOffsets[item] = string.Empty;
                }
            }
            foreach (EOffset item in Enum.GetValues(typeof(EOffset)))
            {
                mResourceOffsets[item] = mDocument.GetDatabaseUIText($"{nameof(mResourceOffsets)}[{item}]", Name);
            }
            mResourceTitleSelectItem = mDocument.GetDatabaseUIText(nameof(mResourceTitleSelectItem), Name);
            SetButtonChangeLanguage(BtnTitle);
            SetButtonChangeLanguage(BtnSelectProcessTitle);
            foreach (ImageButton btn in mBtnRobotIndex)
            {
                // 이름 규칙: $"{nameof(mBtnRobotIndex)}_{i}"
                SetButtonChangeLanguage(btn);
            }
            SetButtonChangeLanguage(BtnTitleCoordinateSystem);
            SetButtonChangeLanguage(BtnRecipeNoTitle);
            SetButtonChangeLanguage(BtnOverrideSpeedTitle);
            SetButtonChangeLanguage(BtnLoad);
            SetButtonChangeLanguage(BtnSave);
            SetButtonChangeLanguage(BtnToolPageFirst);
            SetButtonChangeLanguage(BtnToolPagePrivious);
            SetButtonChangeLanguage(BtnToolPageNext);
            SetButtonChangeLanguage(BtnToolPageLast);
            foreach (var item in mRobotProcessItems.Values.SelectMany(item => item).Where(item => item.IsUsing))
            {
                if (null == item)
                {
                    continue;
                }
                item.Name = mDocument.GetDatabaseUIText(item.Uiid, Name);
            }

            return true;
        }


        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(Button objButton)
        {
            base.SetButtonText(objButton, mDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        /// <summary>
        /// 언어 변경
        /// </summary>
        /// <param name="objButton"></param>
        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            base.SetButtonText(objButton, mDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        /// <summary>
        /// 타이머 유무
        /// </summary>
        /// <param name="bTimer"></param>
        public void SetTimer(bool bTimer)
        {
            timer.Enabled = bTimer;
        }

        /// <summary>
        /// Visible 유무
        /// </summary>
        /// <param name="bVisible"></param>
        public void SetVisible(bool bVisible)
        {
            Visible = bVisible;

            if (true == bVisible)
            {
                mbParameterChanged = false;
                selectRobot(0);

                // 설비 상태 or 권한 레벨에 따라 자원 상태 변경
                SetResourceControl();
                // 해당 폼을 말단으로 설정
                mDocument.GetMainFrame().SetCurrentForm(this);
            }
        }

        /// <summary>
        /// 타이머 동작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // 선택 아이템 표시
            SetButtonText(BtnSelectItemTitle, $"{mResourceTitleSelectItem} [{mSelectRobotIndex}] [{mSelectProcessItems[mSelectProcessItemIndex].ProcessIndex}]");

            // 로봇 인덱스 표시
            foreach (ImageButton btn in mBtnRobotIndex)
            {
                SetButtonBackColor(btn, mSelectRobotIndex == (CProcessMotion.ERobot)btn.Tag ? m_colorClick : m_colorNormal);
            }

            // 프로세스 인덱스 표시
            ERobotProcess selectProcessIndex = mSelectProcessItems[mSelectProcessItemIndex].ProcessIndex;
            foreach (ERobotProcess item in Enum.GetValues(typeof(ERobotProcess)))
            {
                Color buttonColor = selectProcessIndex == item ? m_colorClick : m_colorNormal;
                var findItem = mSelectProcessItems.Where(p => p.ProcessIndex == item).ToArray();
                string buttonTitle = findItem.Length > 0 ? findItem[0].Name : string.Empty;
                switch (item)
                {
                    case ERobotProcess.P1:
                        SetButtonBackColor(BtnProcessSelect00, buttonColor);
                        SetButtonText(BtnProcessSelect00, $"[P1] {buttonTitle}");
                        break;
                    case ERobotProcess.P2:
                        SetButtonBackColor(BtnProcessSelect01, buttonColor);
                        SetButtonText(BtnProcessSelect01, $"[P2] {buttonTitle}");
                        break;
                    case ERobotProcess.P3:
                        SetButtonBackColor(BtnProcessSelect02, buttonColor);
                        SetButtonText(BtnProcessSelect02, $"[P3] {buttonTitle}");
                        break;
                    case ERobotProcess.P4:
                        SetButtonBackColor(BtnProcessSelect03, buttonColor);
                        SetButtonText(BtnProcessSelect03, $"[P4] {buttonTitle}");
                        break;
                    case ERobotProcess.P5:
                        SetButtonBackColor(BtnProcessSelect04, buttonColor);
                        SetButtonText(BtnProcessSelect04, $"[P5] {buttonTitle}");
                        break;
                    case ERobotProcess.P6:
                        SetButtonBackColor(BtnProcessSelect05, buttonColor);
                        SetButtonText(BtnProcessSelect05, $"[P6] {buttonTitle}");
                        break;
                    case ERobotProcess.P7:
                        SetButtonBackColor(BtnProcessSelect06, buttonColor);
                        SetButtonText(BtnProcessSelect06, $"[P7] {buttonTitle}");
                        break;
                    case ERobotProcess.P8:
                        SetButtonBackColor(BtnProcessSelect07, buttonColor);
                        SetButtonText(BtnProcessSelect07, $"[P8] {buttonTitle}");
                        break;
                    case ERobotProcess.P9:
                        SetButtonBackColor(BtnProcessSelect08, buttonColor);
                        SetButtonText(BtnProcessSelect08, $"[P9] {buttonTitle}");
                        break;
                    case ERobotProcess.P10:
                        SetButtonBackColor(BtnProcessSelect09, buttonColor);
                        SetButtonText(BtnProcessSelect09, $"[P10] {buttonTitle}");
                        break;
                }
            }

            // 툴 데이터 표시
            {
                for (int i = 0; i < mPnlToolBases.Length; i++)
                {
                    if (mPnlToolBases[i].Visible == false)
                    {
                        continue;
                    }
                    int index = (mPageIndex * MAX_PAGE_ITEM_COUNT) + i;
                    if (index >= mSelectProcessItems[mSelectProcessItemIndex].DisplayItems.Count)
                    {
                        continue;
                    }
                    EOffset offsetIndex = mSelectProcessItems[mSelectProcessItemIndex].DisplayItems[index];
                    SetButtonText(mBtnToolTitles[i], mResourceOffsets[offsetIndex]);
                    for (int j = 0; j < mBtnToolData[i].Length; j++)
                    {
                        CConfig.ERobotOffset robotOffsetIndex = mSelectProcessItems[mSelectProcessItemIndex].ToolOffests[offsetIndex][j];
                        SetButtonText(mBtnToolData[i][j], $"{mSelectModelParameter.OffsetData[robotOffsetIndex]:0.000} ( {mUnitNames[j]} )");
                    }
                }
            }

            // 툴 데이터 페이지 표시
            SetButtonText(BtnToolPage, $"PAGE ( {mPageIndex + 1} / {mPageCount} )");

            // 레시피 번호 표시
            SetButtonText(BtnRecipeNo, $"{mSelectModelParameter.RecipeNo}");

            // 오버라이드 속도 표시
            SetButtonText(BtnOverrideSpeed, $"{mSelectModelParameter.OverrideSpeed} ( % )");
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == mDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                return;
            }
            if (DialogResult.Yes != mDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_SAVE))
            {
                return;
            }

            mbParameterChanged = checkParameterIsChanged();
            if (mbParameterChanged == true)
            {
                mDocument.m_objProcessMain.m_objProcessMotion.m_objRobot[mSelectRobotIndex].IsNeedSendOffsetData = true;
                mDocument.m_objProcessMain.m_objProcessMotion.m_objRobot[mSelectRobotIndex].IsNeedReceiveRmsData = true;
            }

            mDocument.SetUpdateButtonLog(this, string.Format($"[{nameof(BtnSave_Click)}] [Robot:{mSelectRobotIndex}]"));
            mDocument.m_objConfig.SaveNachiModelParameter(mSelectRobotIndex, mSelectModelParameter);

            // 로봇이 매뉴얼 모드면 옵셋 데이터를 전송한다
            var offsetParameter = mDocument.m_objConfig.GetNachiModelParameter(mSelectRobotIndex).DeepClone();
            // 오버라이드 속도 업데이트
            mDocument.m_objProcessMain.m_objProcessMotion.m_objRobot[mSelectRobotIndex].SetRobotOverrideSpeed(offsetParameter.OverrideSpeed);
            mDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.SAVING_IS_COMPLETE);
            mbParameterChanged = false;
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == mDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                return;
            }
            if (DialogResult.Yes != mDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.DO_YOU_WANT_TO_LOAD))
            {
                return;
            }

            mDocument.SetUpdateButtonLog(this, string.Format($"[{nameof(BtnLoad_Click)}] [Robot:{mSelectRobotIndex}]"));
            mSelectModelParameter = mDocument.m_objConfig.GetNachiModelParameter(mSelectRobotIndex).DeepClone();

            mbParameterChanged = checkParameterIsChanged();
            mDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.LOADING_IS_COMPLETE);
        }

        private void BtnSetRecipe_Click(object sender, EventArgs e)
        {
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == mDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                return;
            }

            int setRecipeNo = mSelectModelParameter.RecipeNo;
            using (var objKeyPad = new FormKeyPad(setRecipeNo, 1, 31))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    int result = Convert.ToInt32(objKeyPad.m_dResultValue);
                    mSelectModelParameter.RecipeNo = result;
                    mDocument.SetUpdateButtonLog(this, string.Format($"[{nameof(BtnSetRecipe_Click)}] [Robot:{mSelectRobotIndex}] [SetValue:{mSelectModelParameter.RecipeNo}]"));
                }
            }
            mbParameterChanged = checkParameterIsChanged();
        }

        private void BtnOverrideSpeed_Click(object sender, EventArgs e)
        {
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == mDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                return;
            }

            int setRecipeNo = mSelectModelParameter.OverrideSpeed;
            using (var objKeyPad = new FormKeyPad(setRecipeNo, 0, 100))
            {
                if (DialogResult.OK == objKeyPad.ShowDialog())
                {
                    int result = Convert.ToInt32(objKeyPad.m_dResultValue);
                    mSelectModelParameter.OverrideSpeed = result;
                    mDocument.SetUpdateButtonLog(this, string.Format($"[{nameof(BtnOverrideSpeed_Click)}] [Robot:{mSelectRobotIndex}] [SetValue:{mSelectModelParameter.OverrideSpeed}]"));
                }
            }
            mbParameterChanged = checkParameterIsChanged();
        }

        private void BtnSelectToolN_Click(object sender, EventArgs e)
        {
            if (CCIMDefine.EMoveState.MOVE_STATE_RUNNING == mDocument.m_eMoveState[(int)CCIMDefine.EPresentState.CURRENT_STATE])
            {
                return;
            }
            int selectIndex = (int)(sender as SpeedButton).Tag;
            int itemIndex = selectIndex % 10;
            int toolIndex = selectIndex / 10;
            int index = (mPageIndex * MAX_PAGE_ITEM_COUNT) + toolIndex;
            if (index >= mSelectProcessItems[mSelectProcessItemIndex].DisplayItems.Count)
            {
                return;
            }
            EOffset offsetIndex = mSelectProcessItems[mSelectProcessItemIndex].DisplayItems[index];
            CConfig.ERobotOffset robotOffsetIndex = mSelectProcessItems[mSelectProcessItemIndex].ToolOffests[offsetIndex][itemIndex];
            double setOffsetData = mSelectModelParameter.OffsetData[robotOffsetIndex];
            double inputRange = Math.Abs(1d);
            switch (itemIndex)
            {
                case 0:
                    inputRange = Math.Abs(mDocument.m_objConfig.GetOptionParameter().dNahciOffsetInterlockX);
                    break;
                case 1:
                    inputRange = Math.Abs(mDocument.m_objConfig.GetOptionParameter().dNahciOffsetInterlockY);
                    break;
                case 2:
                    inputRange = Math.Abs(mDocument.m_objConfig.GetOptionParameter().dNahciOffsetInterlockZ);
                    break;
                case 3:
                    inputRange = Math.Abs(mDocument.m_objConfig.GetOptionParameter().dNahciOffsetInterlockRz);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            using (FormKeyPad objKeyPad = new FormKeyPad(setOffsetData, -1d * inputRange, inputRange))
            {
                if (objKeyPad.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                mSelectModelParameter.OffsetData[robotOffsetIndex] = Convert.ToDouble($"{objKeyPad.m_dResultValue:0.000}");
                mDocument.SetUpdateButtonLog(this, string.Format($"[{nameof(BtnOverrideSpeed_Click)}] [Robot:{mSelectRobotIndex}] [Position:{robotOffsetIndex}] [SetValue:{mSelectModelParameter.OffsetData[robotOffsetIndex]}]"));
            }
            mbParameterChanged = checkParameterIsChanged();
        }

        private void BtnRobotIndex_Click(object sender, EventArgs e)
        {
            CProcessMotion.ERobot selectIndex = (CProcessMotion.ERobot)(sender as ImageButton).Tag;
            selectRobot(selectIndex);
        }

        private void BtnProcessSelect_Click(object sender, EventArgs e)
        {
            ERobotProcess selectIndex = (ERobotProcess)(sender as ImageButton).Tag;
            selectProcessIndex(selectIndex);
            selectPageIndex(0);
        }

        private void BtnToolPageFirst_Click(object sender, EventArgs e)
        {
            selectPageIndex(0);
        }

        private void BtnToolPagePrivious_Click(object sender, EventArgs e)
        {
            selectPageIndex(mPageIndex - 1);
        }

        private void BtnToolPageNext_Click(object sender, EventArgs e)
        {
            selectPageIndex(mPageIndex + 1);
        }

        private void BtnToolPageLast_Click(object sender, EventArgs e)
        {
            selectPageIndex(mPageCount - 1);
        }

        private void selectRobot(CProcessMotion.ERobot selectIndex)
        {
            if (mbParameterChanged == true)
            {
                if (DialogResult.Yes == mDocument.SetMessage(CDefine.EAlarmType.ALARM_QUESTION, CAlarmDefine.EMessageList.YOUR_DATA_HAS_BEEN_CHANGED_DO_YOU_WANT_TO_SAVE_THE_CHANGES))
                {
                    mDocument.m_objConfig.SaveNachiModelParameter(mSelectRobotIndex, mSelectModelParameter);
                    mDocument.SetMessage(CDefine.EAlarmType.ALARM_INFORMATION, CAlarmDefine.EMessageList.SAVING_IS_COMPLETE);
                }
                mbParameterChanged = false;
            }

            mDocument.SetUpdateButtonLog(this, string.Format($"[{nameof(selectRobot)}] [Robot:{mSelectRobotIndex}] [SetValue:{selectIndex}]"));
            mSelectRobotIndex = selectIndex;
            mSelectProcessItems = mRobotProcessItems[mSelectRobotIndex];
            mSelectModelParameter = mDocument.m_objConfig.GetNachiModelParameter(mSelectRobotIndex).DeepClone();
            selectProcessIndex(0);
            selectPageIndex(0);

            foreach (ERobotProcess item in Enum.GetValues(typeof(ERobotProcess)))
            {
                bool bEnable = false;
                if (mSelectProcessItems.Where(p => p.IsUsing && p.ProcessIndex == item).Count() > 0)
                {
                    bEnable = true;
                }
                switch (item)
                {
                    case ERobotProcess.P1:
                        BtnProcessSelect00.Visible = bEnable;
                        break;
                    case ERobotProcess.P2:
                        BtnProcessSelect01.Visible = bEnable;
                        break;
                    case ERobotProcess.P3:
                        BtnProcessSelect02.Visible = bEnable;
                        break;
                    case ERobotProcess.P4:
                        BtnProcessSelect03.Visible = bEnable;
                        break;
                    case ERobotProcess.P5:
                        BtnProcessSelect04.Visible = bEnable;
                        break;
                    case ERobotProcess.P6:
                        BtnProcessSelect05.Visible = bEnable;
                        break;
                    case ERobotProcess.P7:
                        BtnProcessSelect06.Visible = bEnable;
                        break;
                    case ERobotProcess.P8:
                        BtnProcessSelect07.Visible = bEnable;
                        break;
                    case ERobotProcess.P9:
                        BtnProcessSelect08.Visible = bEnable;
                        break;
                    case ERobotProcess.P10:
                        BtnProcessSelect09.Visible = bEnable;
                        break;
                }
            }
        }

        private void selectProcessIndex(ERobotProcess selectIndex)
        {
            mDocument.SetUpdateButtonLog(this, string.Format($"[{nameof(selectProcessIndex)}] [Robot:{mSelectRobotIndex}] [SetValue:{selectIndex}]"));

            mSelectProcessItemIndex = 0;
            foreach (var item in mSelectProcessItems)
            {
                if (item.ProcessIndex == selectIndex)
                {
                    break;
                }
                mSelectProcessItemIndex++;
            }
            if (mSelectProcessItems.Length <= mSelectProcessItemIndex)
            {
                mSelectProcessItemIndex = 0;
            }
            mPageCount = (mSelectProcessItems[mSelectProcessItemIndex].DisplayItems.Count - 1) / MAX_PAGE_ITEM_COUNT + 1;

            switch (mSelectProcessItems[mSelectProcessItemIndex].CoordinateSystem)
            {
                case ECoordinateSystem.XAxisR0:
                    picCoordinateSystem.Image = Properties.Resources.RobotCoordinateX000;
                    break;
                case ECoordinateSystem.XAxisR90:
                    picCoordinateSystem.Image = Properties.Resources.RobotCoordinateX090;
                    break;
                case ECoordinateSystem.XAxisR180:
                    picCoordinateSystem.Image = Properties.Resources.RobotCoordinateX180;
                    break;
                case ECoordinateSystem.XAxisR270:
                    picCoordinateSystem.Image = Properties.Resources.RobotCoordinateX270;
                    break;
            }
        }

        private void selectPageIndex(int selectPageIndex)
        {
            mDocument.SetUpdateButtonLog(this, string.Format($"[{nameof(selectPageIndex)}] [Robot:{mPageIndex}] [SetValue:{selectPageIndex}]"));

            if (selectPageIndex < 0)
            {
                selectPageIndex = 0;
            }
            else if (selectPageIndex > (mPageCount - 1))
            {
                selectPageIndex = mPageCount - 1;
            }
            mPageIndex = selectPageIndex;

            for (int i = 0; i < MAX_PAGE_ITEM_COUNT; i++)
            {
                int index = (mPageIndex * MAX_PAGE_ITEM_COUNT) + i;
                if (index >= mSelectProcessItems[mSelectProcessItemIndex].DisplayItems.Count)
                {
                    mPnlToolBases[i].Visible = false;
                    continue;
                }
                mPnlToolBases[i].Visible = true;
            }
        }

        private bool checkParameterIsChanged()
        {
            var current = mDocument.m_objConfig.GetNachiModelParameter(mSelectRobotIndex);
            if (mSelectModelParameter.RecipeNo != current.RecipeNo)
            {
                return true;
            }
            if (mSelectModelParameter.OverrideSpeed != current.OverrideSpeed)
            {
                return true;
            }
            foreach (var item in mSelectModelParameter.OffsetData)
            {
                if (item.Value != current.OffsetData[item.Key])
                {
                    return true;
                }
            }

            return false;
        }
    }
}