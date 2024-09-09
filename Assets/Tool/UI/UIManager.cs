using System;
using System.Collections.Generic;
using GameSystem.MVCTemplate;
using Tool.Mono;
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

        private GameObject _maskPanel;
        private GraphicRaycaster _maskPanelRaycaster;
        private PointerEventData _eventData;
        private Canvas _canvas;
        private RectTransform _canvasRectTrans;
        private CanvasScaler _canvasScaler;
        private Transform _warnUI,_tipsUI, _gameUI, _menuUI;
        private Dictionary<string, BaseView> _loadBaseViews = new Dictionary<string, BaseView>();
        private Dictionary<string, BaseTips> _loadBaseTips = new Dictionary<string, BaseTips>();

        protected override void OnInit()
        {
            _loadBaseViews = new Dictionary<string, BaseView>();
            _loadBaseTips = new Dictionary<string, BaseTips>();

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

            #region 生成MaskPanel
            _maskPanel = ResMgr.GetInstance().SyncLoad<GameObject>("MaskPanel");
            _maskPanel.transform.SetParent(_canvasRectTrans);
            _maskPanel.SetActive(false);
            #endregion

            #region 初始化鼠标点击检测
            _maskPanelRaycaster = _canvasRectTrans.GetComponent<GraphicRaycaster>();
            _eventData = new PointerEventData(EventSystem.current);
            PublicMonoKit.GetInstance().OnRegisterUpdate(IsClickOnMaskPanel);
            #endregion

            #region 生成UI层
            //生成UI层
            _menuUI = new GameObject("MenuUI").transform;
            _menuUI.SetParent(_canvasRectTrans);
            _menuUI.localPosition = Vector3.zero;

            _gameUI = new GameObject("GameUI").transform;
            _gameUI.SetParent(_canvasRectTrans);
            _gameUI.localPosition = Vector3.zero;

            _tipsUI = new GameObject("TipsUI").transform;
            _tipsUI.SetParent(_canvasRectTrans);
            _tipsUI.localPosition = Vector3.zero;

            _warnUI = new GameObject("SstemtUI").transform;
            _warnUI.SetParent(_canvasRectTrans);
            _warnUI.localPosition = Vector3.zero;
            #endregion
        }

        /// <summary>
        /// 开启遮罩
        /// </summary>
        /// <param name="baseView"></param>
        public void OpenMaskPanel(BasePanel basePanel)
        {
            //归位遮罩
            _maskPanel.transform.SetParent(_canvasRectTrans);
            //移动遮罩到当前view的上方
            var panelParent = basePanel.transform.parent;
            var panelIndex = basePanel.transform.GetSiblingIndex();
            _maskPanel.transform.SetParent(panelParent);
            _maskPanel.transform.SetSiblingIndex(panelIndex);
            _maskPanel.SetActive(true);
        }

        /// <summary>
        /// 关闭遮罩
        /// </summary>
        public void CloseMaskPanel()
        {
            //从该遮罩层起往上查找view，如果有view也开启了遮罩，则将该遮罩移动到该view上方
            if (_maskPanel.activeInHierarchy)
            {
                bool hasView = false;
                bool hasFindUseMaskPanelView = false;

                //从UI层的最顶层开始找起，找到有开启maskPanel的view后，将maskPanel 移动到该view上方
                for (int i = _canvasRectTrans.childCount - 1; i >= 0; i--)
                {
                    var uiLevelTrans = _canvasRectTrans.GetChild(i);
                    for (int j = uiLevelTrans.childCount - 1; j >= 0; j--)
                    {
                        BasePanel basePanel = uiLevelTrans.GetChild(j).GetComponent<BasePanel>();
                        if (basePanel != null && basePanel.gameObject.activeInHierarchy && basePanel.UseMaskPanel)
                        {
                            OpenMaskPanel(basePanel);
                            hasView = true;
                            hasFindUseMaskPanelView = true;
                            break;
                        }
                    }
                    if (hasFindUseMaskPanelView)
                    {
                        break;
                    }
                }

                if (!hasView)
                {
                    _maskPanel.SetActive(false);
                    _maskPanel.transform.SetParent(_canvasRectTrans);
                }
            }

        }

        /// <summary>
        /// 判断当前是否点击遮罩
        /// </summary>
        /// <returns></returns>
        private void IsClickOnMaskPanel()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //设置点击光标和点击位置
                _eventData.position = Input.mousePosition;
                _eventData.pressPosition = Input.mousePosition;
                //发射射线，方向为vector2.one，默认检测该点有无可阻挡射线的UI
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                _maskPanelRaycaster.Raycast(_eventData, raycastResults);
                //判空检测
                if (raycastResults.Count == 0) return;
                if (raycastResults[0].gameObject == _maskPanel)
                {
                    //触发点击遮罩事件
                    if (_maskPanel.activeInHierarchy)
                    {
                        int index = _maskPanel.transform.GetSiblingIndex();
                        BasePanel basePanel = _maskPanel.transform.parent.GetChild(index + 1).GetComponent<BasePanel>();
                        if (basePanel.UseClickMaskPanel)
                        {
                            basePanel.OnClickMaskPanel();
                        }
                    }
                }
                //释放内存
                raycastResults.Clear();
            }
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
            InitUI(uiGo, euiLayer);
            uiGo.SetActive(false);
            BaseView baseView = uiGo.GetComponent<BaseView>();
            _loadBaseViews.TryAdd(path, baseView);
            callback?.Invoke(baseView);
            return baseView;
        }

        /// <summary>
        /// 加载一个Tips
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public T LoadTips<T>(string path) where T : BaseTips
        {
            if (!_loadBaseTips.ContainsKey(path))
            {
                var uiGo = ResMgr.GetInstance().SyncLoad<GameObject>(path);
                InitUI(uiGo, EuiLayer.TipsUI);
                uiGo.SetActive(false);
                T tips = uiGo.GetComponent<T>();
                tips.path = path;
                _loadBaseTips.TryAdd(path, tips);
                return tips;
            }
            return _loadBaseTips[path] as T;
        }

        /// <summary>
        /// 释放Tips
        /// </summary>
        /// <param name="tipsName"></param>
        public void UnloadTips(string path)
        {
            BaseTips baseTips = _loadBaseTips[path];
            if (baseTips == null) throw new Exception("Tips已经释放");
            _loadBaseTips.Remove(path);
            baseTips.OnRelease();
            Object.Destroy(baseTips.gameObject);
            Debug.LogWarning("<size=24><color=#9400D3>TipsModule===>释放："  + path +$"({baseTips.GetInstanceID()})"+ "</color></size>");
        }

        
        /// <summary>
        /// 初始化view
        /// </summary>
        /// <param name="viewGo"></param>
        /// <param name="targetLayer"></param>
        private void InitUI(GameObject viewGo, EuiLayer targetLayer)
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
                case EuiLayer.SystemUI: return _warnUI;
                default: return null;
            }
        }
    }
}