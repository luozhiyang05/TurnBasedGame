using Assets.GameSystem.TemplateOneSystem.Main;
using Framework;

namespace Assets.GameSystem.TemplateOneSystem
{
    public interface ITemplateOneSystemModule: IModule
    {
        public void ShowView();
    }

    public class TemplateOneSystemModule : AbsModule, ITemplateOneSystemModule
    {
        private TemplateOneSystemViewCtrl _viewCtrl;

        protected override void OnInit()
        {
        }

        public void ShowView()
        {
            _viewCtrl ??= new TemplateOneSystemViewCtrl();
            _viewCtrl.ShowView();
        }
    }
}