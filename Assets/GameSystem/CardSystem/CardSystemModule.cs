using System;
using System.Collections.Generic;
using Framework;
using GameSystem.BattleSystem;
using GameSystem.BattleSystem.Scripts;
using GameSystem.CardSystem.Main;
using GameSystem.CardSystem.Scripts;
using Tool.ResourceMgr;

namespace GameSystem.CardSystem
{
    public interface ICardSystemModule : IModule
    {
        void ShowView();
        void UnitUseCard(BaseCardSo card, AbsUnit self, AbsUnit target);
        T LoadPlayerCard<T>(string cardName) where T : BaseCardSo;
    }

    public class CardSystemModule : AbsModule, ICardSystemModule
    {
        private CardSystemViewCtrl _viewCtrl;

        private Dictionary<string, BaseCardSo> _cardDic;

        protected override void OnInit()
        {
            _cardDic = new Dictionary<string, BaseCardSo>();
        }

        public void ShowView()
        {
            _viewCtrl ??= new CardSystemViewCtrl();
            _viewCtrl.OnShowView();
        }

        public void UnitUseCard(BaseCardSo card, AbsUnit self, AbsUnit target)
        {
            card.UseCard(self, target);
        }

        public T LoadPlayerCard<T>(string cardName) where T : BaseCardSo
        {
            if (_cardDic.TryGetValue(cardName, out var card))
            {
                return card as T;
            }

            var loadCard = ResMgr.GetInstance().SyncLoad<T>("PlayerCards/" + cardName);
            if (_cardDic.TryAdd(cardName, loadCard))
            {
                return loadCard;
            }

            throw new Exception($"读取卡牌错误:{cardName}");
        }
    }
}