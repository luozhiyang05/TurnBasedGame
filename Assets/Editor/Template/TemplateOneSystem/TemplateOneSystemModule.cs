using Framework;
using GameSystem.TemplateOneSystem.Main;

namespace GameSystem.TemplateOneSystem
{
    public interface ITemplateOneSystem : IModule
    {
        public void ShowView();
    }

    public class TemplateTemplateOneSystem : AbsModule, ITemplateOneSystem
    {
        private TemplateOneSystemCtrl _ctrl;

        protected override void OnInit()
        {
        }

        public void ShowView()
        {
            _ctrl ??= new TemplateOneSystemCtrl();
            _ctrl.OnShowView();
        }
    }
}