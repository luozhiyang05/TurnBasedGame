using Framework;
using GameSystem.BattleSystem.Scripts;
using GameSystem.CardSystem.Main;
using GameSystem.CardSystem.Scripts;

namespace GameSystem.CardSystem
{
    public interface ICardSystemModule : IModule
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

        public void PlayerAtk(AtkCardSo card, AbsUnit self, AbsUnit target)
        {
            card.OnAttackToCard(self, target);
        }
        
    }
}