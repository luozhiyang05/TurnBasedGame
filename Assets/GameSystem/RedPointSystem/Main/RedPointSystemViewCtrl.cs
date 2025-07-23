using GameSystem.MVCTemplate;
using Tool.UI;

namespace Assets.GameSystem.RedPointSystem.Main
{
    //ctrl管理的视图
    public enum ERedPointSystemViews
    {
        RedPointSystemView,
        SonView,
    }
    public class RedPointSystemViewCtrl : BaseCtrl
    {
        public override BaseModel GetModel() => Model ??= new RedPointSystemViewModel();
        public RedPointSystemViewCtrl(string moduleName, params object[] args) : base(moduleName, args)
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
            SetOpenViewName(ERedPointSystemViews.RedPointSystemView.ToString());
            ShowView(EuiLayer.GameUI, args);
        }
    }
}