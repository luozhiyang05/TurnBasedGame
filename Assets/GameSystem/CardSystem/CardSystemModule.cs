using Framework;
using GameSystem.CardSystem.Main;
using Tool.Mono;
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

        
        public void SelectCardAction(Transform trans)
        {
            var bg = trans.Find("bg");
            float oldY = bg.localPosition.y;
            float percent = 0f;
            ActionKit.GetInstance().CreateActQue("选择卡牌", () =>
            {
                percent += Time.deltaTime / 0.1f;
                bg.localPosition = new Vector3(bg.localPosition.x, Mathf.Lerp(oldY, oldY + 60, percent), bg.localPosition.z);
                bg.localScale = new Vector3(Mathf.Lerp(1, 1.1f, percent), Mathf.Lerp(1, 1.1f, percent), Mathf.Lerp(1, 1.1f, percent));
            }, 0.1f)
            .Execute();
        }

        public void NoSelectCardAction(Transform trans)
        {
            var bg = trans.Find("bg");
            float oldY = bg.localPosition.y;
            float oldScale = bg.localScale.x;
            float percent = 0f;
            ActionKit.GetInstance().CreateActQue("取消卡牌", () =>
            {
                percent += Time.deltaTime / 0.1f;
                bg.localPosition = new Vector3(bg.localPosition.x, Mathf.Lerp(oldY, oldY - 60, percent), bg.localPosition.z);
                bg.localScale = new Vector3(Mathf.Lerp(oldScale, 1, percent), Mathf.Lerp(oldScale, 1, percent), Mathf.Lerp(oldScale, 1, percent));

            }, 0.1f)
            .Execute();
        }

        public void DragCardAction(Transform trans)
        {
            var canvasGroup = trans.Find("bg").GetComponent<CanvasGroup>();
            float percent = 0f;
            ActionKit.GetInstance().CreateActQue("拖拽卡牌", () =>
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
            ActionKit.GetInstance().CreateActQue("拖拽卡牌", () =>
            {
                percent += Time.deltaTime / 0.1f;
                canvasGroup.alpha = Mathf.Lerp(0.2f, 1, percent);
            }, 0.1f)
            .Execute();
        }
    }
}