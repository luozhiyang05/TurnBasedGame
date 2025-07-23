using System.Collections.Generic;
using Framework;
using Tool.ResourceMgr;
using Tool.UI;
using Tool.Utilities;
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
        private string _openViewName;
        protected BaseModel Model;
        protected bool isLoad;
        private HashSet<SubPanelView> _loadPanels = new HashSet<SubPanelView>();
        protected string systemName;
        protected BaseCtrl(string moduleName, params object[] args)
        {
            systemName = PathUtils.GetSystemNameFromModuleName(moduleName);
            Init(args);
        }
        protected abstract void InitListener();

        protected abstract void Init(params object[] args);

        protected void SetOpenViewName(string viewName) => _openViewName = viewName;

        //ctrl调用打开视图
        public void ShowView(EuiLayer euiLayer = EuiLayer.GameUI, params object[] args)
        {
            //打开View
#if UNITY_EDITOR
#else
            _openViewName = PathUtils.GetSystemAssetBundlePath(systemName) + "/" + _openViewName;
            Debug.Log("打开视图，ab路径为：" + _openViewName);
#endif
            UIManager.GetInstance().GetFromPool(systemName + "/" + _openViewName, EuiLayer.GameUI, (BaseView) =>
                 {
                     //ctrl是否第一次加载（在打开主界面时会加载）
                     if (!isLoad)
                     {
                         InitListener();
                         Model = GetModel();
                         Model.Init();
                         Model.BindListener();
                         isLoad = true;
                     }

                     //视图绑定数据和事件，打开
                     var view = BaseView;
                     view.SetModel(Model);
                     view.SetClose(OnClose);
                     view.SetRelease(OnRelease);
                     view.OnShow(args);
                 });
        }

        //打开SubPanelView子视图，泛型为父视图
        public void OpenSubPanelView<T>(string panelName, params object[] args) where T : BaseView
        {
            SubPanelView panel = null;
            foreach (var subPanelView in _loadPanels)
            {
                if (subPanelView.name == panelName)
                {
                    panel = subPanelView;
                    break;
                }
            }
            if (null == panel)
            {
                var fatherView = args[0] as T;
                var go = ResMgr.GetInstance().LoadAsset(systemName, panelName);
                go = GameObject.Instantiate(go, fatherView.transform);
                go.transform.parent.SetAsLastSibling();
                panel = go.GetComponent<SubPanelView>();
                panel.FatherView = fatherView;
                _loadPanels.Add(panel);
            }
            panel.OnShow(args);
        }

        //供外部调用关闭子视图
        public void CloseSubPanelView(string panelName)
        {
            BasePanel targetPanel = null;
            foreach (var view in _loadPanels)
            {
                if (view.name == panelName)
                {
                    targetPanel = view;
                    break;
                }
            }
            targetPanel?.OnHide();
        }

        public abstract BaseModel GetModel();

        private void OnClose()
        {

        }

        private void OnRelease(BaseView baseView)
        {
            var subPanelViews = new List<SubPanelView>();
            foreach (var subPanelView in _loadPanels)
            {
                if (subPanelView.FatherView == baseView)
                {
                    subPanelViews.Add(subPanelView);
                }
            }
            for (int i = 0; i < subPanelViews.Count; i++)
            {
                var subPanelView = subPanelViews[i];
                subPanelView.OnRelease();
                _loadPanels.Remove(subPanelView);
            }
        }

        public IMgr Ins => Global.GetInstance();
    }
}