using System;

namespace Mcc
{
    public sealed class MccLogProvider : IDisposable
    {
        private IMccLogItem[] mLogItems;
        private bool mbShouldWriteExistCell = false;

        internal MccLogProvider(IMccLogItem[] logItems, bool bShouldWriteExistCell)
        {
            mLogItems = logItems;
            mbShouldWriteExistCell = bShouldWriteExistCell;
            writeStart();
        }

        private void writeStart()
        {
            foreach (var item in mLogItems)
            {
                if (mbShouldWriteExistCell == true)
                {
                    item.WriteStartExist();
                }
                else
                {
                    item.WriteStart();
                }
            }
        }

        private void writeEnd()
        {
            foreach (var item in mLogItems)
            {
                if (mbShouldWriteExistCell == true)
                {
                    item.WriteEndExist();
                }
                else
                {
                    item.WriteEnd();
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                writeEnd();

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~MccLogProvider() {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

}
