using Framework;
using GameSystem.BattleSystem;
using GameSystem.BattleSystem.Scripts;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameSystem.CardSystem.Scripts
{
    public class DragCard : DragCell,ICanGetSystem
    {
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
                    Debug.Log("Object under cursor: " + currentObjectUnderCursor.name);
                    // 检测到单位后执行逻辑

                    var absUnit = currentObjectUnderCursor.GetComponent<AbsUnit>();
                    if (absUnit)
                    {
                        var playerUnit = this.GetSystem<IBattleSystemModule>().GetPlayerUnit();
                        (playerUnit as Player)?.UseCard(BaseCardSo,absUnit);
                    }
                }
            }

            ResetPos(() => transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true);
        }

        public IMgr Ins => Global.GetInstance();
    }
}