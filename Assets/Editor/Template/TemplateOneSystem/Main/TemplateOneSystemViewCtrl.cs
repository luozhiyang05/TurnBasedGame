using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;

namespace GameSystem.TemplateOneSystem.Main
{
    public class TemplateOneSystemViewCtrl : BaseCtrl
    {
        /// <summary>
        /// 初始化
        /// </summary>
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
            return new TemplateOneSystemViewModel();
        }

        protected override BaseView GetView()
        {
            return UIManager.GetInstance().LoadViewGo("TemplateOneSystemView", EuiLayer.Mid);
        }
    }
}