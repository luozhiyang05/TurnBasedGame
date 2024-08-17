using Framework;
using GameSystem.TemplateOneSystem.Main;

namespace GameSystem.TemplateOneSystem
{
    public interface ITemplateOneSystem : IModule
    {
        public void ShowView();
    }

    public class TemplateOneSystemModule : AbsModule, ITemplateOneSystem
    {
        private TemplateOneSystemViewCtrl _viewCtrl;

        protected override void OnInit()
        {
        }

        public void ShowView()
        {
            _viewCtrl ??= new TemplateOneSystemViewCtrl();
            _viewCtrl.OnShowView();
        }
    }
}