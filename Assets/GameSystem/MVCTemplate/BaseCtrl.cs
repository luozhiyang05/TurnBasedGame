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
            View.SetModel(Model);
            View.SetClose(OnClose);
            Init();
            InitListener();
        }

        protected abstract void InitListener();
        
        protected abstract void RemoveListener();

        protected abstract void Init();

        protected void OnOpen()=>UIManager.GetInstance().OpenView(Model.ToString(), EuiLayer.Mid);

        public abstract BaseModel GetModel();

        public abstract BaseView GetView();

        private void OnClose()
        {
            RemoveListener();
            Model.Dispose();
        }

        public IMgr Ins => Global.GetInstance();
    }
}