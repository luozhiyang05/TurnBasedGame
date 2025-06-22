using Tool.AudioMgr;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIComponents
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(CButtonClickScale))]
    public class CButton : Button
    {
        public Text Label;
        private ButtonClickedEvent _onClick = new ButtonClickedEvent();
        private CButtonClickScale _clickScale;

        protected override void Awake()
        {
            _clickScale = transform.GetComponent<CButtonClickScale>();
        }
        public string Text
        {
            get { return Label.text; }
            set { Label.text = value; }
        }

        public void Init(string content, string clickAudioPath = null)
        {
            Text = content;
            onClick.AddListener(() =>
            {
                if (clickAudioPath != null)
                {
                    AudioManager.GetInstance().PlayAudio(clickAudioPath);
                }
            });
        }

        public void AddListener(UnityEngine.Events.UnityAction action)
        {
            _onClick.AddListener(action);
        }

        public void RemoveListener(UnityEngine.Events.UnityAction action)
        {
            _onClick.RemoveListener(action);
        }

        public void RemoveAllListener()
        {
            _onClick.RemoveAllListeners();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            _onClick.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _clickScale.ClickChangeScale(true);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _clickScale.ClickChangeScale(false);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            _clickScale.MouseUpChangeScale(true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            _clickScale.MouseUpChangeScale(false);
        }
    }
}