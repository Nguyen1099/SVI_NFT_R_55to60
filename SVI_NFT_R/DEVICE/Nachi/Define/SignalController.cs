using System;
using System.Collections.Generic;
using System.Text;

namespace SVI_NFT_R.DEVICE.Nachi
{
    /// <![CDATA[
    /// 로봇에서 공용으로 사용할 수 있는 신호들을 편하게 사용 할 수 있도록 하는 클래스 입니다.
    /// 
    /// [SIGNAL 명명 규칙]
    /// - 하위 호환성을 위해 되도록이면 기존 신호 이름을 변경하지 않도록한다
    /// - 로봇이 여러대 붙을 경우 Prefix를 로봇 별로 Unique 하게 정의하여 신호 이름 충돌을 피한다 (신호 이름 예> $"{Prefix}_{신호명}")
    /// - 비트 신호를 그룹으로 묶어서 사용할 경우 앞쪽 이름은 똑같이 하고 뒤에 '_{숫자}'를 순서대로(1, 2, 3, ....) 붙여서 표시한다 (신호 이름 예> $"{Prefix}_{신호명}_{index}")
    /// - 워드 영역 그룹 기능은 같은 영역내에 연속된 영역에서만 사용 가능하다
    /// 
    /// [클래스 구조]
    /// SignalController 
    ///  ㄴSignalController.InputHandler - 모든 입력 신호를 묶음
    ///  ㄴSignalController.OutputHandler - 모든 출력 신호를 묶음
    ///      ㄴSignalController.BitAreaBit - 실제로 Bit 값을 변경함
    ///      ㄴSignalController.BitAreaBitGroup - 실제로 Word 값을 변경함
    ///      ㄴSignalController.WordAreaBit - 실제로 Bit 값을 변경함
    ///      ㄴSignalController.WordAreaBitGroup - 실제로 Word 값을 변경함
    /// SignalNames - 공용 신호 이름 및 그룹 구성을 정의함
    /// 
    /// interface 들은 실제 구현부를 외부에서 인스턴스를 선언해서 사용하지 못하도록 막기위해서 사용하였음
    /// ]]>

    public partial class SignalController : IHaveLogEvent
    {
        /// <summary>
        /// 입력 신호
        /// </summary>
        public IInputHandler X { get; private set; }
        /// <summary>
        /// 출력 신호
        /// </summary>
        public IOutputHandler Y { get; private set; }
        public TimeSpan RobotTimeout => Config.WaitTime.CommTimeout.RobotTimeout.ToTimeSpan();
        public event EventHandler<string> OnEventOccured;
        public string SignalNamePrefix { get; private set; }
        private static readonly Dictionary<int, object> mWordWritingLock = new Dictionary<int, object>();
        private readonly CDocument mDocument;
        private readonly bool mbIsVirtual;

        public SignalController(CDocument document, string signalNamePrefix, bool bIsVirtual)
        {
            mbIsVirtual = bIsVirtual;
            mDocument = document;
            SignalNamePrefix = signalNamePrefix;
            var analogParameters = document.m_objProcessMain.m_objCCLinkVer2.GetInitializeParameter.objInterfaceParameterAnalog;
            foreach (var parameter in analogParameters.Values)
            {
                if (false == mWordWritingLock.ContainsKey(parameter.iDataAddress))
                {
                    mWordWritingLock.Add(parameter.iDataAddress, new object());
                }
            }

            X = new InputHandler(document, signalNamePrefix, mbIsVirtual);
            Y = new OutputHandler(document, signalNamePrefix, mbIsVirtual);

            // 개발자 실수 체크
            checkSignalNameValidation();

            foreach (IHaveLogEvent item in X.BB.Values)
            {
                item.OnEventOccured += onEventOccured;
            }
            foreach (IHaveLogEvent item in X.BG.Values)
            {
                item.OnEventOccured += onEventOccured;
            }
            foreach (IHaveLogEvent item in X.WB.Values)
            {
                item.OnEventOccured += onEventOccured;
            }
            foreach (IHaveLogEvent item in X.WG.Values)
            {
                item.OnEventOccured += onEventOccured;
            }
            foreach (IHaveLogEvent item in Y.BB.Values)
            {
                item.OnEventOccured += onEventOccured;
            }
            foreach (IHaveLogEvent item in Y.BG.Values)
            {
                item.OnEventOccured += onEventOccured;
            }
            foreach (IHaveLogEvent item in Y.WB.Values)
            {
                item.OnEventOccured += onEventOccured;
            }
            foreach (IHaveLogEvent item in Y.WG.Values)
            {
                item.OnEventOccured += onEventOccured;
            }
        }

        public IBitOne CreateBitAreaBitOne(string signalName)
        {
            var handler = new BitAreaBitOne(mDocument, signalName, mbIsVirtual);
            handler.OnEventOccured += onEventOccured;
            return new BitAreaBitOne(mDocument, signalName, mbIsVirtual);
        }

        public IBitGroup CreateBitAreaBitGroup(string[] signalNames)
        {
            var handler = new BitAreaBitGroup(mDocument, signalNames, mbIsVirtual);
            handler.OnEventOccured += onEventOccured;
            return handler;
        }

        public IBitOne CreateWordAreaBitOne(string signalName)
        {
            var handler = new WordAreaBitOne(mDocument, signalName, mbIsVirtual);
            handler.OnEventOccured += onEventOccured;
            return handler;
        }

