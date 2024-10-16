using Framework;
using GameSystem.CardSystem.Main;
using GameSystem.CardSystem.Scripts;
using Tool.Mono;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.UI;

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

        /// <summary>
        /// 渲染玩家所有手牌
        /// </summary>
        /// <param name="cardsContent"></param>
        void  RenderHandCards(QArray<GameObject> cardGos,QArray<BaseCardSo> headCards,Transform cardContent);

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
        
        public void RenderHandCards(QArray<GameObject> cardGos,QArray<BaseCardSo> headCards,Transform cardContent)
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
                    var cardTrans = cardGos[i-1].transform;
                    cardTrans.localPosition = new Vector2(startX + offet * (i - 1), 0);
                    RenderCard(cardTrans,  headCards[i - 1], i);
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
                    var cardTrans = cardGos[i-1].transform;
                    cardTrans.localPosition = new Vector2(startX + offet * (i - 1), 0);
                    RenderCard(cardTrans, headCards[i - 1], i);
                }

            }
            void RenderCard(Transform cardTrans, BaseCardSo card, int index)
            {
                //对卡牌信息赋值
                if (!cardTrans.gameObject.TryGetComponent<DragCard>(out DragCard dragCard))
                {
                    dragCard = cardTrans.gameObject.AddComponent<DragCard>();
                    dragCard.headCardIdx = index - 1;
                    dragCard.BaseCardSo = card;
                }
                else
                {
                    dragCard.headCardIdx = index - 1;
                    dragCard.BaseCardSo = card;
                }

                //渲染卡牌名字和描述
                cardTrans.Find("bg/txt_name").GetComponent<Text>().text = card.name;
                cardTrans.Find("bg/txt_desc").GetComponent<Text>().text = card.cardDesc;
                //激活
                cardTrans.SetParent(cardContent);
                cardTrans.gameObject.SetActive(true);
            }
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
            }, 0.1f)
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

            }, 0.1f)
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
            }, 0.1f)
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
            }, 0.1f)
            .Execute();
        }
    }
}