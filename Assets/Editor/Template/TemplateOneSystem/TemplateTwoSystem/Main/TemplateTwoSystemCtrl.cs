using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;

namespace GameSystem.TemplateOneSystem.TemplateTwoSystem.Main
{
    public class TemplateTwoSystemCtrl : BaseCtrl
    {
        public TemplateTwoSystemCtrl() : base(new TemplateTwoSystemModel(), EuiLayer.Mid)
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

        public TemplateTwoSystemModel GetModel() => Model as TemplateTwoSystemModel;
        public TemplateTwoSystemView GetView() => View as TemplateTwoSystemView;
    }
}