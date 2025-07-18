using Framework;
using Tool.UI;
using Tool.Utilities;
using Tool.Utilities.Events;
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
        public string MainViewName => _mainViewName;
        private string _mainViewName;
        private string _openViewName;
        protected BaseModel Model;
        protected BaseView View;
        protected bool isLoad;
        protected bool isOpenedMainView;
        protected BaseCtrl()
        {
            _mainViewName = GetPrefabPath();
            Init();
        }
        protected BaseCtrl(params object[] args)
        {
            _mainViewName = GetPrefabPath();
            Init(args);
        }

        protected abstract void InitListener();

        protected abstract void RemoveListener();

        protected abstract void Init(params object[] args);

        public void ShowView(EuiLayer euiLayer = EuiLayer.GameUI, params object[] args)
        {
            //只有在主界面打开后，其他界面才可以打开
            if (!isOpenedMainView && !_openViewName.Equals(MainViewName))
            {
                return;
            }

            //打开View
            UIManager.GetInstance().GetFromPool(_openViewName, euiLayer, (BaseView) =>
                 {
                     View = BaseView;

                     //记录主界面是否打开
                     if (_openViewName.Equals(MainViewName))
                     {
                         isOpenedMainView = true;
                     }

                     //ctrl是否第一次加载（在打开主界面时会加载）
                     if (!isLoad)
                     {
                         InitListener();
                         Model = GetModel();
                         Model.Init();
                         Model.BindListener();
                         isLoad = true;
                     }

                     //给主界面绑定特殊事件
                     if (BaseView.name.Equals(MainViewName))
                     {
                         View.SetClose(OnClose);
                         View.SetRelease(OnRelease);
                     }
                     else
                     {
                         View.SetClose(null);
                         View.SetRelease(null);
                     }

                     View.SetModel(Model);
                     OnBeforeShow(args);
                     View.OnShow();
                     OnShowComplate(args);
                 });
        }
        protected void SetOpenViewName(string viewName) => _openViewName = viewName;
        protected string GetOpenView() => _openViewName;

        public abstract BaseModel GetModel();

        public abstract BaseView GetView();

        public abstract string GetPrefabPath();

        public abstract void OnBeforeShow(params object[] args);

        public abstract void OnShowComplate(params object[] args);

        private void OnClose()
        {
            RemoveListener();
            Model.RemoveListener();
            isOpenedMainView = false;
        }

        private void OnRelease()
        {
            isLoad = false;
            Model = null;
            View = null;
        }

        public IMgr Ins => Global.GetInstance();
    }
}