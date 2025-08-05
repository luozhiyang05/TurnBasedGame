using UnityEngine;
using UnityEngine.Events;

namespace Assets.GameSystem.MusicNoteSystem.Main
{
    public class NoteMove : MonoBehaviour
    {
        private bool _isMove;
        public float createTime;
        private UnityAction _realeaseCallback;
        void OnEnable()
        {
            _isMove = false;
        }
        public void SetNoteData(float createTime, UnityAction releaseCallback)
        {
            _isMove = true;
            this.createTime = createTime;
            _realeaseCallback = releaseCallback;
        }
        public void PressNote()
        {
            _realeaseCallback?.Invoke();
        }
        void Update()
        {
            if (_isMove)
            {
                float speed = 1596 / 3;
                var targetPos = transform.localPosition;
                transform.localPosition = new Vector2(targetPos.x - speed * Time.deltaTime, targetPos.y);

                if (transform.localPosition.x <= -2040)
                {
                    _realeaseCallback?.Invoke();
                }
            }
        }
    } 
}