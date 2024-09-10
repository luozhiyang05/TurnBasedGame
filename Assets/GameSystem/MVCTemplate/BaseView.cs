using System.Runtime.Versioning;
using System.Threading;
using System;
using Framework;
using Tool.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Tool.Mono;

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

        protected override void AutoInitUI()
        {

        }

        private void Start()
        {
            BindModelListener();
        }

        public void SetModel(BaseModel model)
        {
            Model = model;
        }

        protected abstract void BindModelListener();

        protected override void OnInit()
        {

        }

        public override void OnShow()
        {
            isOpen = true;
            gameObject.SetActive(true);

            //重置倒计时
            ActionKit.GetInstance().RemoveTimer(GetInstanceID()+"OnRelease");

            if (UseMaskPanel) UIManager.GetInstance().OpenMaskPanel(this);
        }

        public override void OnHide()
        {
            isOpen = false;
            _closeCallback?.Invoke();
            gameObject.SetActive(false);

            //回收倒计时
            ActionKit.GetInstance().DelayTime(10f,GetInstanceID()+"OnRelease", OnRelease);
            
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