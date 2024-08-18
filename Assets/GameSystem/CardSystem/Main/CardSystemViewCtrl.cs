using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;

namespace GameSystem.CardSystem.Main
{
    public class CardSystemViewCtrl : BaseCtrl
    {
        public CardSystemViewCtrl() : base(new CardSystemViewModel(), EuiLayer.Mid)
        {
        }

        /// <summary>
        /// 处理部分view的业务
        /// </summary>
        protected override void InitListener()
        {
        }

        /// <summary>
        /// 展示主要view
        /// </summary>
        public void OnShowView() => OnOpen();

        /// <summary>
        /// view展示完毕回调函数
        /// </summary>
        protected override void OnCompleteLoad()
        {
            Debug.Log("加载view完成");
        }


        public CardSystemViewModel GetModel() => Model as CardSystemViewModel;
        public CardSystemView GetView() => View as CardSystemView;
    }
}