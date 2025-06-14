using Tool.Mono;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Tool.Utilities
{
    public abstract class DragCell : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector2 _oldPos = Vector2.one;
        private Vector3 _offset = Vector3.one;
        protected RectTransform _rectTrans;
        protected Transform _parent;
        private CanvasGroup _canvasGroup;
        private bool _canDrag = true;
        private UnityAction _resetCallback;

        private void Awake()
        {
            _canvasGroup = transform.GetComponent<CanvasGroup>() == null
                ? gameObject.AddComponent<CanvasGroup>()
                : gameObject.GetComponent<CanvasGroup>();
            _rectTrans = transform as RectTransform;
            _parent = transform.parent;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_canDrag) return;

            OnStartDrag(eventData);

            _oldPos = _rectTrans.position;

            RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTrans, eventData.position,
                eventData.enterEventCamera, out var pos);
            _offset = pos - _rectTrans.position;
        }

        protected abstract void OnStartDrag(PointerEventData eventData);

        public void OnDrag(PointerEventData eventData)
        {
            if (!_canDrag) return;

            OnDragging(eventData);

            RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTrans, eventData.position,
                eventData.enterEventCamera, out var pos);
            _rectTrans.position = pos - _offset;
        }

        protected virtual void OnDragging(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_canDrag) return;

            OnFinishDrag(eventData);
        }

        protected abstract void OnFinishDrag(PointerEventData eventData);

        private float _moveSpeed = 1;

        protected void ResetPos(UnityAction resetCallback = null, float speed = 1000)
        {
            _resetCallback = resetCallback;
            _moveSpeed = speed;
            _canDrag = false;
            PublicMonoKit.GetInstance().GetPublicMono().OnRegisterUpdate(ResetAnimation);
        }

        private void ResetAnimation()
        {
            _rectTrans.position =
                Vector3.MoveTowards(_rectTrans.position, _oldPos, _moveSpeed * Time.deltaTime); //对比连个vector3的值
            if (!(Vector3.Distance(_rectTrans.position, _oldPos) < 0.01f)) return;
            _rectTrans.position = _oldPos;
            _resetCallback?.Invoke();
            _canDrag = true;
            PublicMonoKit.GetInstance().GetPublicMono().OnUnRegisterUpdate(ResetAnimation);
        }

        public float GetDargDistance()
        {
            return Vector2.Distance(Input.mousePosition, _oldPos);
        }

        protected void SetTop(bool isTop)
        {
            _rectTrans.SetParent(isTop ? _parent.parent : _parent);
        }

        protected void SetCanBlockRaycasts(bool isBlock)
        {
            _canvasGroup.blocksRaycasts = isBlock;
        }
    }
}