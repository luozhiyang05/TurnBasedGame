using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;

namespace GameSystem.TemplateOneSystem.Main
{
    public class TemplateOneSystemCtrl : BaseCtrl
    {
        public TemplateOneSystemCtrl() : base(new TemplateOneSystemModel(), EuiLayer.Mid)
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


        public TemplateOneSystemModel GetModel() => Model as TemplateOneSystemModel;
        public TemplateOneSystemView GetView() => View as TemplateOneSystemView;
    }
}