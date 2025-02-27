using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.GameSystem.MenuSystem.Setting.Main
{
    public class SettingViewCtrl : BaseCtrl
    {
        public override string GetPrefabPath() => "SettingView";
        public override BaseModel GetModel() => Model ??= new SettingViewModel();
        public override BaseView GetView() => View;
        public SettingViewCtrl() : base() { }
        public SettingViewCtrl(params object[] args) : base(args)
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
        public override void OnBeforeShow(params object[] args)
        {
            //一般做给View层传递数据
        }
        public override void OnShowComplate(params object[] args)
        {
            //一般做网络请求
        }
    }
}