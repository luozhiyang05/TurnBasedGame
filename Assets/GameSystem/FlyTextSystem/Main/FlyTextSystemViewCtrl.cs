using GameSystem.MVCTemplate;

namespace Assets.GameSystem.FlyTextSystem.Main
{
    public class FlyTextSystemViewCtrl : BaseCtrl
    {
        public override string GetPrefabPath() => "FlyTextSystemView";
        public override BaseModel GetModel() => Model ??= new FlyTextSystemViewModel();
        public override BaseView GetView() => View;
        public FlyTextSystemViewCtrl() : base() { }
        public FlyTextSystemViewCtrl(params object[] args) : base(args)
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