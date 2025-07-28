using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Tool.UI
{
    public class UIBlock : MonoBehaviour, IPointerClickHandler
    {
        private UnityAction _clickCallback;
        public void SetClickCallback(UnityAction callback)
        {
            _clickCallback = callback;
        }
        public void ClearClickCallback()
        {
            _clickCallback = null;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (null != _clickCallback)
            {
                _clickCallback.Invoke();
            }
        }
    }
}