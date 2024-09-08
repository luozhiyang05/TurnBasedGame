using System.Collections.Generic;
using GameSystem.MVCTemplate;
using Tool.ResourceMgr;
using Tool.Single;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Tool.UI
{
    public enum EuiLayer
    {
        TipsUI, //提示
        GameUI, //游戏UI
        MenuUI, //菜单UI
        SystemUI //系统UI
    }


    public class UIManager : Singleton<UIManager>
    {
        public Vector2 Resolution
        {
            get => _canvasScaler.referenceResolution;
            set => _canvasScaler.referenceResolution = value;
        }

        private Canvas _canvas;
        private RectTransform _canvasRectTrans;
        private CanvasScaler _canvasScaler;
        private Transform _tipsUI, _gameUI, _menuUI, _systemUI;
        private Dictionary<string, BaseView> _loadBasViews = new Dictionary<string, BaseView>();

        protected override void OnInit()
        {
            _loadBasViews = new Dictionary<string, BaseView>();

            #region 创建UICanvas和EventSystem

            //创建画布
            var canvasObj = new GameObject("UICanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))
            {
                //设置UI
                layer = LayerMask.NameToLayer("UI")
            };

            //创建事件系统
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            
            //销毁保护
            Object.DontDestroyOnLoad(canvasObj);
            //销毁保护
            Object.DontDestroyOnLoad(eventSystem);

            #endregion

            #region 给画布赋值

            //获取UICanvas的组件
            _canvas = canvasObj.GetComponent<Canvas>();
            _canvasScaler = canvasObj.GetComponent<CanvasScaler>();
            _canvasRectTrans = canvasObj.GetComponent<RectTransform>();

            //设置值
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            _canvasScaler.referenceResolution = new Vector2(1920, 1080);

            //画布坐标清零
            _canvasRectTrans.localPosition = Vector3.zero;
            _canvasRectTrans.localScale = Vector3.one;
            _canvasRectTrans.offsetMax = Vector2.zero;
            _canvasRectTrans.offsetMin = Vector2.zero;

            #endregion

            #region 生成UI层

            //生成UI层
            _systemUI = new GameObject("SstemtUI").transform;
            _systemUI.SetParent(_canvasRectTrans);
            _systemUI.localPosition = Vector3.zero;

            _menuUI = new GameObject("MenuUI").transform;
            _menuUI.SetParent(_canvasRectTrans);
            _menuUI.localPosition = Vector3.zero;

            _gameUI = new GameObject("GameUI").transform;
            _gameUI.SetParent(_canvasRectTrans);
            _gameUI.localPosition = Vector3.zero;

            _tipsUI = new GameObject("TipsUI").transform;
            _tipsUI.SetParent(_canvasRectTrans);
            _tipsUI.localPosition = Vector3.zero;

            #endregion
        }

        /// <summary>
        /// 加载view预制体,获取baseView
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="euiLayer"></param>
        /// <returns></returns>
        public BaseView LoadUIPrefab(string path, EuiLayer euiLayer, UnityAction<BaseView> callback = null)
        {
            var uiGo = ResMgr.GetInstance().SyncLoad<GameObject>(path);
            InitView(uiGo, euiLayer);
            uiGo.SetActive(false);
            BaseView baseView = uiGo.GetComponent<BaseView>();
            _loadBasViews.TryAdd(path, baseView);
            callback?.Invoke(baseView);
            return baseView;
        }

        /// <summary>
        /// 打开视图
        /// </summary>
        /// <param name="viewName"></param>
        public void OpenView(string modelName)
        {
            var viewName = modelName[(modelName.LastIndexOf('.') + 1)..].Replace("Model", "");
            if (_loadBasViews.TryGetValue(viewName, out var baseView))
            {
                baseView.OnShow();
            }
            else
            {
                Debug.LogError("没有找到对应的view");
            }
        }

        /// <summary>
        /// 初始化view
        /// </summary>
        /// <param name="viewGo"></param>
        /// <param name="targetLayer"></param>
        private void InitView(GameObject viewGo, EuiLayer targetLayer)
        {
            //设置层级
            viewGo.transform.SetParent(GetFatherLayer(targetLayer));

            //坐标清零
            viewGo.transform.localPosition = Vector3.zero;
            viewGo.transform.localScale = Vector3.one;

            //锚点初始化
            var uiRectTrans = viewGo.GetComponent<RectTransform>();
            uiRectTrans.offsetMax = Vector2.zero;
            uiRectTrans.offsetMin = Vector2.zero;

            //设置UI长宽为分辨率
            uiRectTrans.sizeDelta = Resolution;
        }

        /// <summary>
        /// 获取UI层trans
        /// </summary>
        /// <param name="eUILayer"></param>
        /// <returns></returns>
        private Transform GetFatherLayer(EuiLayer eUILayer)
        {
            switch (eUILayer)
            {
                case EuiLayer.TipsUI: return _tipsUI;
                case EuiLayer.GameUI: return _gameUI;
                case EuiLayer.MenuUI: return _menuUI;
                case EuiLayer.SystemUI: return _systemUI;
                default: return null;
            }
        }
    }
}