        public IBitGroup CreateWordAreaBitGroup(string[] signalNames)
        {
            var handler = new WordAreaBitGroup(mDocument, signalNames, mbIsVirtual);
            handler.OnEventOccured += onEventOccured;
            return handler;
        }

        public void DisposeSignalHandler(IHaveLogEvent handler)
        {
            if (handler == null)
            {
                return;
            }
            handler.OnEventOccured -= onEventOccured;
        }

        private void checkSignalNameValidation()
        {
            StringBuilder sbValidationLog = new StringBuilder();
            var interfaceParameter = mDocument.m_objProcessMain.m_objCCLinkVer2.GetInitializeParameter;
            bool bError = false;
            bool bFind = false;
            sbValidationLog.AppendLine($"Robot Signal Validation Failed.");
            foreach (var item in X.BB.Values)
            {
                if (interfaceParameter.objInterfaceParameterDigital.ContainsKey(item.SignalName) == false)
                {
                    if (bFind == false)
                    {
                        sbValidationLog.AppendLine($"X Bit Area Bit [");
                        bFind = true;
                    }
                    sbValidationLog.AppendLine($"\t{item.SignalName}");
                    bError = true;
                }
            }
            if (bFind == true)
            {
                sbValidationLog.AppendLine($"]");
            }
            bFind = false;

            foreach (var item in X.WB.Values)
            {
                if (interfaceParameter.objInterfaceParameterAnalog.ContainsKey(item.SignalName) == false)
                {
                    if (bFind == false)
                    {
                        sbValidationLog.AppendLine($"X Word Area Bit [");
                        bFind = true;
                    }
                    sbValidationLog.AppendLine($"\t{item.SignalName}");
                    bError = true;
                }
            }
            if (bFind == true)
            {
                sbValidationLog.AppendLine($"]");
            }
            bFind = false;

            foreach (var item in Y.BB.Values)
            {
                if (interfaceParameter.objInterfaceParameterDigital.ContainsKey(item.SignalName) == false)
                {
                    if (bFind == false)
                    {
                        sbValidationLog.AppendLine($"Y Bit Area Bit [");
                        bFind = true;
                    }
                    sbValidationLog.AppendLine($"\t{item.SignalName}");
                    bError = true;
                }
            }
            if (bFind == true)
            {
                sbValidationLog.AppendLine($"]");
            }
            bFind = false;

            foreach (var item in Y.WB.Values)
            {
                if (interfaceParameter.objInterfaceParameterAnalog.ContainsKey(item.SignalName) == false)
                {
                    if (bFind == false)
                    {
                        sbValidationLog.AppendLine($"Y Word Area Bit [");
                        bFind = true;
                    }
                    sbValidationLog.AppendLine($"\t{item.SignalName}");
                    bError = true;
                }
            }
            if (bFind == true)
            {
                sbValidationLog.AppendLine($"]");
            }
            bFind = false;

            foreach (var item in X.BG.Values)
            {
                foreach (var signalName in item.SignalNames)
                {
                    if (interfaceParameter.objInterfaceParameterDigital.ContainsKey(signalName) == false)
                    {
                        if (bFind == false)
                        {
                            sbValidationLog.AppendLine($"X Bit Area Group [");
                            bFind = true;
                        }
                        sbValidationLog.AppendLine($"\t{signalName}");
                        bError = true;
                    }
                }
            }
            if (bFind == true)
            {
                sbValidationLog.AppendLine($"]");
            }
            bFind = false;

            foreach (var item in X.WG.Values)
            {
                foreach (var signalName in item.SignalNames)
                {
                    if (interfaceParameter.objInterfaceParameterAnalog.ContainsKey(signalName) == false)
                    {
                        if (bFind == false)
                        {
                            sbValidationLog.AppendLine($"X Word Area Group [");
                            bFind = true;
                        }
                        sbValidationLog.AppendLine($"\t{signalName}");
                        bError = true;
                    }
                }
            }
            if (bFind == true)
            {
                sbValidationLog.AppendLine($"]");
            }
            bFind = false;

            foreach (var item in Y.BG.Values)
            {
                foreach (var signalName in item.SignalNames)
                {
                    if (interfaceParameter.objInterfaceParameterDigital.ContainsKey(signalName) == false)
                    {
                        if (bFind == false)
                        {
                            sbValidationLog.AppendLine($"Y Bit Area Group [");
                            bFind = true;
                        }
                        sbValidationLog.AppendLine($"\t{signalName}");
                        bError = true;
                    }
                }
            }
            if (bFind == true)
            {
                sbValidationLog.AppendLine($"]");
            }
            bFind = false;

            foreach (var item in Y.WG.Values)
            {
                foreach (var signalName in item.SignalNames)
                {
                    if (interfaceParameter.objInterfaceParameterAnalog.ContainsKey(signalName) == false)
                    {
                        if (bFind == false)
                        {
                            sbValidationLog.AppendLine($"Y Word Area Group [");
                            bFind = true;
                        }
                        sbValidationLog.AppendLine($"\t{signalName}");
                        bError = true;
                    }
                }
            }
            if (bFind == true)
            {
                sbValidationLog.AppendLine($"]");
            }
            bFind = false;

            if (bError == true)
            {
                throw new Exception(sbValidationLog.ToString());
            }
        }

        public static object GetWordWriteLockObject(int addressIndex) => mWordWritingLock[addressIndex];

        private void onEventOccured(object sender, string logMessage)
        {
            OnEventOccured?.Invoke(sender, logMessage);
        }
    }

}
