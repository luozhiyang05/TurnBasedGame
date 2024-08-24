using System;
using System.Collections.Generic;
using Framework;
using GameSystem.BattleSystem;
using GameSystem.BattleSystem.Scripts;
using GameSystem.CardSystem.Main;
using GameSystem.CardSystem.Scripts;
using Tool.ResourceMgr;
using Tool.Utilities.DataStruct;

namespace GameSystem.CardSystem
{
    public interface ICardSystemModule : IModule
    {
        void ShowView();
        /// <summary>
        /// 加载玩家使用的卡组
        /// </summary>
        void LoadUseCards();

        /// <summary>
        /// 获取卡牌
        /// </summary>
        /// <param name="count"></param>
        void GetCards(int count);
        
        /// <summary>
        /// 单位使用卡牌
        /// </summary>
        /// <param name="card"></param>
        /// <param name="self"></param>
        /// <param name="target"></param>
        void UnitUseCard(BaseCardSo card, AbsUnit self, AbsUnit target);
        
        /// <summary>
        /// 从玩家卡组中获取卡牌
        /// </summary>
        /// <param name="cardName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetCardFromUseCards<T>(string cardName) where T : BaseCardSo;
    }

    public class CardSystemModule : AbsModule, ICardSystemModule
    {
        private CardSystemViewCtrl _viewCtrl;
        private Dictionary<string, BaseCardSo> _loadCardsDic;
        private QArray<BaseCardSo> _nowUseCards;
        private QArray<BaseCardSo> _nowHeadCards;
        private QArray<BaseCardSo> _obsCards;
        

        protected override void OnInit()
        {
            _loadCardsDic = new Dictionary<string, BaseCardSo>();
            _nowUseCards = new QArray<BaseCardSo>(10);
            _nowHeadCards = new QArray<BaseCardSo>(10);
            _obsCards = new QArray<BaseCardSo>(10);
        }

        public void ShowView()
        {
            _viewCtrl ??= new CardSystemViewCtrl();
            _viewCtrl.OnShowView();
        }

        public void LoadUseCards()
        {
            //根据ids加载卡牌
            for (int i = 0; i < 5; i++)
            {
                var loadCard = ResMgr.GetInstance().SyncLoad<BaseCardSo>("PlayerCards/" + "普通攻击卡");
                _nowUseCards.AddFromTail(loadCard);
            }

            for (int i = 0; i < 5; i++)
            {
                var loadCard = ResMgr.GetInstance().SyncLoad<BaseCardSo>("PlayerCards/" + "普通防御卡");
                _nowUseCards.AddFromTail(loadCard);
            }
        }

        public void GetCards(int count)
        {
            //如果当前出战卡组剩余卡牌不足获取，则将弃牌队列存入出战卡组中
            if (_nowUseCards.Count < count)
            {
                //将弃牌队列中的卡牌加入出战卡组
                while (_obsCards.Count > 0)
                {
                    var card = _obsCards.GetFromHead();
                    _nowUseCards.AddFromTail(card);
                }
            }
            
            //根据要获取的卡牌数获取卡牌到手牌中
            for (int i = 0; i < count; i++)
            {
                var card = _nowUseCards.GetFromHead();
                _nowHeadCards.AddFromTail(card);
            }   
        }


        public void UnitUseCard(BaseCardSo card, AbsUnit self, AbsUnit target)
        {
            //使用卡牌逻辑
            card.UseCard(self, target);
            
            //将使用的卡牌丢入弃牌队列
            _obsCards.AddFromTail(card);
        }

        public T GetCardFromUseCards<T>(string cardName) where T : BaseCardSo
        {
            if (_loadCardsDic.TryGetValue(cardName, out var card))
            {
                return card as T;
            }

            var loadCard = ResMgr.GetInstance().SyncLoad<T>("PlayerCards/" + cardName);
            if (_loadCardsDic.TryAdd(cardName, loadCard))
            {
                return loadCard;
            }

            throw new Exception($"读取卡牌错误:{cardName}");
        }
    }
}