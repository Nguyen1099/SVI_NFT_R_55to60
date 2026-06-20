using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using UiAsset;

namespace SVI_NFT_R
{
    public partial class CDialogEQPFunctionList : CFormCommon, CFormInterface
    {
        public enum EFName
        {
            EF_NAME_CELL_TRACKING = 0,
            EF_NAME_TRACKING_CONTROL,
            EF_NAME_MATERIAL_TRACKING,
            EF_NAME_CELL_MCR_MODE,
            EF_NAME_MATERIAL_MCR_MODE,
            EF_NAME_LOT_ASSIGN_INFO,
            EF_NAME_AGV_ACCESS_MODE,
            EF_NAME_AREA_SENSOR_MODE,
            EF_NAME_SORT_MODE,
            EF_NAME_INTERLOCK_CONTROL,
            //LOADER (LI01) LOAD PORT MCR MODE
            EF_NAME_LOADER_LOAD_PORT_MCR_MODE,
            //LOADER (LS01) USE PORT MCR MODE
            EF_NAME_LOADER_USE_PORT_MCR_MODE,
            //UNLOADER USE PORT MCR MODE
            EF_NAME_UNLOADER_USE_PORT_MCR_MODE,
            //APC MODE
            EF_NAME_APC_MODE,
            //MULTI PASS MODE
            EF_NAME_MULTI_PASS_MODE,
            EF_NAME_FINAL
        }
        public enum EFSTOnOff
        {
            ON = 1,
            OFF,
            NOTHING,
            EFST_ONOFF_FINAL
        }
        public enum EFSTOnOffTrace
        {
            ON = 1,
            OFF,
            TRACE,
            NOTHING,
            EFST_ONOFF_FINAL
        }
        public enum EFSTUse
        {
            USE = 1,
            UNUSE,
            NOTHING,
            EFST_USE_FINAL
        }
        public enum EFSTAutoManual
        {
            AUTO = 1,
            MANUAL,
            NOTHING,
            EFST_AUTO_MANUAL_FINAL
        }
        public enum EFSTTrackingControl
        {
            TKIN = 1,
            TKOUT,
            BOTH,
            NOTHING,
            EFST_TRACNING_FINAL
        }
        public enum EFSTInterlockControl
        {
            TRANSFER = 1,
            LOADING,
            STEP,
            OWN,
            EFST_INTERLOCK_FINAL
        }
        private class EfstButtonTagSet
        {
            public EFName EFID { get; private set; }
            public string Value { get; set; }
            public Button SelectButton { get; private set; }
            public Button IndicatorButton { get; private set; }

            public EfstButtonTagSet(EFName efid, object value, Button selectButton, Button indicatorButton)
            {
                EFID = efid;
                Value = value.ToString();
                SelectButton = selectButton;
                IndicatorButton = indicatorButton;

                SelectButton.Tag = this;
                IndicatorButton.Tag = this;
            }
        }
        private readonly List<EfstButtonTagSet> mEfstButtonTagItems = new List<EfstButtonTagSet>();
        private string[] mInternalEfstValues;
        private CDocument m_objDocument;

        public CDialogEQPFunctionList(CDocument objDocument)
        {
            InitializeComponent();
            m_objDocument = objDocument;
        }

