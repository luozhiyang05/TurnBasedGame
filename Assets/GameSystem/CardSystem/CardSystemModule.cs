using Framework;
using GameSystem.CardSystem.Main;

namespace GameSystem.CardSystem
{
    public interface ICardSystemModule: IModule
    {
        public void ShowView();
    }

    public class CardSystemModule : AbsModule, ICardSystemModule
    {
        private CardSystemViewCtrl _viewCtrl;

        protected override void OnInit()
        {
        }

        public void ShowView()
        {
            _viewCtrl ??= new CardSystemViewCtrl();
            _viewCtrl.OnShowView();
        }
    }
}