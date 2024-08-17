using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;

namespace GameSystem.TemplateOneSystem.TemplateTwoSystem.Main
{
    public class TemplateTwoSystemViewCtrl : BaseCtrl
    {
        public TemplateTwoSystemViewCtrl() : base(new TemplateTwoSystemViewModel(), EuiLayer.Mid)
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
            Debug.Log("加载viewGo完成");
        }

        public TemplateTwoSystemViewModel GetModel() => Model as TemplateTwoSystemViewModel;
        public TemplateTwoSystemView GetView() => View as TemplateTwoSystemView;
    }
}