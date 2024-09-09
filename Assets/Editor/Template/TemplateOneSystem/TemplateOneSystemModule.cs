using Framework;
using GameSystem.TemplateOneSystem.Main;

namespace GameSystem.TemplateOneSystem
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