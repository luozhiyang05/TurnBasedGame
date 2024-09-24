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