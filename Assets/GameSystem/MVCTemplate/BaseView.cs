using System;
using Framework;
using Tool.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameSystem.MVCTemplate
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseView : BasePanel, IController
    {
        [NonSerialized] public CanvasGroup CanvasGroup;
        [NonSerialized] public EuiLayer EuiLayer;
        public bool isOpen;
        protected BaseModel Model;
        private UnityAction _closeCallback;

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

            if (UseMaskPanel) UIManager.GetInstance().OpenMaskPanel(this);
        }

        public override void OnHide()
        {
            isOpen = false;
            _closeCallback?.Invoke();
            gameObject.SetActive(false);
            
            if (UseMaskPanel) UIManager.GetInstance().CloseMaskPanel();
        }

        public void SetClose(UnityAction callback)
        {
            _closeCallback = callback;
        }

        /// <summary>
        /// 点击遮罩事件
        /// </summary>
        public override void OnClickMaskPanel()
        {
            OnHide();
        }

        public IMgr Ins => Global.GetInstance();
    }
}