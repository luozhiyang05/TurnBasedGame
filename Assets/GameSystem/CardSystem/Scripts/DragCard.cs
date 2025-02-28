using System.Collections.Generic;
using Assets.GameSystem.BattleSystem;
using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.CardSystem.Scripts.Cmd;
using Framework;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Assets.GameSystem.CardSystem.Scripts
{
    public class DragCard : DragCell, ICanGetSystem, ICanSendCmd, IPointerEnterHandler, IPointerExitHandler
    {
        public int headCardIdx;
        public BaseCard BaseCard;
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
                //RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                PointerEventData pointerEvent = new(EventSystem.current);
                pointerEvent.position = Input.mousePosition;
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEvent, raycastResults);
                if (raycastResults.Count > 0)
                {
                    if (raycastResults[0].gameObject.name.Equals("img_body"))
                    {
                        //获取鼠标光标下的Unit
                        GameObject currentObjectUnderCursor = raycastResults[0].gameObject;

                        //判断当前行动带你是否足够使用卡牌
                        var player = this.GetSystem<IBattleSystemModule>().GetPlayerUnit() as Player;
                        if (player.nowActPoint >= BaseCard.depletePoint)
                        {
                            //使用卡牌的命令
                            this.SendCmd<UseCardCmd, CardData>(new CardData()
                            {
                                user = player,
                                headCardIdx = headCardIdx,
                                cardSo = BaseCard,
                                target = currentObjectUnderCursor.transform.parent.gameObject
                            });
                            return;
                        }
                    }
                }
                else
                {
                    //当拖拽距离大于350且当前卡牌可以自动使用，则自动使用
                    var player = this.GetSystem<IBattleSystemModule>().GetPlayerUnit() as Player;
                    if (BaseCard.canAutoUse && GetDargDistance() > 350f)
                    {
                        // 判断行动点数是否够用
                        if (player.nowActPoint >= BaseCard.depletePoint)
                        {

                            //使用卡牌的命令
                            this.SendCmd<UseCardCmd, CardData>(new CardData()
                            {
                                user = player,
                                headCardIdx = headCardIdx,
                                cardSo = BaseCard,
                                target = player.gameObject  //自动使用的卡牌目标为自己
                            });
                            return;
                        }
                    }
                }
            }

            SetCanBlockRaycasts(true);
            this.GetSystem<ICardSystemModule>().NoDragCardAction(transform);

            ResetPos(() =>
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);
            }, 4000f);
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
        public BaseCard cardSo;
        public GameObject target;
    }

    public struct UseCardHistory
    {
        public string cardName;
        public string userName;
        public string targetName;
    }
}