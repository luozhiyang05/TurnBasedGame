using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;

namespace GameSystem.TemplateOneSystem.TemplateTwoSystem.Main
{
    public class TemplateTwoSystemViewCtrl : BaseCtrl
    {
        protected override void Init()
        {
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        protected override void InitListener()
        {
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        protected override void RemoveListener()
        {
        }
        
        /// <summary>
        /// 展示主要view
        /// </summary>
        public void OnShowView() => OnOpen();
        
        protected override BaseModel GetModel()
        {
            return new TemplateTwoSystemViewModel();
        }

        protected override BaseView GetView()
        {
            return UIManager.GetInstance().LoadViewGo("TemplateTwoSystemView",EuiLayer.Mid);
        }
    }
}