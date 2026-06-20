using SVI_NFT_R.DEVICE.Nachi.SignalNames;
using System;

namespace SVI_NFT_R.DEVICE.Nachi
{
    public class RobotHandshakeHandler
    {
        public IReadOnlyBitOne[] RobotArrivalDownReq { get; private set; }
        public IBitOne[] ControlArrivalDownAck { get; private set; }
        public IReadOnlyBitOne[] RobotPostActionReq { get; private set; }
        public IBitOne[] ControlPostActionAck { get; private set; }
        private readonly SignalController mSignalController;

        public RobotHandshakeHandler(SignalController signalController, IReadOnlyBitOne[] robotArrivalDownReq, IBitOne[] controlArrivalDownAck, IReadOnlyBitOne[] robotPostActionReq, IBitOne[] controlPostActionAck)
        {
            mSignalController = signalController;
            RobotArrivalDownReq = robotArrivalDownReq;
            ControlArrivalDownAck = controlArrivalDownAck;
            RobotPostActionReq = robotPostActionReq;
            ControlPostActionAck = controlPostActionAck;
        }

        /// <summary>
        /// H/S 시작. 로봇 다운 위치로 이동.
        /// </summary>
        /// <returns></returns>
        public bool Begin_Down()
        {
            if (Array.TrueForAll(RobotArrivalDownReq, waitForSignalOn) == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// H/S 정리. 로봇 업 위치로 이동.
        /// </summary>
        /// <returns></returns>
        public bool End_Up()
        {
            Array.ForEach(ControlArrivalDownAck, setSignalOn);
            if (Array.TrueForAll(RobotArrivalDownReq, waitForSignalOff) == false)
            {
                return false;
            }
            Array.ForEach(ControlArrivalDownAck, setSignalOff);

            if (Array.TrueForAll(RobotPostActionReq, waitForSignalOn) == false)
            {
                return false;
            }
            Array.ForEach(ControlPostActionAck, setSignalOn);
            if (Array.TrueForAll(RobotPostActionReq, waitForSignalOff) == false)
            {
                return false;
            }
            Array.ForEach(ControlPostActionAck, setSignalOff);

            return true;
        }

        private void setSignalOn(IBitOne controller) => controller.Set();

        private void setSignalOff(IBitOne controller) => controller.Clear();

        private bool waitForSignalOn(IReadOnlyBitOne controller) => controller.WaitForTargetValue(CDefine.ON, mSignalController.RobotTimeout, checkCancel);

        private bool waitForSignalOff(IReadOnlyBitOne controller) => controller.WaitForTargetValue(CDefine.OFF, mSignalController.RobotTimeout, checkCancel);

        private bool checkCancel() => mSignalController.X.BB[Bit.EInput.EMERGENCY_STOP].Value;
    }
}
