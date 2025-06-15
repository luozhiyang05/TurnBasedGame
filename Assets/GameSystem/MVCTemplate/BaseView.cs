using System;
using Framework;
using Tool.UI;
using UnityEngine;
using UnityEngine.Events;
using Tool.AudioMgr;

namespace GameSystem.MVCTemplate
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseView : BasePanel, IController
    {
        [NonSerialized] public CanvasGroup CanvasGroup;
        [NonSerialized] public EuiLayer EuiLayer;
        [NonReorderable] public bool isOpen;
        protected BaseModel Model;

        private UnityAction _closeCallback;
        private UnityAction _releaseCallback;

        private void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            AutoInitUI();
            OnInit();
        }

        protected override void AutoInitUI() { }
        protected override void OnInit() { }
        private void Start() => BindModelListener();
        private void OnEnable() => isOpen = true;
        private void OnDisable() => isOpen = false;
        public void SetModel(BaseModel model) => Model = model;
        protected void SetName(string viewName) => name = viewName;

        protected abstract void BindModelListener();

        public override void OnShow()
        {
            if (useAudio)
            {
                PlayAudio(EAudioType.Effect);
                PlayAudio(EAudioType.Bgm);
            }
            transform.SetAsLastSibling();
            if (UseMaskPanel) UIManager.GetInstance().OpenMaskPanel(this);
        }

        public override void OnHide()
        {
            if (isOpen==false)
            {
                return;
            }
            if (useAudio)
            {
                PlayAudio(EAudioType.Effect, false);
                CloseBgm();
            }
            _closeCallback?.Invoke();
            gameObject.SetActive(false);
            if (UseMaskPanel) UIManager.GetInstance().CloseMaskPanel();
        }

        public void SetClose(UnityAction callback)
        {
            _closeCallback = callback;
        }

        public void SetRelease(UnityAction callback)
        {
            _releaseCallback = callback;
        }

        /// <summary>
        /// 点击遮罩事件
        /// </summary>
        public override void OnClickMaskPanel()
        {
            OnHide();
        }

        public override void OnRelease()
        {
            _releaseCallback?.Invoke();
        }

        public IMgr Ins => Global.GetInstance();
    }
}