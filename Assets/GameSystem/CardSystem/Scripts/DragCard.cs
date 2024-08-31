using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameSystem.CardSystem.Scripts
{
    public class DragCard : DragCell
    {
        protected override void OnStartDrag(PointerEventData eventData)
        {
            transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;
        }

        protected override void OnFinishDrag(PointerEventData eventData)
        {
            ResetPos(()=>transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true);
        }
    }
}