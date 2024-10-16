using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem.TemplateOneSystem.TemplateTwoSystem.Main
{
    public class TemplateTwoSystemViewCtrl : BaseCtrl
    {
        public override string GetPrefabPath() => "TemplateTwoSystemView";
        public override BaseModel GetModel() => Model ??= new TemplateTwoSystemViewModel();
        public override BaseView GetView() => View;
        public TemplateTwoSystemViewCtrl() : base() { }
        public TemplateTwoSystemViewCtrl(params object[] args) : base(args)
        {

        }
        protected override void Init(params object[] args)
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