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
            //View未释放
            if (IsLoad && View && !View.gameObject.activeInHierarchy)
            {
                InitListener();
                Model = GetModel();
                Model.BindListener();
                View.OnShow();
                OnShowComplate();
                return;
            }

            //view已经释放,需要重新加载预制体
            if (!IsLoad && !View)
            {
                UIManager.GetInstance().GetFromPool(GetPrefabPath(), EuiLayer.GameUI, (BaseView) =>
                {
                    IsLoad = true;
                    
                    View = BaseView;
                    View.SetModel(Model);
                    View.SetClose(OnClose);
                    View.SetRelease(OnRelease);

                    ShowView();
                });
            }
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
            //UIManager.GetInstance().UnloadView(View);
        }

        public IMgr Ins => Global.GetInstance();
    }
}