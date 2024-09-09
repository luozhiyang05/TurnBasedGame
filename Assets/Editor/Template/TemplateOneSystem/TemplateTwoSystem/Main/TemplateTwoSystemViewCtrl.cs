using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;
using UnityEngine.Events;

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
        
        public override BaseModel GetModel()
        {
            return Model ??= new TemplateTwoSystemViewModel();
        }

        public override BaseView GetView()
        {
           return UIManager.GetInstance().LoadUIPrefab("TemplateTwoSystemView", EuiLayer.GameUI);
        }

        public override void OnCompleteLoad()
        {
            //一般做网络请求或给view层传递数据
        }
    }
}