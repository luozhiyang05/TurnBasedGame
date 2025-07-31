using System;
using Framework;
using Tool.UI;
using UnityEngine;
using UnityEngine.Events;
using Tool.AudioMgr;
using Tool.Utilities;
using Tool.Utilities.Events;

namespace GameSystem.MVCTemplate
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseView : BasePanel, IController
    {
        public Transform Main;
        [NonSerialized] public CanvasGroup CanvasGroup;
        [NonSerialized] public EuiLayer EuiLayer;
        [NonSerialized] public bool isOpen;
        protected BaseModel Model;
        private BaseView _fatherView;
        private UnityAction _closeCallback;
        private UnityAction _releaseCallback;
        private UnityAction<BaseView> _removeFormOpenViewsCallback;
        public string SystemPath => _systemPath;
        private string _systemPath;

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
        public void SetFatherView(BaseView fatherView) => _fatherView = fatherView;
        public bool CheckFatherViewIsNull() => _fatherView == null;
        protected void SetPath(string path)
        {
            var viewName = PathUtils.GetViewNameFromSystemPath(path);
            name = viewName;
            _systemPath = path;
        }

        protected abstract void BindModelListener();

        public override void OnShow(params object[] args)
        {
            CalculateParticalSort();
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
            _removeFormOpenViewsCallback?.Invoke(this);
            if (UseMaskPanel) UIManager.GetInstance().CloseMaskPanel(this);
        }

        public void SetRemoveFromOpenViewsCallback(UnityAction<BaseView> callback)
        {
            _removeFormOpenViewsCallback = callback;
        }

        public void SetClose(UnityAction callback)
        {
            _closeCallback = () =>
            {
                callback?.Invoke();
                UIManager.GetInstance().EnterIdlePool(this);
            };
        }

        public void SetRelease(UnityAction<BaseView> callback)
        {
            _releaseCallback = () =>
            {
                callback?.Invoke(this);
                EventsHandle.EventTrigger(EventsNameConst.RELEASE_VIEW, name);
            };
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