        /// <summary>
        /// 설비에서 EFID, EFST의 조합을 지원하는지 여부를 반환함
        /// </summary>
        /// <param name="efid">EFID</param>
        /// <param name="efst">EFST</param>
        /// <returns>설비 지원 여부</returns>
        public static bool CanEditEfstCombination(EFName efid, object efst)
        {
            if (efid == EFName.EF_NAME_MULTI_PASS_MODE)
            {
                return false;
            }

            string key = $"{efid}+{efst}";
            switch (key)
            {
                case "EF_NAME_CELL_TRACKING+ON":
                case "EF_NAME_CELL_TRACKING+OFF":
                //case "EF_NAME_CELL_TRACKING+TRACE":
                case "EF_NAME_CELL_TRACKING+NOTHING":
                    return true;

                case "EF_NAME_TRACKING_CONTROL+TKIN":
                //case "EF_NAME_TRACKING_CONTROL+TKOUT":
                //case "EF_NAME_TRACKING_CONTROL+BOTH":
                case "EF_NAME_TRACKING_CONTROL+NOTHING":
                    return true;

                //case "EF_NAME_MATERIAL_TRACKING+ON":
                //case "EF_NAME_MATERIAL_TRACKING+OFF":
                case "EF_NAME_MATERIAL_TRACKING+NOTHING":
                    return true;

                case "EF_NAME_CELL_MCR_MODE+USE":
                case "EF_NAME_CELL_MCR_MODE+UNUSE":
                    //case "EF_NAME_CELL_MCR_MODE+NOTHING":
                    return true;

                //case "EF_NAME_MATERIAL_MCR_MODE+USE":
                //case "EF_NAME_MATERIAL_MCR_MODE+UNUSE":
                case "EF_NAME_MATERIAL_MCR_MODE+NOTHING":
                    return true;

                //case "EF_NAME_LOT_ASSIGN_INFO+AUTO":
                //case "EF_NAME_LOT_ASSIGN_INFO+MANUAL":
                case "EF_NAME_LOT_ASSIGN_INFO+NOTHING":
                    return true;

                //case "EF_NAME_AGV_ACCESS_MODE+AUTO":
                //case "EF_NAME_AGV_ACCESS_MODE+MANUAL":
                case "EF_NAME_AGV_ACCESS_MODE+NOTHING":
                    return true;

                //case "EF_NAME_AREA_SENSOR_MODE+USE":
                //case "EF_NAME_AREA_SENSOR_MODE+UNUSE":
                case "EF_NAME_AREA_SENSOR_MODE+NOTHING":
                    return true;

                //case "EF_NAME_SORT_MODE+USE":
                //case "EF_NAME_SORT_MODE+UNUSE":
                case "EF_NAME_SORT_MODE+NOTHING":
                    return true;

                //case "EF_NAME_INTERLOCK_CONTROL+TRANSFER":
                case "EF_NAME_INTERLOCK_CONTROL+LOADING":
                case "EF_NAME_INTERLOCK_CONTROL+STEP":
                    //case "EF_NAME_INTERLOCK_CONTROL+OWN":
                    return true;

                //case "EF_NAME_LOADER_LOAD_PORT_MCR_MODE+ON":
                //case "EF_NAME_LOADER_LOAD_PORT_MCR_MODE+OFF":
                case "EF_NAME_LOADER_LOAD_PORT_MCR_MODE+NOTHING":
                    return true;

                //case "EF_NAME_LOADER_USE_PORT_MCR_MODE+ON":
                //case "EF_NAME_LOADER_USE_PORT_MCR_MODE+OFF":
                case "EF_NAME_LOADER_USE_PORT_MCR_MODE+NOTHING":
                    return true;

                //case "EF_NAME_UNLOADER_USE_PORT_MCR_MODE+ON":
                //case "EF_NAME_UNLOADER_USE_PORT_MCR_MODE+OFF":
                case "EF_NAME_UNLOADER_USE_PORT_MCR_MODE+NOTHING":
                    return true;

                //case "EF_NAME_APC_MODE+AUTO":
                //case "EF_NAME_APC_MODE+MANUAL":
                case "EF_NAME_APC_MODE+NOTHING":
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// EFID에 EFST 기본 값을 반환 함
        /// </summary>
        /// <param name="efid">EFID</param>
        /// <returns>EFST 기본 값</returns>
        public static string GetEfstDefaultValue(EFName efid)
        {
            switch (efid)
            {
                case EFName.EF_NAME_CELL_TRACKING:
                    return EFSTOnOffTrace.ON.ToString();

                case EFName.EF_NAME_TRACKING_CONTROL:
                    return EFSTTrackingControl.TKIN.ToString();

                case EFName.EF_NAME_CELL_MCR_MODE:
                    return EFSTUse.USE.ToString();

                case EFName.EF_NAME_INTERLOCK_CONTROL:
                    return EFSTInterlockControl.LOADING.ToString();

                case EFName.EF_NAME_APC_MODE:
                    return EFSTAutoManual.NOTHING.ToString();

                case EFName.EF_NAME_MULTI_PASS_MODE:
                    return "0";

                default:
                    return EFSTOnOff.NOTHING.ToString();
            }
        }

        /// <summary>
        /// EFID 1, 2, 4 항목 값 변경시 전이도를 적용하고 값을 업데이트 함
        /// </summary>
        /// <param name="efid">EFID</param>
        /// <param name="efstValue">NEW EFST</param>
        /// <param name="efstValues">EFST 값이 저장된 배열</param>
        public static void UpdateEfstValue(EFName efid, string efstValue, ref string[] efstValues)
        {
            if (efid == EFName.EF_NAME_FINAL)
            {
                return;
            }
            // 값이 변경 되지 않은 경우
            if (efstValues[(int)efid] == efstValue)
            {
                return;
            }

            // 전이도 적용
            switch (efid)
            {
                case EFName.EF_NAME_CELL_TRACKING:
                    if (efstValue == EFSTOnOffTrace.ON.ToString())
                    {
                        efstValues[(int)EFName.EF_NAME_TRACKING_CONTROL] = EFSTTrackingControl.TKIN.ToString();
                        if (efstValues[(int)EFName.EF_NAME_CELL_MCR_MODE] == EFSTUse.UNUSE.ToString())
                        {
                            efstValues[(int)EFName.EF_NAME_CELL_MCR_MODE] = EFSTUse.USE.ToString();
                        }
                    }
                    else
                    {
                        efstValues[(int)EFName.EF_NAME_TRACKING_CONTROL] = EFSTTrackingControl.NOTHING.ToString();
                        if (efstValue == EFSTOnOffTrace.NOTHING.ToString() && efstValues[(int)EFName.EF_NAME_CELL_MCR_MODE] == EFSTUse.USE.ToString())
                        {
                            efstValues[(int)EFName.EF_NAME_CELL_MCR_MODE] = EFSTUse.UNUSE.ToString();
                        }
                    }
                    break;

                case EFName.EF_NAME_TRACKING_CONTROL:
                    if (efstValue == EFSTTrackingControl.NOTHING.ToString())
                    {
                        if (efstValues[(int)EFName.EF_NAME_CELL_MCR_MODE] == EFSTUse.USE.ToString())
                        {
                            efstValues[(int)EFName.EF_NAME_CELL_TRACKING] = EFSTOnOffTrace.OFF.ToString();
                        }
                        else
                        {
                            efstValues[(int)EFName.EF_NAME_CELL_TRACKING] = EFSTOnOffTrace.NOTHING.ToString();
                        }
                    }
                    else
                    {
                        efstValues[(int)EFName.EF_NAME_CELL_TRACKING] = EFSTOnOffTrace.ON.ToString();
                        if (efstValues[(int)EFName.EF_NAME_CELL_MCR_MODE] == EFSTUse.UNUSE.ToString())
                        {
                            efstValues[(int)EFName.EF_NAME_CELL_MCR_MODE] = EFSTUse.USE.ToString();
                        }
                    }
                    break;

                case EFName.EF_NAME_CELL_MCR_MODE:
                    if (efstValue == EFSTUse.UNUSE.ToString())
                    {
                        efstValues[(int)EFName.EF_NAME_CELL_TRACKING] = EFSTOnOffTrace.NOTHING.ToString();
                        efstValues[(int)EFName.EF_NAME_TRACKING_CONTROL] = EFSTTrackingControl.NOTHING.ToString();
                    }
                    else if (efstValue == EFSTUse.USE.ToString())
                    {
                        efstValues[(int)EFName.EF_NAME_CELL_TRACKING] = EFSTOnOffTrace.ON.ToString();
                        efstValues[(int)EFName.EF_NAME_TRACKING_CONTROL] = EFSTTrackingControl.TKIN.ToString();
                    }
                    break;

                default:
                    break;
            }

            // 값 업데이트
            efstValues[(int)efid] = efstValue;
        }

        public bool SetChangeLanguage()
        {
            SetButtonChangeLanguage(BtnDescriptionCellTrackingOn);
            SetButtonChangeLanguage(BtnDescriptionCellTrackingOff);
            SetButtonChangeLanguage(BtnDescriptionCellTrackingNothing);

            SetButtonChangeLanguage(BtnDescriptionTrackingControlTKIN);
            SetButtonChangeLanguage(BtnDescriptionTrackingControlTKOUT);
            SetButtonChangeLanguage(BtnDescriptionTrackingControlBOTH);
            SetButtonChangeLanguage(BtnDescriptionTrackingControlNothing);

            SetButtonChangeLanguage(BtnDescriptionMaterialTrackingOn);
            SetButtonChangeLanguage(BtnDescriptionMaterialTrackingOff);
            SetButtonChangeLanguage(BtnDescriptionMaterialTrackingNothing);

            SetButtonChangeLanguage(BtnDescriptionCellMCRModeUse);
            SetButtonChangeLanguage(BtnDescriptionCellMCRModeUnuse);
            SetButtonChangeLanguage(BtnDescriptionCellMCRModeNothing);

            SetButtonChangeLanguage(BtnDescriptionMaterialMCRModeUse);
            SetButtonChangeLanguage(BtnDescriptionMaterialMCRModeUnuse);
            SetButtonChangeLanguage(BtnDescriptionMaterialMCRModeNothing);

            SetButtonChangeLanguage(BtnDescriptionLotAssignInfoAuto);
            SetButtonChangeLanguage(BtnDescriptionLotAssignInfoManual);
            SetButtonChangeLanguage(BtnDescriptionLotAssignInfoNothing);

            SetButtonChangeLanguage(BtnDescriptionAGVAccessModeAuto);
            SetButtonChangeLanguage(BtnDescriptionAGVAccessModeManual);
            SetButtonChangeLanguage(BtnDescriptionAGVAccessModeNothing);

            SetButtonChangeLanguage(BtnDescriptionAreaSensorModeUse);
            SetButtonChangeLanguage(BtnDescriptionAreaSensorModeUnuse);
            SetButtonChangeLanguage(BtnDescriptionAreaSensorModeNothing);

            SetButtonChangeLanguage(BtnDescriptionSortModeUse);
            SetButtonChangeLanguage(BtnDescriptionSortModeUnuse);
            SetButtonChangeLanguage(BtnDescriptionSortModeNothing);

            SetButtonChangeLanguage(BtnDescriptionInterlockControlTransfer);
            SetButtonChangeLanguage(BtnDescriptionInterlockControlLoading);
            SetButtonChangeLanguage(BtnDescriptionInterlockControlStep);
            SetButtonChangeLanguage(BtnDescriptionInterlockControlOwn);

            SetButtonChangeLanguage(BtnDescriptionLoaderLoadPortMcr_ON);
            SetButtonChangeLanguage(BtnDescriptionLoaderLoadPortMcr_OFF);
            SetButtonChangeLanguage(BtnDescriptionLoaderLoadPortMcr_Nothing);

            SetButtonChangeLanguage(BtnDescriptionLoaderUsePortMcr_ON);
            SetButtonChangeLanguage(BtnDescriptionLoaderUsePortMcr_OFF);
            SetButtonChangeLanguage(BtnDescriptionLoaderUsePortMcr_Nothing);

            SetButtonChangeLanguage(BtnDescriptionUnLoaderUsePortMcr_ON);
            SetButtonChangeLanguage(BtnDescriptionUnLoaderUsePortMcr_OFF);
            SetButtonChangeLanguage(BtnDescriptionUnLoaderUsePortMcr_Nothing);

            SetButtonChangeLanguage(BtnDescriptionAPCMode_Auto);
            SetButtonChangeLanguage(BtnDescriptionAPCMode_Manu);
            SetButtonChangeLanguage(BtnDescriptionAPCMode_Nothing);

            BtnMultiPassModeDescription.Text = string.Format(m_objDocument.GetDatabaseUIText(BtnMultiPassModeDescription.Name, Name).Replace("\\n", "\n"));

            SetButtonChangeLanguage(BtnConfirm);
            SetButtonChangeLanguage(BtnExit);

            return true;
        }

        public void SetTimer(bool bTimer)
        {
            if (true == bTimer)
            {
                timer.Enabled = true;
            }
            else
            {
                timer.Enabled = false;
            }
        }

        public void SetVisible(bool bVisible)
        {
        }

        private void CDialogEQPFunctionList_Load(object sender, EventArgs e)
        {
            Initialize();

            SetTimer(true);
        }

        private void CDialogEQPFunctionList_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeInitialize();
        }

        private bool Initialize()
        {
            bool bResult = false;
            do
            {
                // 다이얼로그 초기화.
                if (false == InitializeForm()) break;
                bResult = true;
            } while (false);

            return bResult;
        }

        private bool InitializeForm()
        {
            InitializeButtonTag();

            // 설비에서 지원하는 EFID, EFST에 조합에 따라 버튼을 활성화/비활성화 한다
            foreach (var tag in mEfstButtonTagItems)
            {
                SetButtonEnable(tag.SelectButton, CanEditEfstCombination(tag.EFID, tag.Value));
            }

            // 현재 버퍼에 값을 내부 배열에 복사
            var objCIMParameter = m_objDocument.m_objConfig.GetCimParameter();
            mInternalEfstValues = new string[(int)EFName.EF_NAME_FINAL];
            for (int iLoopCount = 0; iLoopCount < (int)EFName.EF_NAME_FINAL; iLoopCount++)
            {
                mInternalEfstValues[iLoopCount] = objCIMParameter.strEFValueBuffer[iLoopCount];
            }

            // 언어변환
            SetChangeLanguage();

            SetButtonColor();

            // 타이머 외부에서 제어
            timer.Interval = 100;
            timer.Enabled = false;

            return true;
        }

        private void InitializeButtonTag()
        {
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_CELL_TRACKING, EFSTOnOffTrace.ON, BtnCellTrackingOn, BtnSelectCellTrackingOn));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_CELL_TRACKING, EFSTOnOffTrace.OFF, BtnCellTrackingOff, BtnSelectCellTrackingOff));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_CELL_TRACKING, EFSTOnOffTrace.TRACE, BtnCellTrackingTrace, BtnSelectCellTrackingTrace));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_CELL_TRACKING, EFSTOnOffTrace.NOTHING, BtnCellTrackingNothing, BtnSelectCellTrackingNothing));

            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_TRACKING_CONTROL, EFSTTrackingControl.TKIN, BtnTrackingControlTkin, BtnSelectTrackingControlTkin));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_TRACKING_CONTROL, EFSTTrackingControl.TKOUT, BtnTrackingControlTkout, BtnSelectTrackingControlTkout));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_TRACKING_CONTROL, EFSTTrackingControl.BOTH, BtnTrackingControlBoth, BtnSelectTrackingControlBoth));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_TRACKING_CONTROL, EFSTTrackingControl.NOTHING, BtnTrackingControlNothing, BtnSelectTrackingControlNothing));

            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_MATERIAL_TRACKING, EFSTOnOff.ON, BtnMaterialTrackingOn, BtnSelectMaterialTrackingOn));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_MATERIAL_TRACKING, EFSTOnOff.OFF, BtnMaterialTrackingOff, BtnSelectMaterialTrackingOff));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_MATERIAL_TRACKING, EFSTOnOff.NOTHING, BtnMaterialTrackingNothing, BtnSelectMaterialTrackingNothing));

            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_CELL_MCR_MODE, EFSTUse.USE, BtnCellMcrModeUse, BtnSelectCellMcrModeUse));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_CELL_MCR_MODE, EFSTUse.UNUSE, BtnCellMcrModeUnuse, BtnSelectCellMcrModeUnuse));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_CELL_MCR_MODE, EFSTUse.NOTHING, BtnCellMcrModeNothing, BtnSelectCellMcrModeNothing));

            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_MATERIAL_MCR_MODE, EFSTUse.USE, BtnMaterialMcrModeUse, BtnSelectMaterialMcrModeUse));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_MATERIAL_MCR_MODE, EFSTUse.UNUSE, BtnMaterialMcrModeUnuse, BtnSelectMaterialMcrModeUnuse));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_MATERIAL_MCR_MODE, EFSTUse.NOTHING, BtnMaterialMcrModeNothing, BtnSelectMaterialMcrModeNothing));

            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_LOT_ASSIGN_INFO, EFSTAutoManual.AUTO, BtnLotAssignInfoAuto, BtnSelectLotAssignInfoAuto));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_LOT_ASSIGN_INFO, EFSTAutoManual.MANUAL, BtnLotAssignInfoManual, BtnSelectLotAssignInfoManual));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_LOT_ASSIGN_INFO, EFSTAutoManual.NOTHING, BtnLotAssignInfoNothing, BtnSelectLotAssignInfoNothing));

            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_AGV_ACCESS_MODE, EFSTAutoManual.AUTO, BtnAGVAccessModeAuto, BtnSelectAGVAccessModeAuto));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_AGV_ACCESS_MODE, EFSTAutoManual.MANUAL, BtnAGVAccessModeManual, BtnSelectAGVAccessModeManual));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_AGV_ACCESS_MODE, EFSTAutoManual.NOTHING, BtnAGVAccessModeNothing, BtnSelectAGVAccessModeNothing));

            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_AREA_SENSOR_MODE, EFSTUse.USE, BtnAreaSensorModeUse, BtnSelectAreaSensorModeUse));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_AREA_SENSOR_MODE, EFSTUse.UNUSE, BtnAreaSensorModeUnuse, BtnSelectAreaSensorModeUnuse));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_AREA_SENSOR_MODE, EFSTUse.NOTHING, BtnAreaSensorModeNothing, BtnSelectAreaSensorModeNothing));

            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_SORT_MODE, EFSTUse.USE, BtnSortModeUse, BtnSelectSortModeUse));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_SORT_MODE, EFSTUse.UNUSE, BtnSortModeUnuse, BtnSelectSortModeUnuse));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_SORT_MODE, EFSTUse.NOTHING, BtnSortModeNothing, BtnSelectSortModeNothing));

            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_INTERLOCK_CONTROL, EFSTInterlockControl.TRANSFER, BtnInterlockControlTransfer, BtnSelectInterlockControlTransfer));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_INTERLOCK_CONTROL, EFSTInterlockControl.LOADING, BtnInterlockControlLoading, BtnSelectInterlockControlLoading));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_INTERLOCK_CONTROL, EFSTInterlockControl.STEP, BtnInterlockControlStep, BtnSelectInterlockControlStep));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_INTERLOCK_CONTROL, EFSTInterlockControl.OWN, BtnInterlockControlOwn, BtnSelectInterlockControlOwn));

            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_LOADER_LOAD_PORT_MCR_MODE, EFSTOnOff.ON, BtnLoaderLoadPortMcr_ON, BtnSelectLoaderLoadPortMcr_ON));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_LOADER_LOAD_PORT_MCR_MODE, EFSTOnOff.OFF, BtnLoaderLoadPortMcr_OFF, BtnSelectLoaderLoadPortMcr_OFF));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_LOADER_LOAD_PORT_MCR_MODE, EFSTOnOff.NOTHING, BtnLoaderLoadPortMcr_Nothing, BtnSelectLoaderLoadPortMcr_Nothing));

            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_LOADER_USE_PORT_MCR_MODE, EFSTOnOff.ON, BtnLoaderUsePortMcr_ON, BtnSelectLoaderUsePortMcr_ON));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_LOADER_USE_PORT_MCR_MODE, EFSTOnOff.OFF, BtnLoaderUsePortMcr_OFF, BtnSelectLoaderUsePortMcr_OFF));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_LOADER_USE_PORT_MCR_MODE, EFSTOnOff.NOTHING, BtnLoaderUsePortMcr_Nothing, BtnSelectLoaderUsePortMcr_Nothing));

            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_UNLOADER_USE_PORT_MCR_MODE, EFSTOnOff.ON, BtnUnLoaderUsePortMcr_ON, BtnSelectUnLoaderUsePortMcr_ON));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_UNLOADER_USE_PORT_MCR_MODE, EFSTOnOff.OFF, BtnUnLoaderUsePortMcr_OFF, BtnSelectUnLoaderUsePortMcr_OFF));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_UNLOADER_USE_PORT_MCR_MODE, EFSTOnOff.NOTHING, BtnUnLoaderUsePortMcr_Nothing, BtnSelectUnLoaderUsePortMcr_Nothig));

            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_APC_MODE, EFSTAutoManual.AUTO, BtnAPCMode_Auto, BtnSelectAPCMode_Auto));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_APC_MODE, EFSTAutoManual.MANUAL, BtnAPCMode_Manu, BtnSelectAPCMode_Manu));
            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_APC_MODE, EFSTAutoManual.NOTHING, BtnAPCMode_Nothing, BtnSelectAPCMode_Nothing));

            mEfstButtonTagItems.Add(new EfstButtonTagSet(EFName.EF_NAME_MULTI_PASS_MODE, 0, BtnMultiPassMode, BtnSelectMultiPassMode));
        }

        private void DeInitialize()
        {
        }

        private void SetButtonChangeLanguage(Button objButton)
        {
            SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        private void SetButtonChangeLanguage(ImageButton objButton)
        {
            SetButtonText(objButton, m_objDocument.GetDatabaseUIText(objButton.Name, Name));
        }

        private void SetButtonColor()
        {
            SetButtonBackColor(BtnConfirm, m_colorNormal);
            SetButtonBackColor(BtnExit, m_colorNormal);

            HashSet<string> clickableButtonNames = new HashSet<string>(mEfstButtonTagItems.Select(i => i.SelectButton.Name));
            // 클릭 기능이 없는 버튼은 마우스를 올려도 색이 변하지 않도록 설정함
            var speedButtons = Controls.GetChildControlListByType(typeof(SpeedButton));
            foreach (SpeedButton btn in speedButtons.Cast<SpeedButton>())
            {
                if (null != btn
                    // 클릭이 기능이 있는 버튼은 스킵함
                    && false == clickableButtonNames.Contains(btn.Name)
                    )
                {
                    btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                    btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                    btn.BackColorChanged += NonClickableButton_BackColorChanged;
                    btn.Cursor = Cursors.Default;
                }

                // 배경색 설정
                if (9 <= btn.Name.Length
                    && true == btn.Name.Substring(0, 9).Equals("BtnHeader")
                    )
                {
                    SetButtonBackColor(btn, m_colorLabel);
                }
                else
                {
                    SetButtonBackColor(btn, m_colorLabelSub);
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var objCIMParameter = m_objDocument.m_objConfig.GetCimParameter();

            foreach (EfstButtonTagSet tag in mEfstButtonTagItems)
            {
                bool bCimSelected;
                bool bSelectValueMatched;
                switch (tag.EFID)
                {
                    case EFName.EF_NAME_MULTI_PASS_MODE:
                        bCimSelected = false;
                        bSelectValueMatched = false;
                        tag.Value = mInternalEfstValues[(int)tag.EFID];
                        break;

                    default:
                        bCimSelected = objCIMParameter.strEFValue[(int)tag.EFID] == tag.Value;
                        bSelectValueMatched = mInternalEfstValues[(int)tag.EFID] == tag.Value;
                        break;
                }

                // 현재 적용된 값 표시 업데이트
                if (tag.SelectButton.Enabled == true)
                {
                    SetButtonColor(tag.SelectButton, Color.Black, bCimSelected == true ? m_colorOn : m_colorNormal);
                    SetControlFontStyle(tag.SelectButton, FontStyle.Regular);
                }
                else
                {
                    SetButtonColor(tag.SelectButton, Color.Black, m_colorNormal);
                    SetControlFontStyle(tag.SelectButton, FontStyle.Strikeout);
                }
                SetButtonText(tag.SelectButton, tag.Value);
                // 선택 상태 표시 업데이트
                {
                    Color selectionColor = (objCIMParameter.strEFValueBuffer[(int)tag.EFID] == mInternalEfstValues[(int)tag.EFID]) ? m_colorOn : m_colorOrange;
                    SetButtonColor(tag.IndicatorButton, Color.Black, bSelectValueMatched == true ? selectionColor : m_colorNormal);
                }
            }
        }

        private void SetControlFontStyle(Control control, FontStyle fontStyle)
        {
            if (control.Font.Style == fontStyle)
            {
                return;
            }

            control.Font = new Font(control.Font, fontStyle);
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}]");
            bool bChanged = false;
            var objCIMParameter = m_objDocument.m_objConfig.GetCimParameter().DeepClone();
            do
            {
                for (int iLoopCount = 0; iLoopCount < (int)CCIMDefine.EFunctionID.EFID_FINAL - 1; iLoopCount++)
                {
                    if (mInternalEfstValues[iLoopCount] != objCIMParameter.strEFValueBuffer[iLoopCount])
                    {
                        m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [EFID: {(EFName)iLoopCount}] [EFST: {objCIMParameter.strEFValueBuffer[iLoopCount]} -> {mInternalEfstValues[iLoopCount]}]");
                        objCIMParameter.strEFValueBuffer[iLoopCount] = mInternalEfstValues[iLoopCount];
                        bChanged = true;
                    }
                }

                if (true == bChanged)
                {
                    m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] Save change value");
                    // 최종 변경 주체 업데이트
                    objCIMParameter.strEFValueChangeByWho = "EQP";
                    // 변경 리스트 적용
                    m_objDocument.m_objConfig.SaveCimParameter(objCIMParameter);

                    // 일단 버퍼에 변경사항을 적용하고 CIM 보고는 다른 스레드에서 체크하여 진행함
                }
                else
                {
                    m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] Close");
                    // 변경사항이 없으면 창을 닫는다
                    ActiveForm.Close();
                }
            } while (false);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] Close");
            ActiveForm.Close();
            //Hide();
        }

        private void BtnEfstValueEdit_Click(object sender, EventArgs e)
        {
            var tag = ((Button)sender).Tag as EfstButtonTagSet;
            if (tag == null)
            {
                return;
            }
            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [EFID: {tag.EFID}]");

            string settingValue;
            switch (tag.EFID)
            {
                case EFName.EF_NAME_MULTI_PASS_MODE:
                    {
                        int defaultValue;
                        int.TryParse(mInternalEfstValues[(int)tag.EFID], out defaultValue);
                        using (var objKeyPad = new FormKeyPad(defaultValue, 0, 100, "Multi Pass Mode"))
                        {
                            if (objKeyPad.ShowDialog() != DialogResult.OK)
                            {
                                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [EFID: {tag.EFID}] if (objKeyPad.ShowDialog() != DialogResult.OK)");
                                return;
                            }
                            int resultValue = (int)objKeyPad.m_dResultValue;
                            if (resultValue % 10 != 0)
                            {
                                m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [EFID: {tag.EFID}] [resultValue: {resultValue}] if (resultValue % 10 != 0)");
                                return;
                            }
                            settingValue = resultValue.ToString();
                        }
                    }
                    break;

                default:
                    settingValue = tag.Value;
                    break;
            }

            m_objDocument.SetUpdateButtonLog(this, $"[{MethodBase.GetCurrentMethod().Name}] [EFID: {tag.EFID}] [EFST: {mInternalEfstValues[(int)tag.EFID]} -> {settingValue}]");
            UpdateEfstValue(tag.EFID, settingValue, ref mInternalEfstValues);
        }
    }
}
