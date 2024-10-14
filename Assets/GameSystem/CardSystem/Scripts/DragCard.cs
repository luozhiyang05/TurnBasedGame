using Framework;
using GameSystem.BattleSystem;
using GameSystem.BattleSystem.Scripts;
using GameSystem.CardSystem.Scripts.Cmd;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameSystem.CardSystem.Scripts
{
    public class DragCard : DragCell,ICanGetSystem,ICanSendCmd,IPointerEnterHandler,IPointerExitHandler
    {
        public int headCardIdx;
        public BaseCardSo BaseCardSo;
        private int _idx;
        
        protected override void OnStartDrag(PointerEventData eventData)
        {
            this.GetSystem<ICardSystemModule>().DragCardAction(transform);
        }

        protected override void OnFinishDrag(PointerEventData eventData)
        {
            SetCanBlockRaycasts(false);
            
            if (Camera.main != null)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit.collider != null)
                {
                    GameObject currentObjectUnderCursor = hit.collider.gameObject;
                    //使用卡牌的命令
                    this.SendCmd<UseCardCmd,CardData>(new CardData()
                    { 
                        user = this.GetSystem<IBattleSystemModule>().GetPlayerUnit(),
                        headCardIdx = headCardIdx,
                        cardSo = BaseCardSo,
                        target = currentObjectUnderCursor
                    });
                    return;
                }
            }

            SetCanBlockRaycasts(true);
            this.GetSystem<ICardSystemModule>().NoDragCardAction(transform);

            ResetPos(() =>{
                //transform.SetSiblingIndex(headCardIdx);
                LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);
            },4000f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _idx = transform.GetSiblingIndex();
            SetTop(true);

            this.GetSystem<ICardSystemModule>().SelectCardAction(transform);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetTop(false);
            transform.SetSiblingIndex(_idx);

            this.GetSystem<ICardSystemModule>().NoSelectCardAction(transform);
        }

        public IMgr Ins => Global.GetInstance();
    }

    public struct CardData
    {
        public AbsUnit user;
        public int headCardIdx;
        public BaseCardSo cardSo;
        public GameObject target;
    }
}