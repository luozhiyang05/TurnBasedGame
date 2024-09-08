using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;
using UnityEngine.Events;

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
        
        public override BaseModel GetModel()
        {
            return Model ??= new TemplateOneSystemViewModel();
        }

        public override BaseView GetView()
        {
           return UIManager.GetInstance().LoadUIPrefab("TemplateOneSystemView", EuiLayer.GameUI);
        }

        public override void OnCompleteLoad()
        {
            //一般做网络请求或给view层传递数据
        }
    }
}