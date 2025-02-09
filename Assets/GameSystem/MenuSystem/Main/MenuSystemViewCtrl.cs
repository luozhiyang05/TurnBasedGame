using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem.MenuSystem.Main
{
    public class MenuSystemViewCtrl : BaseCtrl
    {
        public override string GetPrefabPath() => "MenuSystemView";
        public override BaseModel GetModel() => Model ??= new MenuSystemViewModel();
        public override BaseView GetView() => View;
        public MenuSystemViewCtrl() : base() { }
        public MenuSystemViewCtrl(params object[] args) : base(args)
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
        public override void OnBeforeShow()
        {
            //一般做给View层传递数据
        }
        public override void OnShowComplate()
        {
            //一般做网络请求
        }
    }
}