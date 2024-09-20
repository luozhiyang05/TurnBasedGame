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

        /// <summary>
        /// 回合结束丢弃手牌
        /// </summary>
        void UpdateHeadCardInEr();
        
        /// <summary>
        /// 回合开始更新手牌
        /// </summary>
        void UpdateHeadCardInSr();
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
            _viewCtrl.ShowView();
        }

        public void UnitUseCard(BaseCardSo card, AbsUnit self, AbsUnit target)
        {
            //使用卡牌逻辑
            card.UseCard(self, target);

            //将使用的卡牌丢入弃牌队列
            (_viewCtrl.GetModel() as CardSystemViewModel)?.DiscardCards(card);
        }

        public void UpdateHeadCardInEr()
        {
            (_viewCtrl.GetModel() as CardSystemViewModel)?.UpdateHeadCardInEr();
        }

        public void UpdateHeadCardInSr()
        {
            (_viewCtrl.GetModel() as CardSystemViewModel)?.UpdateHeadCardInSr();
        }
    }
}