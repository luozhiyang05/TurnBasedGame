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
        }

        public void ShowView(params object[] args)
        {
            ctrl ??= new TemplateSystemViewCtrl();
            (ctrl as TemplateSystemViewCtrl).OpenView(args);
        }
    }
}