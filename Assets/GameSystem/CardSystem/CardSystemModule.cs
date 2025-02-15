using Assets.GameSystem.CardSystem.Main;
using Assets.GameSystem.CardSystem.ObsCard.Main;
using Assets.GameSystem.CardSystem.Scripts;
using Framework;
using Tool.Mono;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameSystem.CardSystem
{
    public interface ICardSystemModule : IModule
    {
        void ShowView();

        /// <summary>
        /// 打开观察卡牌视图
        /// </summary>
        /// <param name="obsCards"></param>
        /// <param name="isUseCards">true为牌库，false为弃牌堆</param>
        void ShowObsCardsView(QArray<BaseCardSo> obsCards, bool isUseCards);

        /// <summary>
        /// 回合结束丢弃手牌
        /// </summary>
        void UpdateHeadCardInEr();

        /// <summary>
        /// 回合开始更新手牌
        /// </summary>
        void UpdateHeadCardInSr();

        /// <summary>
        /// 渲染玩家所有手牌
        /// </summary>
        /// <param name="cardsContent"></param>
        void RenderHandCards(QArray<GameObject> cardGos, QArray<BaseCardSo> headCards, Transform cardContent);

        /// <summary>
        /// 渲染卡牌信息
        /// </summary>
        /// <param name="cardCell"></param>
        /// <param name="card"></param>
        void RenderCardInfo(Transform cardCell, BaseCardSo card);

        /// <summary>
        /// 渲染历史记录
        /// </summary>
        /// <param name="cardData"></param>
        void RenderHistoryInfo(Transform historyCell, UseCardHistory history);

        #region 卡牌动画
        /// <summary>
        /// 选择卡牌时的动画
        /// </summary>
        void SelectCardAction(Transform trans);

        /// <summary>
        /// 取消选中卡牌时的动画
        /// </summary>
        void NoSelectCardAction(Transform trans);

        /// <summary>
        /// 拖拽卡牌时的动画
        /// </summary>
        /// <param name="trans"></param>
        void DragCardAction(Transform trans);

        /// <summary>
        /// 取消拖拽卡牌时的动画
        /// </summary>
        /// <param name="trans"></param>
        void NoDragCardAction(Transform trans);
        #endregion
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

        public void ShowObsCardsView(QArray<BaseCardSo> obsCards, bool isUseCards)
        {
            var ctrl = new ObsCardViewCtrl(new object[] { obsCards.Clone(), isUseCards });
            ctrl.ShowView();
        }

        public void UpdateHeadCardInEr()
        {
            (_viewCtrl.GetModel() as CardSystemViewModel)?.UpdateHeadCardInEr();
        }

        public void UpdateHeadCardInSr()
        {
            (_viewCtrl.GetModel() as CardSystemViewModel)?.UpdateHeadCardInSr();
        }

        public void RenderHandCards(QArray<GameObject> cardGos, QArray<BaseCardSo> headCards, Transform cardContent)
        {
            int cardCnt = headCards.Count;
            bool isSingleCnt = cardCnt % 2 != 0;
            int offet = 150;
            int startX;
            if (isSingleCnt)
            {

                startX = -(cardCnt - 1) / 2 * offet;
                for (int i = 1; i <= cardCnt; i++)
                {
                    var cardTrans = cardGos[i - 1].transform;
                    cardTrans.localPosition = new Vector2(startX + offet * (i - 1), 0);
                    RenderCard(cardTrans, headCards[i - 1], i);
                }
            }
            else
            {
                if (cardCnt == 2)
                {
                    cardGos[0].transform.localPosition = new Vector2(-75, 0);
                    cardGos[1].transform.localPosition = new Vector2(75, 0);
                    RenderCard(cardGos[0].transform, headCards[0], 1);
                    RenderCard(cardGos[1].transform, headCards[1], 2);
                    return;
                }

                startX = -75 - (cardCnt / 2 - 1) * offet;
                for (int i = 1; i <= cardCnt; i++)
                {
                    var cardTrans = cardGos[i - 1].transform;
                    cardTrans.localPosition = new Vector2(startX + offet * (i - 1), 0);
                    RenderCard(cardTrans, headCards[i - 1], i);
                }

            }
            void RenderCard(Transform cardCell, BaseCardSo card, int index)
            {
                //对卡牌信息赋值
                if (!cardCell.gameObject.TryGetComponent<DragCard>(out DragCard dragCard))
                {
                    dragCard = cardCell.gameObject.AddComponent<DragCard>();
                    dragCard.headCardIdx = index - 1;
                    dragCard.BaseCardSo = card;
                }
                else
                {
                    dragCard.headCardIdx = index - 1;
                    dragCard.BaseCardSo = card;
                }

                //渲染卡牌名字和描述信息
                RenderCardInfo(cardCell, card);

                //激活
                cardCell.SetParent(cardContent);
                cardCell.gameObject.SetActive(true);
            }
        }

        public void RenderCardInfo(Transform cardCell, BaseCardSo card)
        {
            cardCell.Find("bg/txt_name").GetComponent<Text>().text = card.name;
            cardCell.Find("bg/txt_desc").GetComponent<Text>().text = card.cardDesc;
        }

        public void RenderHistoryInfo(Transform historyCell, UseCardHistory history)
        {
            var txtDesc = historyCell.Find("txt_desc").GetComponent<Text>();
            txtDesc.text = $"{history.userName} 对 {history.targetName} 使用了{history.cardName} 卡牌";
        }

        public void SelectCardAction(Transform trans)
        {
            var bg = trans.Find("bg");
            float percent = 0f;
            ActionKit.GetInstance().CreateActQue(trans.gameObject, () =>
            {
                percent += Time.deltaTime / 0.1f;
                bg.localPosition = new Vector3(bg.localPosition.x, Mathf.Lerp(0, 60, percent), bg.localPosition.z);
                bg.localScale = new Vector3(Mathf.Lerp(1, 1.1f, percent), Mathf.Lerp(1, 1.1f, percent), Mathf.Lerp(1, 1.1f, percent));
            }, 0.12f)
            .Execute();
        }

        public void NoSelectCardAction(Transform trans)
        {
            var bg = trans.Find("bg");
            float oldScale = bg.localScale.x;
            float percent = 0f;
            ActionKit.GetInstance().CreateActQue(trans.gameObject, () =>
            {
                percent += Time.deltaTime / 0.1f;
                bg.localPosition = new Vector3(bg.localPosition.x, Mathf.Lerp(60, 0, percent), bg.localPosition.z);
                bg.localScale = new Vector3(Mathf.Lerp(oldScale, 1, percent), Mathf.Lerp(oldScale, 1, percent), Mathf.Lerp(oldScale, 1, percent));

            }, 0.12f)
            .Execute();
        }

        public void DragCardAction(Transform trans)
        {
            var canvasGroup = trans.Find("bg").GetComponent<CanvasGroup>();
            float percent = 0f;
            ActionKit.GetInstance().CreateActQue(trans.gameObject, () =>
            {
                percent += Time.deltaTime / 0.1f;
                canvasGroup.alpha = Mathf.Lerp(1, 0.2f, percent);
            }, 0.12f)
            .Execute();
        }

        public void NoDragCardAction(Transform trans)
        {
            var canvasGroup = trans.Find("bg").GetComponent<CanvasGroup>();
            float percent = 0f;
            ActionKit.GetInstance().CreateActQue(trans.gameObject, () =>
            {
                percent += Time.deltaTime / 0.1f;
                canvasGroup.alpha = Mathf.Lerp(0.2f, 1, percent);
            }, 0.12f)
            .Execute();
        }
    }
}