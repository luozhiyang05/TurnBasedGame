using Assets.GameSystem.TemplateSystem.Main;
using Framework;

namespace Assets.GameSystem.TemplateSystem
{
    public interface ITemplateSystemModule: IModule
    {
        public void ShowView(params object[] args);
    }

    public class TemplateSystemModule : BaseModule, ITemplateSystemModule
    {

        protected override void OnInit()
        {
            ctrl = new TemplateSystemViewCtrl(nameof(TemplateSystemModule));
        }

        public void ShowView(params object[] args)
        {
            (ctrl as TemplateSystemViewCtrl).OpenView(args);
        }
    }
}