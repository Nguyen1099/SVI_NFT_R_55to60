using HLDevice;
using System;
using System.Threading;

namespace SVI_NFT_R
{
    public sealed class SubBatchMotorRepeatMove
    {
        public bool IsRunning { get; private set; } = false;
        public CProcessMotion.EMotor MotorIndex { get; private set; }
        public int[] PositionIndexes { get; private set; }
        public int CurrentIndex { get; private set; } = 0;
        public int RepeatCount { get; private set; } = 0;
        private bool mbCancel = false;
        private readonly CProcessInterlock mInterlock;
        private readonly CDocument mDocument;
        private string mResourceTitleRepeatMove;
        private string mResourceTitleRepeatProgress;

        public SubBatchMotorRepeatMove(CDocument doc, CProcessInterlock interlock)
        {
            mDocument = doc;
            mInterlock = interlock;
        }

        public bool Execute(CProcessMotion.EMotor motorIndex, int repeatCount, int[] targetIndexes)
        {
            if (IsRunning == true)
            {
                return false;
            }
            mResourceTitleRepeatMove = mDocument.GetDatabaseUIText(nameof(mResourceTitleRepeatMove), nameof(SubBatchMotorRepeatMove));
            mResourceTitleRepeatProgress = mDocument.GetDatabaseUIText(nameof(mResourceTitleRepeatProgress), nameof(SubBatchMotorRepeatMove));
            IsRunning = true;
            mbCancel = false;
            MotorIndex = motorIndex;
            RepeatCount = repeatCount;
            PositionIndexes = targetIndexes;
            CDeviceMotor motor = mDocument.m_objProcessMain.m_objProcessMotion.m_objMotor[MotorIndex];

            try
            {
                var dialog = mDocument.GetMainFrame().ShowWaitMessage(true, $"");
                dialog.CanCancel = true;
                dialog.CancelClick += Dialog_CancelClick;
                for (int index = 0; index < RepeatCount; index++)
                {
                    CurrentIndex = index;

                    for (int i = 0; i < PositionIndexes.Length; i++)
                    {
                        string displayPositionName = $"MOVE => [{motor.HLGetMotorPosition().strPositionName[PositionIndexes[i]]}]";
                        mDocument.GetMainFrame().ShowWaitMessage(true, $"[{MotorIndex}] {mResourceTitleRepeatMove}{Environment.NewLine}  {displayPositionName}{Environment.NewLine}  {mResourceTitleRepeatProgress}: {CurrentIndex + 1} / {RepeatCount}");

                        // 처음에 모터 컨디션을 확인 할 때 에러 메시지가 대기 다이얼로그에 가릴 수 있어서 적용함
                        mDocument.GetMainFrame().ShowWaitMessage(index > 0);

                        if (mbCancel == true)
                        {
                            return false;
                        }
                        // 위치 이동
                        if (Move(PositionIndexes[i]) == false)
                        {
                            return false;
                        }
                        // 이동 후 안정화 딜레이
                        Thread.Sleep(motor.HLGetMotorOperationParameter().iDelayAfterMoving);
                    }
                }

                return true;
            }
            finally
            {
                var dialog = mDocument.GetMainFrame().ShowWaitMessage(false);
                dialog.CancelClick -= Dialog_CancelClick;
                dialog.CanCancel = false;
                IsRunning = false;
            }
        }

        private void Dialog_CancelClick(object sender, EventArgs e)
        {
            mbCancel = true;
        }

        private bool Move(int positionIndex)
        {
            CDeviceMotor motor = mDocument.m_objProcessMain.m_objProcessMotion.m_objMotor[MotorIndex];

            // 인터락 확인
            if (mInterlock.CheckMotionClassInterlock(MotorIndex.ToString(), motor.HLGetMotorPosition().dPosition[positionIndex]) == false)
            {
                return false;
            }
            mDocument.GetMainFrame().ShowWaitMessage(true);

            // 이동 시작 (자동 시퀀스 속도로 구동함)
            if (motor.HLMoveAbsoluteIndex(positionIndex, HLDevice.Abstract.CDeviceMotorAbstract.EVelocityMode.VELOCITY_MODE_AUTO_RUN) == false)
            {
                return false;
            }

            // 이동 완료 대기
            if (motor.HLWaitMotorStatus(motor, HLDevice.Abstract.CDeviceMotorAbstract.EPositionCompare.POSITION_COMPARE_EQUAL, motor.HLGetMotorOperationParameter().iStandardLimitTimeOut) == false)
            {
                return false;
            }

            return true;
        }
    }
}
