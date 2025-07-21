using GameSystem.MVCTemplate;

namespace Assets.GameSystem.TemplateSystem.Main
{
    //ctrl管理的视图
    public enum TempSystemViews
    {
        TemplateSystemView,
    }
    public class TemplateSystemViewCtrl : BaseCtrl
    {
        public override string GetPrefabPath() => "TemplateSystemView";
        public override BaseModel GetModel() => Model ??= new TemplateSystemViewModel();
        public TemplateSystemViewCtrl() : base() { }
        public TemplateSystemViewCtrl(string systemName, params object[] args) : base(systemName, args)
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