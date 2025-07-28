using Assets.GameSystem.RedPointSystem.Main;
using Framework;

namespace Assets.GameSystem.RedPointSystem
{
    public interface IRedPointSystemModule: IModule
    {
        public void ShowView(params object[] args);
        public void OpenSon(params object[] args);
    }

    public class RedPointSystemModule : BaseModule, IRedPointSystemModule
    {

        protected override void OnInit()
        {
            ctrl = new RedPointSystemViewCtrl(nameof(RedPointSystemModule));
        }

        public void ShowView(params object[] args)
        {
            (ctrl as RedPointSystemViewCtrl).OpenView(args);
        }

        public void OpenSon(params object[] args)
        {
            (ctrl as RedPointSystemViewCtrl).OpenSubPanelView<RedPointSystemView>(ERedPointSystemViews.SonView.ToString(), args);
        }
    }
}