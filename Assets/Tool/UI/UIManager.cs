using System;
using System.Collections.Generic;
using GameSystem.MVCTemplate;
using Tool.ResourceMgr;
using Tool.Single;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Tool.UI
{
    public enum EuiLayer
    {
        Top, //提示，警告
        Mid, //人物信息，血量，按钮UI
        Down, //二级弹窗
        System //UI交互
    }


    public class UIManager : Singleton<UIManager>
    {
        private Vector2 Resolution
        {
            get => _canvasScaler.referenceResolution;
            set => _canvasScaler.referenceResolution = value;
        }

        private Canvas _canvas;
        private RectTransform _canvasRectTrans;
        private CanvasScaler _canvasScaler;

        private Transform _top, _mid, _down, _system;
        private Stack<BaseView> _topStack, _midStack, _downStack, _systemStack;
        private Dictionary<string, BaseView> _loadedPanelDic = new Dictionary<string, BaseView>();
        private Dictionary<Type, GameObject> _loadedTipsDic = new Dictionary<Type, GameObject>();

        protected override void OnInit()
        {
            #region 初始化UI栈和UI字典

            _topStack = new Stack<BaseView>();
            _midStack = new Stack<BaseView>();
            _downStack = new Stack<BaseView>();
            _systemStack = new Stack<BaseView>();
            _loadedPanelDic = new Dictionary<string, BaseView>();

            #endregion

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

            #region 赋值

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
            _system = new GameObject("System").transform;
            _system.SetParent(_canvasRectTrans);
            _system.localPosition = Vector3.zero;

            _down = new GameObject("Down").transform;
            _down.SetParent(_canvasRectTrans);
            _down.localPosition = Vector3.zero;

            _mid = new GameObject("Mid").transform;
            _mid.SetParent(_canvasRectTrans);
            _mid.localPosition = Vector3.zero;

            _top = new GameObject("Top").transform;
            _top.SetParent(_canvasRectTrans);
            _top.localPosition = Vector3.zero;

            #endregion
        }

        public void Start(Vector2 resolution) => Resolution = resolution;


        /// <summary>
        /// 加载view预制体,获取baseView
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="euiLayer"></param>
        /// <returns></returns>
        public BaseView LoadViewGo(string viewName, EuiLayer euiLayer)
        {
            var loadUIBaseGo = ResMgr.GetInstance().SyncLoad<GameObject>(viewName);
            loadUIBaseGo.gameObject.SetActive(false);
            InitView(loadUIBaseGo, euiLayer);
            var baseView = loadUIBaseGo.GetComponent<BaseView>();
            _loadedPanelDic.TryAdd(viewName, baseView);
            return baseView;
        }

        
        /// <summary>
        /// 打开Tips
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LoadTips<T>() where T : BaseTips
        {
            var type = typeof(T);
            var tipsName = type.Name;
 
            if (!_loadedTipsDic.ContainsKey(type))
            {
                var loadUIBaseGo = ResMgr.GetInstance().SyncLoad<GameObject>(tipsName,false);
                _loadedTipsDic.Add(type,loadUIBaseGo);
            }

            var tipsPrefab = _loadedTipsDic[type];
            var tipsGo = Object.Instantiate(tipsPrefab);
            return tipsGo.GetComponent<T>();
        }

       
        /// <summary>
        /// 同步打开view
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="eUILayer"></param>
        /// <exception cref="Exception"></exception>
        public void OpenView(string viewName, EuiLayer eUILayer)
        {
            //根据UI层次获取队列
            Stack<BaseView> uiStack = GetUIStack(eUILayer);
            string name = viewName;

            //先判断UI字典有无,有的话判断是否打开
            if (_loadedPanelDic.TryGetValue(name, out var baseView))
            {
                //已经打开，不处理
                if (baseView.isOpen) return;

                //压入ui栈
                PushPanelToStack(baseView, uiStack);

                //打开view
                baseView.EuiLayer = eUILayer;
                baseView.OnShow();
                return;
            }

            throw new Exception("viewGo未加载");
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
        /// 初始化Tips
        /// </summary>
        /// <param name="t"></param>
        /// <typeparam name="T"></typeparam>
        public void InitTips<T>(T t) where T : BaseTips
        {
            //设置层级
            GameObject tipsGo = t.gameObject;
            tipsGo.transform.SetParent(GetFatherLayer(EuiLayer.Top));

            //坐标清零
            tipsGo.transform.localPosition = Vector3.zero;
            tipsGo.transform.localScale = Vector3.one;

            //锚点初始化
            var uiRectTrans = tipsGo.GetComponent<RectTransform>();
            uiRectTrans.offsetMax = Vector2.zero;
            uiRectTrans.offsetMin = Vector2.zero;

            //设置UI长宽为分辨率
            uiRectTrans.sizeDelta = Resolution;
        }

        /// <summary>
        /// 压入ui栈
        /// </summary>
        /// <param name="pushView"></param>
        /// <param name="targetStack"></param>
        private void PushPanelToStack(BaseView pushView, Stack<BaseView> targetStack)
        {
            //判断当前队列是否有UI,有的话当前最顶端UI失去交互
            if (targetStack.TryPeek(out var oldBase)) oldBase.CanvasGroup.interactable = false;

            //将要打开的队列入栈
            targetStack.Push(pushView);

            //顶端UI开启交互
            if (targetStack.TryPeek(out var newBase)) newBase.CanvasGroup.interactable = true;
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <param name="eUILayer"></param>
        public void ClosePanel(EuiLayer eUILayer)
        {
            //根据UI层次获取队列
            Stack<BaseView> uiStack = GetUIStack(eUILayer);
            if (!uiStack.TryPeek(out _)) return;

            //队列最顶端UI出列，失活
            if (!uiStack.TryPop(out var closeUIBase)) return;
            closeUIBase.CanvasGroup.interactable = false;
            closeUIBase.gameObject.SetActive(false);
            //closeUIBase.OnHide();

            if (uiStack.TryPeek(out var value))
            {
                value.CanvasGroup.interactable = true;
            }
        }

        /// <summary>
        /// 获取视图
        /// </summary>
        /// <param name="resPath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetUIPanel<T>(string resPath) where T : BaseView
        {
            _loadedPanelDic.TryGetValue(typeof(T).Name, out var view);
            if (view != null) return view as T;
            view = ResMgr.GetInstance().SyncLoad<T>(resPath);
            return (T)view;
        }


        /// <summary>
        /// 获取UI栈
        /// </summary>
        /// <param name="eUILayer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private Stack<BaseView> GetUIStack(EuiLayer eUILayer)
        {
            //根据UI层次获取队列
            return eUILayer switch
            {
                EuiLayer.System => _systemStack,
                EuiLayer.Down => _downStack,
                EuiLayer.Top => _topStack,
                EuiLayer.Mid => _midStack,
                _ => throw new ArgumentOutOfRangeException(nameof(eUILayer), eUILayer, null)
            };
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
                case EuiLayer.Top: return _top;
                case EuiLayer.Mid: return _mid;
                case EuiLayer.Down: return _down;
                case EuiLayer.System: return _system;
                default: return null;
            }
        }
    }
}