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
    public class DragCard : DragCell,ICanGetSystem,ICanSendCmd
    {
        public int headCardIdx;
        public BaseCardSo BaseCardSo;
        
        protected override void OnStartDrag(PointerEventData eventData)
        {
            transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;
        }

        protected override void OnFinishDrag(PointerEventData eventData)
        {
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
                }
            }

            ResetPos(() =>{
                transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;
                LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);
            });
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