using System.Collections.Generic;
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
        protected bool isLoad;
        protected bool isOpenedMainView;
        private HashSet<BaseView> _openViews = new HashSet<BaseView>();
        private string _systemName;
        protected BaseCtrl()
        {
            _mainViewName = GetPrefabPath();
            Init();
        }
        protected BaseCtrl(string systemName,params object[] args)
        {
            _systemName = systemName;
            _mainViewName = GetPrefabPath();
            Init(args);
        }
        protected abstract void InitListener();

        protected abstract void RemoveListener();

        protected abstract void Init(params object[] args);

        protected void SetOpenViewName(string viewName) => _openViewName = viewName;

        //ctrl调用打开视图
        public void ShowView(EuiLayer euiLayer = EuiLayer.GameUI, params object[] args)
        {
            //只有在主界面打开后，其他界面才可以打开
            if (!isOpenedMainView && !_openViewName.Equals(MainViewName))
            {
                return;
            }

            //打开View
#if UNITY_EDITOR
#else
            _openViewName = PathUtils.GetSystemAssetBundlePath(_systemName) + "/" + _openViewName;
            Debug.Log("打开视图，ab路径为：" + _openViewName);
#endif
            UIManager.GetInstance().GetFromPool(_openViewName, euiLayer, (BaseView) =>
                 {
                     isOpenedMainView = true;

                     var view = BaseView;
                     _openViews.Add(view);

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
                         view.SetClose(OnClose);
                         view.SetRelease(OnRelease);
                     }
                     else
                     {
                         view.SetClose(null);
                         view.SetRelease(null);
                     }

                     view.SetModel(Model);
                     OnBeforeShow(args);
                     view.OnShow();
                     OnShowComplate(args);
                     view.SetRemoveFromOpenViewsCallback(OnRemoveFormOpenViews);
                 });
        }

        //供外部调用关闭视图
        public void CloseView(string viewName)
        {
            BaseView targetView = null;
            foreach (var view in _openViews)
            {
                if (view.name == viewName)
                {
                    targetView = view;
                    break;
                }
            }
            if (null == targetView)
            {
                return;
            }
            targetView.OnHide();
            _openViews.Remove(targetView);
        }

        //获取视图
        protected T GetView<T>() where T : BaseView
        {
            foreach (var view in _openViews)
            {
                if (view is T)
                    return view as T;
            }
            return null;
        }

        public abstract BaseModel GetModel();

        public abstract string GetPrefabPath();

        public abstract void OnBeforeShow(params object[] args);

        public abstract void OnShowComplate(params object[] args);

        private void OnClose()
        {
            RemoveListener();
            Model.RemoveListener();
            isOpenedMainView = false;

            // 关闭所有子视图，它会先将所有子视图存入idlePool后，主视图才会存入idlePool，不会有顺序报空的问题
            var baseViews = new List<BaseView>();
            foreach (var view in _openViews)
            {
                if (view.name.Equals(MainViewName)) continue;
                baseViews.Add(view);
            }
            for(var i = 0; i < baseViews.Count; i++)
            {
                baseViews[i].OnHide();
            }
            baseViews.Clear();
            _openViews.Clear();
        }

        private void OnRelease()
        {
            isLoad = false;
            Model = null;
            _openViews.Clear();
            _openViews = null;
        }

        private void OnRemoveFormOpenViews(BaseView view)
        {
            _openViews.Remove(view);
        }

        public IMgr Ins => Global.GetInstance();
    }
}