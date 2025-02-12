using GameSystem.MVCTemplate;
using Tool.UI;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem.MenuSystem.LevelChose.Main
{
    public class LevelChoseViewCtrl : BaseCtrl
    {
        public override string GetPrefabPath() => "LevelChoseView";
        public override BaseModel GetModel() => Model ??= new LevelChoseViewModel();
        public override BaseView GetView() => View;
        public LevelChoseViewCtrl() : base() { }
        public LevelChoseViewCtrl(params object[] args) : base(args)
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