using Tool.CustomAttribute;
using Tool.Mono;
using UnityEngine;

namespace UIComponents
{
    public class CButtonClickScale : MonoBehaviour
    {
        [CustomPropertyText("光标碰到缩放比")]
        public float mouseUpScale = 1.15f;

        [CustomPropertyText("点击缩放比")]
        public float clickScale = 0.9f;
        [CustomPropertyText("缩放时间")]
        public float time = 0.05f;
        private Vector3 originScale;
        private Vector3 nowScale;
        private Vector3 clickTargetScale;
        private Vector3 mouseUpTragetScale;
        private bool isScale = false;
        private bool mouseIsUp = false;
        private float _time = 0;
        private void Start()
        {
            originScale = transform.localScale;
            nowScale = transform.localScale;
            clickTargetScale = originScale * clickScale;
            mouseUpTragetScale = originScale * mouseUpScale;
        }

        public void Update()
        {
            // nowScale = transform.localScale;

            if (isScale == false) return;
            _time += Time.deltaTime;
        }

        public void MouseUpChangeScale(bool up)
        {
            nowScale = transform.localScale;
            mouseIsUp = up;
            //如果当前正在缩放，立马删除当前动画队列
            if (isScale)
            {
                ActionKit.GetInstance().KillActQue(gameObject);
                _time = 0;
            }

            //动画队列
            isScale = true;
            ActionKit.GetInstance().CreateActQue(gameObject, () =>
            {
                transform.localScale = Vector3.Lerp(nowScale, up ? mouseUpTragetScale : originScale, _time / time);
            }, time)
            .Append(() =>
            {
                transform.localScale = up ? mouseUpTragetScale : originScale;
                isScale = false;
                _time = 0;
            }, 0f)
            .KillAuto(true)
            .Execute();
        }

        public void ClickChangeScale(bool down)
        {
            nowScale = transform.localScale;

            //如果当前正在缩放，立马删除当前动画队列
            if (isScale)
            {
                ActionKit.GetInstance().KillActQue(gameObject);
            }

            //动画队列
            isScale = true;
            ActionKit.GetInstance().CreateActQue(gameObject, () =>
            {
                //保证光标在按钮上时的原始缩放比
                transform.localScale = Vector3.Lerp(nowScale, down ? clickTargetScale : (mouseIsUp ? mouseUpTragetScale : originScale), _time / time);
            }, time)
            .Append(() =>
            {
                transform.localScale = down ? clickTargetScale : (mouseIsUp ? mouseUpTragetScale : originScale);
                isScale = false;
                _time = 0;
            }, 0f)
            .KillAuto(true)
            .Execute();
        }
    }
}

