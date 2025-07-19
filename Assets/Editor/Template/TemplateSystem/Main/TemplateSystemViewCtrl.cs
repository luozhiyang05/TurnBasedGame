using GameSystem.MVCTemplate;

namespace Assets.GameSystem.TemplateSystem.Main
{
    public class TemplateSystemViewCtrl : BaseCtrl
    {
        public override string GetPrefabPath() => "TemplateSystemView";
        public override BaseModel GetModel() => Model ??= new TemplateSystemViewModel();
        public TemplateSystemViewCtrl() : base() { }
        public TemplateSystemViewCtrl(params object[] args) : base(args)
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

        public void OpenView()
        {
            var viewName = GetPrefabPath();
            SetOpenViewName(viewName);
            ShowView();
        }
    }
}