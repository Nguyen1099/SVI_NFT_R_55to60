using ENC.IO;
using ENC.IO.Device.Melsec;
using System.Collections.Generic;
using System.Linq;

namespace SVI_NFT_R
{
    public class IoHardware
    {
        public bool IsInitialized { get; private set; } = false;
        public IoHardwareMapping CcLinkIeControlNetworkDevB { get; } = new IoHardwareMapping();
        public IoHardwareMapping CcLinkIeControlNetworkDevW { get; } = new IoHardwareMapping();
        public IoHardwareMapping VirtualCcLinkIeControlNetworkDevB { get; } = new IoHardwareMapping();
        public IoHardwareMapping VirtualCcLinkIeControlNetworkDevW { get; } = new IoHardwareMapping();
        public readonly List<IoHardwareMapping> mIoHardwareMappings = new List<IoHardwareMapping>();

        public bool Initialize(CDocument document)
        {
            if (IsInitialized == true)
            {
                return false;
            }

            // IO하드웨어 맵핑 초기화
            {
                bool bIsVirtualMode = document.m_objConfig.GetSystemParameter().eSimulationMode == CDefine.ESimulationMode.SIMULATION_MODE_ON;
                // CC Link IE Control Network
                {
                    // http://kr.cc-link.org/ko/board.do?act=cc_link_ie07
                    short.TryParse(document.m_objConfig.GetCCLinkIEParameter().strInterfaceChannel, out short channelNumber);
                    CcLinkIeControlNetworkDevB.SetDeviceName("CCLinkIeControlNetwork.DevB")
                        .AddDeviceCCLinkIeControlNetworkDevB((ECCLinkIeControlNetwork)channelNumber)
                        .UsingMemoryMapVirtualMachine(bIsVirtualMode || document.m_objConfig.GetCCLinkIEParameter().bRunSimulationMode == true, "CCLinkIeControlNetwork.DevB")
                        .Initialize();
                    CcLinkIeControlNetworkDevW.SetDeviceName("CCLinkIeControlNetwork.DevW")
                        .AddDeviceCCLinkIeControlNetworkDevW((ECCLinkIeControlNetwork)channelNumber)
                        .UsingMemoryMapVirtualMachine(bIsVirtualMode || document.m_objConfig.GetCCLinkIEParameter().bRunSimulationMode == true, "CCLinkIeControlNetwork.DevW")
                        .Initialize();
                }
                // Virtual CC Link IE Control Network (설비 간 인터페이스 언링크 드라이런 용)
                {
                    // http://kr.cc-link.org/ko/board.do?act=cc_link_ie07
                    short.TryParse(document.m_objConfig.GetCCLinkIEParameter().strInterfaceChannel, out short channelNumber);
                    VirtualCcLinkIeControlNetworkDevB.SetDeviceName("Virtual.CCLinkIeControlNetwork.DevB")
                        .AddDeviceCCLinkIeControlNetworkDevB((ECCLinkIeControlNetwork)channelNumber)
                        .UsingMemoryMapVirtualMachine(true, "Virtual.CCLinkIeControlNetwork.DevB")
                        .Initialize();
                    VirtualCcLinkIeControlNetworkDevW.SetDeviceName("Virtual.CCLinkIeControlNetwork.DevW")
                        .AddDeviceCCLinkIeControlNetworkDevW((ECCLinkIeControlNetwork)channelNumber)
                        .UsingMemoryMapVirtualMachine(true, "Virtual.CCLinkIeControlNetwork.DevW")
                        .Initialize();
                }
            }

            // 모든 IO하드웨어 맵핑을 자동으로 리스트에 등록함
            mIoHardwareMappings.Clear();
            var properties = GetType().GetProperties();
            foreach (var property in properties)
            {
                if (Equals(property.PropertyType, typeof(IoHardwareMapping)) == false)
                {
                    continue;
                }
                if (property.GetValue(this) is IoHardwareMapping ioHardwareMapping)
                {
                    mIoHardwareMappings.Add(ioHardwareMapping);
                }
            }
            IsInitialized = true;
            return true;
        }

        public void DeInitialize()
        {
            if (IsInitialized == false)
            {
                return;
            }

            // IO하드웨어 맵핑 정리
            foreach (IoHardwareMapping item in GetEnumerator().Reverse())
            {
                item.DeInitialize();
            }

            IsInitialized = false;
        }

        public IEnumerable<IoHardwareMapping> GetEnumerator()
        {
            return mIoHardwareMappings;
        }

        public IEnumerable<string> GetNames()
        {
            return GetEnumerator().Select(i => i.Name);
        }

        public IoHardwareMapping GetIoHardwareMapping(string name)
        {
            return mIoHardwareMappings.FirstOrDefault(i => i.Name == name);
        }
    }
}
