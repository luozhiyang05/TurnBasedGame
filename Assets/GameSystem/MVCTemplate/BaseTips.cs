using System;
using Tool.UI;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem.MVCTemplate
{
    public abstract class BaseTips : BasePanel
    {
        private void Awake()
        {
            OnInit();
        }
        protected abstract override void OnInit();
        
        private void Open()
        {
            UIManager.GetInstance().InitTips(this);
            gameObject.SetActive(true);
            
        }

        public virtual void OnOpen()
        {
            Open();
        }


        protected void CloseTips()
        {
            Destroy(gameObject);
        }
       
    }
}