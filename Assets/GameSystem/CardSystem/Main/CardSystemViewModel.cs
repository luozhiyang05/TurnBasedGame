using System;
using System.Transactions;
using GameSystem.CardSystem.Scripts;
using GameSystem.MVCTemplate;
using Tool.Mono;
using Tool.ResourceMgr;
using Tool.Utilities;
using Tool.Utilities.Events;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem.CardSystem.Main
{
    public class CardSystemViewModel : BaseModel
    {
        private QArray<BaseCardSo> _nowUseCards;
        private QArray<BaseCardSo> _nowHeadCards;
        private QArray<BaseCardSo> _obsCards;
        
        private UnityAction _updateViewCallback;
        private UnityAction _useCardCallback;
        
        protected override void OnInit()
        {
            _nowUseCards = new QArray<BaseCardSo>(10);
            _nowHeadCards = new QArray<BaseCardSo>(10);
            _obsCards = new QArray<BaseCardSo>(10);
        }

        /// <summary>
        /// 监听某些数据更改事件,可以通知view更新
        /// </summary>
        public override void BindListener()
        {
            //监听使用卡牌事件
            EventsHandle.AddListenEvent<CardData>(EventsNameConst.SUCCESS_USE_CARD, (CardData) =>
            {
                //丢弃卡牌
                DiscardCards(CardData.headCardIdx);
                //更新视图
                if (_updateViewCallback != null)
                {
                    _updateViewCallback.Invoke();
                }
            });
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        public override void RemoveListener()
        {
            EventsHandle.RemoveOneEventByEventName<CardData>(EventsNameConst.SUCCESS_USE_CARD, (CardData) =>
            {
                //丢弃卡牌
                DiscardCards(CardData.headCardIdx);
                //更新视图
                if (_updateViewCallback != null)
                {
                    _updateViewCallback.Invoke();
                }
            });
        }

        /// <summary>
        /// 回合结束时回调事件
        /// </summary>
        /// <param name="callback"></param>
        public void SetUpdateViewCallback(UnityAction callback)
        {
            _updateViewCallback = callback;
        }
        
        /// <summary>
        /// 加载玩家出战卡牌
        /// </summary>
        public void LoadUseCards()
        {
            //根据ids加载卡牌
            for (int i = 0; i < 5; i++)
            {
                var loadCard = ResMgr.GetInstance().SyncLoad<BaseCardSo>("PlayerCards/" + "普通攻击卡");
                _nowUseCards.Add(loadCard);
            }

            for (int i = 0; i < 5; i++)
            {
                var loadCard = ResMgr.GetInstance().SyncLoad<BaseCardSo>("PlayerCards/" + "普通防御卡");
                _nowUseCards.Add(loadCard);
            }
        }

        /// <summary>
        /// 从玩家出战卡牌中获取卡牌到手牌
        /// </summary>
        /// <param name="count"></param>
        public void GetCardsFormUseCards(int count)
        {
            //如果当前出战卡组剩余卡牌不足获取，则将弃牌堆中的卡牌洗入出战卡组中
            if (_nowUseCards.Count < count)
            {
                //将弃牌队列中的卡牌加入出战卡组
                while (_obsCards.Count > 0)
                {
                    var card = _obsCards.RemoveRange();
                    _nowUseCards.Add(card);
                }
            }

            //根据要获取的卡牌数获取卡牌到手牌中
            for (int i = 0; i < count; i++)
            {
                var card = _nowUseCards.GetFromHead();
                _nowHeadCards.Add(card);
            }
        }

        /// <summary>
        /// 获取目前玩家的手牌用于view展示
        /// </summary>
        /// <returns></returns>
        public QArray<BaseCardSo> GetNowHeadCards()
        {
            return _nowHeadCards;
        }

        /// <summary>
        /// 丢弃卡牌到弃牌堆中
        /// </summary>
        public void DiscardCards(int headCardIdx = -1)
        {
            if (headCardIdx != -1)
            {
                var cardSo = _nowHeadCards.RemoveAt(headCardIdx);
                _obsCards.Add(cardSo);
                _useCardCallback?.Invoke();
                return;
            }
            while (_nowHeadCards.Count > 0)
            {
                var card = _nowHeadCards.GetFromHead();
                _obsCards.Add(card);
            }
        }

        /// <summary>
        /// 回合开始时，更新玩家手牌
        /// </summary>
        public void UpdateHeadCardInSr()
        {
            //获取新的卡牌
            GetCardsFormUseCards(9);
            
            //通知视图更新
            _updateViewCallback?.Invoke();
        }

        /// <summary>
        /// 回合结束时，丢弃玩家手牌
        /// </summary>
        public void UpdateHeadCardInEr()
        {
            //丢弃旧的卡牌
            DiscardCards();
            
            //通知视图更新
            _updateViewCallback?.Invoke();
        }
    }
}