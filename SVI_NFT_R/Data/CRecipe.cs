using System;
using System.Diagnostics;

namespace SVI_NFT_R
{
    public class CRecipe
    {

        private CDocument m_objDocument;
        /// <summary>
        /// 모델 파라미터 리스트
        /// </summary>
        public CConfig.CModelParameter m_objModelParameter;
        /// <summary>
        /// 생성자
        /// </summary>
        public CRecipe(CDocument objDocument)
        {
            m_objDocument = objDocument;
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            bool bReturn = false;

            do
            {
                m_objModelParameter = m_objDocument.m_objModel.GetModelParameter(m_objDocument.m_objConfig.GetSystemParameter().strPPID);
                bReturn = true;
            } while (false);

            return bReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void DeInitialize()
        {
        }

        public void SendPpidChangeEvent(CCIMDefine.EPpidChangeMode eMode, CConfig.CModelParameter objModelParameter, string ppidSubscript)
        {
            switch (eMode)
            {
                // 삭제나 생성시에는 모든 파라메터를 보고한다
                case CCIMDefine.EPpidChangeMode.PPID_CHANGE_MODE_CREATE:
                case CCIMDefine.EPpidChangeMode.PPID_CHANGE_MODE_MODIFY:
                    {
                        PPChangeEvent objPPChangeEvent = new PPChangeEvent();
                        objPPChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                        objPPChangeEvent.BODY.MODE = string.Format("{0}", (int)eMode);
                        objPPChangeEvent.BODY.PPID = objModelParameter.strPPID;
                        objPPChangeEvent.BODY.PPID_TYPE = "1";
                        objPPChangeEvent.BODY.PPID_NUMBER = objModelParameter.Index.ToString() + ppidSubscript;
                        objPPChangeEvent.BODY.COMMANDS = new PPChangeEvent._PPChangeEvent.COMMAND[1];
                        objPPChangeEvent.BODY.COMMANDS[0].CCODE = string.Format("{0}", (int)eMode);
                        var parameterValues = Enum.GetValues(typeof(CCIMDefine.EPpidParamList));
                        objPPChangeEvent.BODY.COMMANDS[0].PARAMS = new PPChangeEvent._PPChangeEvent.COMMAND.PARAM[parameterValues.Length];
                        int index = 0;
                        foreach (CCIMDefine.EPpidParamList paramValue in parameterValues)
                        {
                            objPPChangeEvent.BODY.COMMANDS[0].PARAMS[index].PARAMNAME = paramValue.ToString();
                            objPPChangeEvent.BODY.COMMANDS[0].PARAMS[index].PARAMVALUE = objModelParameter[paramValue];
                            index++;
                        }
                        m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetPPChangeEvent(objPPChangeEvent);
                    }
                    break;
                case CCIMDefine.EPpidChangeMode.PPID_CHANGE_MODE_DELETE:
                    {
                        PPChangeEvent objPPChangeEvent = new PPChangeEvent();
                        objPPChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
                        objPPChangeEvent.BODY.MODE = string.Format("{0}", (int)eMode);
                        objPPChangeEvent.BODY.PPID = objModelParameter.strPPID;
                        objPPChangeEvent.BODY.PPID_TYPE = "1";
                        objPPChangeEvent.BODY.PPID_NUMBER = objModelParameter.Index.ToString() + ppidSubscript;
                        objPPChangeEvent.BODY.COMMANDS = new PPChangeEvent._PPChangeEvent.COMMAND[1];
                        objPPChangeEvent.BODY.COMMANDS[0].CCODE = string.Format("{0}", (int)eMode);
                        var parameterValues = Enum.GetValues(typeof(CCIMDefine.EPpidParamList));
                        objPPChangeEvent.BODY.COMMANDS[0].PARAMS = new PPChangeEvent._PPChangeEvent.COMMAND.PARAM[0];
                        m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetPPChangeEvent(objPPChangeEvent);
                    }
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        public void SetSaveRecipeAndReportCim(CConfig.CModelParameter setModelParameter)
        {
            string ppidSubscript = "C";
            CConfig.CModelParameter objModelParameter;
            if (null == setModelParameter)
            {
                objModelParameter = m_objModelParameter.DeepClone();
            }
            else
            {
                objModelParameter = setModelParameter;
            }
            int currentRevision;
            int.TryParse(m_objModelParameter.strSoftRevision, out currentRevision);
            objModelParameter.strSoftRevision = (++currentRevision).ToString();

            // 변경사항이 있을때에만 CIM에 보고함
            var parameterValues = Enum.GetValues(typeof(CCIMDefine.EPpidParamList));
            foreach (CCIMDefine.EPpidParamList paramValue in parameterValues)
            {
                if (false == Equals(m_objModelParameter[paramValue], objModelParameter[paramValue]))
                {
                    // CIM 보고
                    SendPpidChangeEvent(CCIMDefine.EPpidChangeMode.PPID_CHANGE_MODE_MODIFY, objModelParameter, ppidSubscript);
                    break;
                }
            }
            // 모델 파라미터 저장
            m_objDocument.m_objConfig.SaveModelParameter(objModelParameter);
            // 모델 파라미터 리스트 갱신
            m_objModelParameter = m_objDocument.m_objModel.GetModelParameter(objModelParameter.strPPID);
            // 진공 옵션 적용
            m_objDocument.m_objProcessMain.m_objProcessMotion.ApplyVacuumOptions();
            // EC 변경사항있는경우 보고됨
            m_objDocument.SetECList();
        }

        public void SetLoadRecipeAndReportCim(CConfig.CSystemParameter objSystemParameter, bool bCimReport)
        {
            string strOldPpid = m_objModelParameter.strPPID;
            m_objModelParameter = m_objDocument.m_objModel.GetModelParameter(objSystemParameter.strPPID);

            if (bCimReport == true)
            {
                // CIM 보고.
                CurrentPPIDChangeEvent objCurrentPPIDChangeEvent = new CurrentPPIDChangeEvent();
                objCurrentPPIDChangeEvent.EQPID = objSystemParameter.strEquipmentID;
                objCurrentPPIDChangeEvent.BODY.PPID = m_objModelParameter.strPPID;
                objCurrentPPIDChangeEvent.BODY.PPID_TYPE = "1";
                objCurrentPPIDChangeEvent.BODY.OLD_PPID = strOldPpid;
                m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetCurrentPPIDChangeEvent(objCurrentPPIDChangeEvent);
            }
            m_objDocument.m_objConfig.SaveSystemParameter(objSystemParameter);
            // 모델 파라미터 로드
            m_objDocument.m_objConfig.LoadModelParameter();
            // 모델 옵셋 파라미터 로드
            m_objDocument.m_objConfig.LoadModelOffsetParameter();
            // SDV Vision 파라미터 로드
            m_objDocument.m_objConfig.LoadInspOptionParameter();
            // Align Vision 파라미터 로드
            m_objDocument.m_objConfig.LoadAlignOptionParameter();
            // 모터 티칭 정보 파라미터 로드
            m_objDocument.m_objProcessMain.m_objProcessMotion.LoadTeachParameter();
            // 로봇 모델 파라미터 로드
            m_objDocument.m_objConfig.LoadNachiModelParameter();

            m_objDocument.m_objProcessMain.m_objProcessMotion.SetInitializeCommand(CProcessAbstract.EProcessManagerInitialize.PROCESS_MANAGER_INITIALIZE_NONE);

            // 진공 옵션 적용
            m_objDocument.m_objProcessMain.m_objProcessMotion.ApplyVacuumOptions();
            // EC 변경사항있는경우 보고됨
            m_objDocument.SetECList();
        }

        public void SetCurrentRecipeReportCim()
        {
            // 현재 Recipe를 Change 보고한다
            CurrentPPIDChangeEvent objCurrentPPIDChangeEvent = new CurrentPPIDChangeEvent();
            objCurrentPPIDChangeEvent.EQPID = m_objDocument.m_objConfig.GetSystemParameter().strEquipmentID;
            objCurrentPPIDChangeEvent.BODY.PPID = m_objModelParameter.strPPID;
            objCurrentPPIDChangeEvent.BODY.PPID_TYPE = "1";
            objCurrentPPIDChangeEvent.BODY.OLD_PPID = m_objModelParameter.strPPID;
            m_objDocument.m_objProcessCIM.m_objCIMSendMessage.SetCurrentPPIDChangeEvent(objCurrentPPIDChangeEvent);
        }
    }
}