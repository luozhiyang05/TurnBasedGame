using GameSystem.MVCTemplate;
using GlobalData;
using UIComponents;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tips
{
    public class CardsCheckTips : BaseTips
    {
        public Text Txt_tip;
        public CButton Btn_certain;
        #region 遮罩相关
        /// <summary>
        /// 是否启用MaskPanel，启用的话只需要取消注释
        /// </summary>
        /// <returns></returns>
        public override bool MaskPanel()
        {
            return true;
        }

        /// <summary>
        /// 是否开启点击遮罩关闭View，启用的话只需要取消注释
        /// </summary>
        /// <returns></returns>
        // public override bool ClickMaskPanel()
        // {
        //     return true;
        // }
        #endregion

        protected override void Init()
        {
            
        }

        protected override void OnOpen(params object[] args)
        {
        }

        public override void OnRelease()
        {

        }
    }
}