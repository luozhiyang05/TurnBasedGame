using GameSystem.MVCTemplate;
using UIComponents;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tips
{
    public class HistoryTips : BaseTips
    {   

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
        public override bool ClickMaskPanel()
        {
            return true;
        }
        #endregion

        protected override void Init()
        {

        }

        public override void OnShow()
        {
   
        }
    }
}