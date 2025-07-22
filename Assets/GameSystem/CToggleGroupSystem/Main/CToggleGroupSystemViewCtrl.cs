using GameSystem.MVCTemplate;
using Tool.UI;

namespace Assets.GameSystem.CToggleGroupSystem.Main
{
    //ctrl管理的视图
    public enum ECToggleGroupSystemViews
    {
        CToggleGroupSystemView,
        ToggleOne,
        ToggleTwo,
        ToggleThree,
    }
    public class CToggleGroupSystemViewCtrl : BaseCtrl
    {
        public override BaseModel GetModel() => Model ??= new CToggleGroupSystemViewModel();
        public CToggleGroupSystemViewCtrl(string moduleName, params object[] args) : base(moduleName, args)
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
            SetOpenViewName(ECToggleGroupSystemViews.CToggleGroupSystemView.ToString());
            ShowView(EuiLayer.GameUI, args);
        }
    }
}