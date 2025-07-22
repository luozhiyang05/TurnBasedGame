using Assets.GameSystem.CToggleGroupSystem.Main;
using Framework;

namespace Assets.GameSystem.CToggleGroupSystem
{
    public interface ICToggleGroupSystemModule : IModule
    {
        public void ShowView(params object[] args);
        public void ToggleOne(params object[] args);
        public CToggleGroupSystemViewCtrl GetCtrl();
    }

    public class CToggleGroupSystemModule : BaseModule, ICToggleGroupSystemModule
    {

        protected override void OnInit()
        {
            ctrl = new CToggleGroupSystemViewCtrl(nameof(CToggleGroupSystemModule));
        }

        public void ShowView(params object[] args)
        {
            (ctrl as CToggleGroupSystemViewCtrl).OpenView(args);
        }

        public CToggleGroupSystemViewCtrl GetCtrl()
        {
            return ctrl as CToggleGroupSystemViewCtrl;
        }

        public void ToggleOne(params object[] args)
        {
            (ctrl as CToggleGroupSystemViewCtrl).OpenSubPanelView<CToggleGroupSystemView>(ECToggleGroupSystemViews.ToggleOne.ToString(), args);
        }
    }
}