using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem.TemplateOneSystem.Main
{
    public class TemplateOneSystemViewCtrl : BaseCtrl
    {
        public override string GetPrefabPath() => "TemplateOneSystemView";
        public override BaseModel GetModel() => Model ??= new TemplateOneSystemViewModel();
        public override BaseView GetView() => View;
        protected override void Init()
        {
        }
        protected override void InitListener()
        {
        }
        protected override void RemoveListener()
        {
        }
        public override void OnShowComplate()
        {
            //一般做网络请求或给view层传递数据
        }
    }
}