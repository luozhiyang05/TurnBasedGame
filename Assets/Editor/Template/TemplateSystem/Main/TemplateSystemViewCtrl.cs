using GameSystem.MVCTemplate;
using Tool.UI;

namespace Assets.GameSystem.TemplateSystem.Main
{
    //ctrl管理的视图
    public enum ETemplateSystemViews
    {
        TemplateSystemView,
    }
    public class TemplateSystemViewCtrl : BaseCtrl
    {
        public override BaseModel GetModel() => Model ??= new TemplateSystemViewModel();
        public TemplateSystemViewCtrl(string moduleName, params object[] args) : base(moduleName, args)
        {

        }
        protected override void Init(params object[] args)
        {

        }
        protected override void InitListener()
        {
        }

        public void OpenView(params object[] args)
        {
            SetOpenViewName(ETemplateSystemViews.TemplateSystemView.ToString());
            ShowView(EuiLayer.GameUI, args);
        }
    }
}