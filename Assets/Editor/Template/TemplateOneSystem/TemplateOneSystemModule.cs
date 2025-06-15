using Assets.GameSystem.TemplateOneSystem.Main;
using Framework;

namespace Assets.GameSystem.TemplateOneSystem
{
    public interface ITemplateOneSystemModule: IModule
    {
        public void ShowView();
    }

    public class TemplateOneSystemModule : BaseModule, ITemplateOneSystemModule
    {

        protected override void OnInit()
        {
        }

        public void ShowView()
        {
            var viewName = "TemplateOneSystemView";
            var ctrl = GetCtrl(viewName);
            if (ctrl == null)
            {
                ctrl = new TemplateOneSystemViewCtrl();
                SetViewInfo(viewName, ctrl);
            }
            ctrl.ShowView();
        }
    }
}