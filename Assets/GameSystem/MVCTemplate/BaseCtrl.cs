using Framework;
using Tool.UI;
using UnityEngine;

namespace GameSystem.MVCTemplate
{
    /// <summary>
    /// 功能：
    /// 连通model和view
    /// 将view层的某些业务放入ctrl
    /// 控制view的打开和关闭
    /// </summary>
    public abstract class BaseCtrl : ICanGetSystem
    {
        protected BaseModel Model;
        protected BaseView View;
        protected BaseCtrl()
        {
            Model = GetModel();
            View = GetView();
            OnCompleteLoad();
            View.SetModel(Model);
            View.SetClose(OnClose);
            Init();
        }

        protected abstract void InitListener();
        
        protected abstract void RemoveListener();

        protected abstract void Init();

        public void ShowView()
        {
            if (View.isOpen) return;
            InitListener();
            Model.BindListener();
            View.OnShow();
        }

        public abstract BaseModel GetModel();

        public abstract BaseView GetView();

        public abstract void OnCompleteLoad();

        private void OnClose()
        {
            RemoveListener();
            Model.RemoveListener();
        }

        private void OnRelease()
        {
            
        }

        public IMgr Ins => Global.GetInstance();
    }
}