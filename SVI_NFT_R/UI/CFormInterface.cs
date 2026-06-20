using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SVI_NFT_R
{
    /// <summary>
    /// 폼에서 무조건 구현해줘야 하는 부분
    /// </summary>
    public interface CFormInterface
    {
        /// <summary>
        /// 모든 폼에서 언어 변경 함수를 묶어서 관리
        /// </summary>
        /// <returns></returns>
        bool SetChangeLanguage();

        /// <summary>
        /// 모든 폼에서 타이머 끄고 키고를 제어
        /// </summary>
        /// <param name="bTimer"></param>
        void SetTimer(bool bTimer);

        /// <summary>
        /// 모든 폼에서 Visible 상태를 제어
        /// </summary>
        /// <param name="bVisible"></param>
        void SetVisible(bool bVisible);
    }

    /// <summary>
    /// 폼에서 사용되는 확장 메서드를 정의한 클래스
    /// </summary>
    public static class CFormExtention
    {
        /// <summary>
        /// 컨트롤 컬랙션 내부에 타입이 일치하는 자식 컨트롤의 리스트를 반환하는 함수
        /// </summary>
        /// <param name="ctrls">컨트롤 컬랙션</param>
        /// <param name="type">검색 할 타입</param>
        /// <returns>타입이 일치하는 컨트롤 리스트</returns>
        public static List<Control> GetChildControlListByType(this Control.ControlCollection ctrls, Type type)
        {
            var result = new List<Control>();
            var controlQueue = new Queue<Control.ControlCollection>();
            controlQueue.Enqueue(ctrls);

            while (controlQueue.Count > 0)
            {
                Control.ControlCollection controls = controlQueue.Dequeue();
                if (
                    null == controls
                    || 0 == controls.Count
                    )
                {
                    continue;
                }
                foreach (Control control in controls)
                {
                    if (type == control.GetType())
                    {
                        result.Add(control);
                    }
                    controlQueue.Enqueue(control.Controls);
                }
            }

            return result;
        }
    }
}