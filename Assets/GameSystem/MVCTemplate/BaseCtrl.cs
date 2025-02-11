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
        protected BaseCtrl(params object[] args)
        {
            Init(args);
        }

        protected abstract void InitListener();

        protected abstract void RemoveListener();

        protected abstract void Init(params object[] args);

        public void ShowView(EuiLayer euiLayer = EuiLayer.GameUI,params object[] args)
        {
            // 没有加载或者已经加载但是没有激活，则去池子中处理
            if (!IsLoad || (IsLoad && !View.isOpen))
            {
                UIManager.GetInstance().GetFromPool(GetPrefabPath(), euiLayer, (BaseView) =>
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
                    
                    Model.Init();
                    Model.BindListener();

                    OnBeforeShow(args);
                    View.OnShow();
                    OnShowComplate(args);

                    IsLoad = true;
                });
            }


        }

        public abstract BaseModel GetModel();

        public abstract BaseView GetView();

        public abstract string GetPrefabPath();

        public abstract void OnBeforeShow(params object[] args);

        public abstract void OnShowComplate(params object[] args);

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