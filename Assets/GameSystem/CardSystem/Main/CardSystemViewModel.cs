using Assets.GameSystem.BattleSystem;
using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.CardSystem.Scripts;
using Framework;
using GameSystem.MVCTemplate;
using Tool.ResourceMgr;
using Tool.Utilities;
using Tool.Utilities.Events;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.GameSystem.CardSystem.Main
{
    public class CardSystemViewModel : BaseModel
    {
        private QArray<BaseCard> _nowUseCards;    // 出战卡牌
        private QArray<BaseCard> _nowHeadCards;   // 手牌
        private QArray<BaseCard> _discardCards;   // 弃牌堆
        private QArray<UseCardHistory> _usedCardsHistory;   // 历史记录

        private QArray<int> rangeIndexs = new QArray<int>(); // 获取卡牌的随机索引
        private UnityAction _updateViewCallback;
        private UnityAction<int> _useCardCallback;

        public override void Init()
        {
            _nowUseCards = new QArray<BaseCard>(10);
            _nowHeadCards = new QArray<BaseCard>(10);
            _discardCards = new QArray<BaseCard>(10);
            _usedCardsHistory = new QArray<UseCardHistory>(10);
        }

        /// <summary>
        /// 监听某些数据更改事件,可以通知view更新
        /// </summary>
        public override void BindListener()
        {
            //监听使用卡牌事件
            EventsHandle.AddListenEvent<CardData>(EventsNameConst.SUCCESS_USE_CARD, UseCardDelegate);
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        public override void RemoveListener()
        {
            EventsHandle.RemoveOneEventByEventName<CardData>(EventsNameConst.SUCCESS_USE_CARD, UseCardDelegate);
        }

        /// <summary>
        /// 加载玩家出战卡牌
        /// </summary>
        public void LoadUseCards(int cardGroupId)
        {
            // 根据卡组id，获取卡组中的卡牌id
            var cardGroup = this.GetSystem<ICardSystemModule>().GetCardsId(cardGroupId);
            for (int i = 0; i < cardGroup.Count; i++)
            {
                _nowUseCards.Add(this.GetSystem<ICardSystemModule>().GetCardById(cardGroup[i]));
            }
        }

        /// <summary>
        /// 根据要计算的卡牌个数，计算随机索引
        /// </summary>
        /// <param name="count"></param>
        public void ComputeRangeIndexs(int count)
        {
            if (count-rangeIndexs.Count > 0)
            {
                while (rangeIndexs.Count != count)
                {
                    var index = UnityEngine.Random.Range(0, _nowUseCards.Count)+1;
                    if (!rangeIndexs.ContainValue(index))
                    {
                        rangeIndexs.Add(index);
                    }
                }
            }
        }

        /// <summary>
        /// 从玩家出战卡牌中获取卡牌到手牌
        /// </summary>
        /// <param name="count"></param>
        public void GetCardsFormUseCards(int count, bool needUpdateView = false)
        {
            //如果当前出战卡组剩余卡牌不足获取，则将弃牌堆中的卡牌洗入出战卡组中
            if (_nowUseCards.Count < count)
            {
                //将弃牌队列中的卡牌加入出战卡组
                while (_discardCards.Count > 0)
                {
                    var card = _discardCards.RemoveRange();
                    _nowUseCards.Add(card);
                }
            }

            // 如果当前随机索引数量不满足要获取的卡牌数，先获取足够的随机索引
            ComputeRangeIndexs(count);

            //根据要获取的卡牌数，使用随机索引获取卡牌到手牌中
            var cnt = rangeIndexs.Count;
            var tempCards = new QArray<BaseCard>(count);
            for (int i = 0; i < cnt; i++)
            {
                var card = _nowUseCards[rangeIndexs[0]-1];
                tempCards.Add(card);
                _nowHeadCards.Add(card);
                // _nowUseCards.Remove(card);   不能在这移除，因为移除后，后面的索引会前移，索引可能会出错报错
                rangeIndexs.RemoveAt(0);
            }
            for (int i = 0; i < tempCards.Count; i++)
            {
                _nowUseCards.Remove(tempCards[i]);
            }

            // 是否需要手动更新卡牌试图，一般用于手动获取卡牌时
            if (needUpdateView)
            {
                UpdateView();
            }
        }

        /// <summary>
        /// 丢弃卡牌到弃牌堆中
        /// </summary>
        public void DiscardCards(int headCardIdx = -1)
        {
            // 根据手牌索引，丢弃指定卡牌
            if (headCardIdx != -1)
            {
                var cardSo = _nowHeadCards.RemoveAt(headCardIdx);
                _discardCards.Add(cardSo);
                return;
            }

            // 丢弃所有可以丢弃的卡牌
            for (int i = 0; i < _nowHeadCards.Count; i++)
            {
                // 将可以丢弃的卡牌丢弃
                if (_nowHeadCards[i].discard)
                {
                    var card = _nowHeadCards.RemoveAt(i);
                    _discardCards.Add(card);
                    i--;    //丢弃后，后面的元素会前移，所以要重新检测当前索引的卡牌
                }
            }
        }

        /// <summary>
        /// 回合开始时，更新玩家手牌
        /// </summary>
        public void UpdateHeadCardInSr()
        {
            //获取新的卡牌
            var characterData = this.GetSystem<IBattleSystemModule>().GetCharacterData();
            GetCardsFormUseCards(characterData.maxHeadCardCnt);

            //通知视图更新
            UpdateView();
        }

        /// <summary>
        /// 回合结束时，丢弃玩家手牌
        /// </summary>
        public void UpdateHeadCardInEr()
        {
            //丢弃旧的卡牌
            DiscardCards();

            //通知视图更新
            UpdateView();
        }

        /// <summary>
        /// 获取目前玩家的手牌用于view展示
        /// </summary>
        /// <returns></returns>
        public QArray<BaseCard> GetNowHeadCards()
        {
            return _nowHeadCards;
        }

        /// <summary>
        /// 获取弃牌堆中的卡牌
        /// </summary>
        /// <returns></returns>
        public QArray<BaseCard> GetDiscardCards()
        {
            return _discardCards;
        }

        /// <summary>
        /// 获取玩家所有卡牌
        /// </summary>
        /// <returns></returns>
        public QArray<BaseCard> GetUserCards()
        {
            return _nowUseCards;
        }

        /// <summary>
        /// 获取使用卡牌的历史记录
        /// </summary>
        /// <returns></returns>
        public QArray<UseCardHistory> GetHistory()
        {
            return _usedCardsHistory;
        }

        #region 事件
        private void UseCardDelegate(CardData CardData)
        {
            Debug.Log("出牌的序号为：" + CardData.headCardIdx + ";" + "出牌的名字为：" + CardData.cardSo.cardName);
            //记录历史
            _usedCardsHistory.Add(new UseCardHistory
            {
                cardName = CardData.cardSo.cardName,
                userName = CardData.user.name,
                targetName = CardData.target.name
            });
            //丢弃卡牌
            DiscardCards(CardData.headCardIdx);
            //更新视图
            UseCard(CardData.headCardIdx);
        }
        #endregion

        #region 回调
        public void SetUpdateViewCallback(UnityAction callback)
        {
            _updateViewCallback = callback;
        }

        private void UpdateView()
        {
            _updateViewCallback?.Invoke();
        }

        public void SetUseCardCallback(UnityAction<int> callback)
        {
            _useCardCallback = callback;
        }

        private void UseCard(int headCardIdx)
        {
            _useCardCallback?.Invoke(headCardIdx);
        }
        #endregion
    }
}