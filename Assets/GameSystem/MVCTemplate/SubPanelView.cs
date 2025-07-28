using System;
using Framework;
using Tool.AudioMgr;
using UnityEngine.Events;

namespace GameSystem.MVCTemplate
{
    public abstract class SubPanelView : BasePanel, IController
    {
        [NonSerialized] public bool isOpen;
        protected BaseModel Model;
        public BaseView FatherView
        {
            set => _fatherView = value;
            get => _fatherView;
        }
        private BaseView _fatherView;
        private UnityAction _closeCallback;
        private void Awake()
        {
            AutoInitUI();
            OnInit();
        }

        protected override void AutoInitUI() { }
        protected override void OnInit() => Init();
        protected abstract void Init();
        protected abstract void Open(params object[] args);
        private void OnEnable() => isOpen = true;
        private void OnDisable() => isOpen = false;
        public void SetModel(BaseModel model) => Model = model;
        protected void SetName(string viewName) => name = viewName;
        protected abstract void BindModelListener();
        protected abstract void RemoveModelListener();
        public void SetCloseCallback(UnityAction callback) => _closeCallback = callback;
        public override void OnShow(params object[] args)
        {
            if (useAudio)
            {
                PlayAudio(EAudioType.Effect);
                PlayAudio(EAudioType.Bgm);
            }
            transform.SetAsLastSibling();
            BindModelListener();
            gameObject.SetActive(true);

            Open(args);
        }
        public override void OnHide()
        {
            if (isOpen == false)
            {
                return;
            }
            if (useAudio)
            {
                PlayAudio(EAudioType.Effect, false);
                CloseBgm();
            }
            RemoveModelListener();
            gameObject.SetActive(false);
        }
        protected void Close() => _closeCallback?.Invoke();
        public IMgr Ins => Global.GetInstance();
    }
}