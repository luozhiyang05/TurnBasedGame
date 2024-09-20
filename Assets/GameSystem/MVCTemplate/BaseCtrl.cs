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
        protected bool IsLoad;
        protected BaseCtrl()
        {
            Init();
        }

        protected abstract void InitListener();

        protected abstract void RemoveListener();

        protected abstract void Init();

        public void ShowView()
        {
            UIManager.GetInstance().GetFromPool(GetPrefabPath(), EuiLayer.GameUI, (BaseView) =>
            {
                if (!IsLoad)
                {
                    Model = GetModel();
                    View = BaseView;
                    View.SetModel(Model);
                    View.SetClose(OnClose);
                    View.SetRelease(OnRelease);
                }

                InitListener();
                Model.BindListener();
                
                View.OnShow();
                OnShowComplate();
                
                IsLoad = true;
            });
        }

        public abstract BaseModel GetModel();

        public abstract BaseView GetView();

        public abstract string GetPrefabPath();

        public abstract void OnShowComplate();

        private void OnClose()
        {
            RemoveListener();
            Model.RemoveListener();

            UIManager.GetInstance().EnterPool(View);
        }

        private void OnRelease()
        {
            IsLoad = false;
            Model = null;
            View = null;
        }

        public IMgr Ins => Global.GetInstance();
    }
}