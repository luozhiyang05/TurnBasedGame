using System;
using Tool.Mono;
using Tool.UI;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem.MVCTemplate
{
    public abstract class BaseTips : BasePanel
    {
        protected BaseModel _baseModel;
        public static BaseTips New(string path)
        {          
            var uiMgr = UIManager.GetInstance();
           
            if (!uiMgr.HasLoadTips(path))
            {
                return uiMgr.LoadTips(path, EuiLayer.TipsUI);
            }
            return uiMgr.GetTips(path);
        }

        void Awake()
        {
            OnInit();
        }

        void Update()
        {
        }

        protected override void OnInit()
        {
        }

        public virtual void Init(BaseModel baseModel)
        {
            _baseModel = baseModel;
        }

        public override void OnShow()
        {
            gameObject.SetActive(true);
            if (UseMaskPanel) UIManager.GetInstance().OpenMaskPanel(this);
        }

        public override void OnHide()
        {
            gameObject.SetActive(false);
            if (UseMaskPanel) UIManager.GetInstance().CloseMaskPanel();
        }

        
        public override void OnRelease()
        {
            _baseModel = null;
        }

        /// <summary>
        /// 点击遮罩事件
        /// </summary>
        public override void OnClickMaskPanel()
        {
            OnHide();
        }
    }
}