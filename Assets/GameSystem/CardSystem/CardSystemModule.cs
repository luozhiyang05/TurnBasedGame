using Framework;
using GameSystem.CardSystem.Main;
using GameSystem.CardSystem.Scripts;
using Tool.Mono;
using Tool.ResourceMgr;
using Tool.UI;
using Tool.Utilities;
using UnityEngine;

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
        /// 渲染玩家手牌位置和旋转
        /// </summary>
        /// <param name="cardsContent"></param>
        void RenderHandCardsPosAndRot(Transform cardsContent);

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
        
        public void RenderHandCardsPosAndRot(Transform cardContent)
        {
            var uiAnimationSo = ResMgr.GetInstance().SyncLoad<UIAnimationSo>("UIAnimationSo");
            var uiAnimationCurve = uiAnimationSo.handCardAnimCurve;
            var cardCnt = cardContent.childCount - 1;
            var angle = 60 / (cardCnt + 1);
            float addAngle = 30;
            float x = 1 / (float)(cardCnt - 1);
            for (int i = 1; i <= cardCnt; i++)
            {
                var cardGo = cardContent.GetChild(i);
                addAngle -= angle;
                cardGo.transform.localEulerAngles = new Vector3(0,0,addAngle);
                var addHeight = uiAnimationCurve.Evaluate((i - 1) * x) * 60;
                cardGo.transform.position = new Vector2(cardGo.transform.position.x, cardGo.transform.position.y + addHeight);
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