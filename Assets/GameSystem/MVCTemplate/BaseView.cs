using System;
using Framework;
using Tool.UI;
using UnityEngine;
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

        private void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            AutoInitUI();
        }

        protected override void AutoInitUI()
        {
            
        }

        private void Start()
        {
            BindModelListener();
            OnInit();
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
            Model.BindListener();
            gameObject.SetActive(true);
        }

        public override void OnHide()
        {
            isOpen = false;
            Model.RemoveListener();
            UIManager.GetInstance().ClosePanel(EuiLayer);
        }

        public IMgr Ins => Global.GetInstance();
    }
}