using GameSystem.MVCTemplate;
using Tool.UI;

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

        public void OpenView(params object[] args)
        {
            var viewName = GetPrefabPath();
            SetOpenViewName(viewName);
            ShowView(EuiLayer.GameUI, args);
        }
    }
}