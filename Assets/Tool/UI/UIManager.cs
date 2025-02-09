using System;
using System.Collections.Generic;
using GameSystem.MVCTemplate;
using Tool.Mono;
using Tool.ResourceMgr;
using Tool.Single;
using Tool.Utilities;
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
        private Dictionary<string, BaseTips> _loadBaseTips = new Dictionary<string, BaseTips>();

        private const float GC_CHECK = 10f; //GC检查间隔
        private const float GC_TIME = 5f;  //GC回收间隔

        private bool _lock = false; //GC锁

        private QArray<PrefabVo> _idlePool = new QArray<PrefabVo>(10);

        private QArray<PrefabVo> _pool = new QArray<PrefabVo>(10);
        
        private QArray<PrefabVo> _gcQueue = new QArray<PrefabVo>(10);

        protected override void OnInit()
        {
            _loadBaseTips = new Dictionary<string, BaseTips>();
            
            #region UICanvas初始化
            ActionKit.GetInstance().AddTimer(GC_Check,GC_CHECK,"GC_Check",true);
            ActionKit.GetInstance().AddTimer(GC_Release,GC_TIME,"GC_Release",true);

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

            //生成遮罩
            _maskPanel = ResMgr.GetInstance().SyncLoad<GameObject>("MaskPanel");
            _maskPanel.transform.SetParent(_canvasRectTrans);
            _maskPanel.SetActive(false);
            
            //监听遮罩点击
            _maskPanelRaycaster = _canvasRectTrans.GetComponent<GraphicRaycaster>();
            _eventData = new PointerEventData(EventSystem.current);
            PublicMonoKit.GetInstance().OnRegisterUpdate(IsClickOnMaskPanel);

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


        #region 预制体池子
        /// <summary>
        /// 获取缓存池中的prefab
        /// </summary>
        /// <param name="path"></param>
        /// <param name="euiLayer"></param>
        /// <param name="callback"></param>
        public void GetFromPool(string path, EuiLayer euiLayer, Action<BaseView> callback)
        {
            //GC锁，避免在尝试获取缓存池时，cache被GC检查
            if (!_lock)
            {
                //如果idlePool有缓存，则直接获取，然后移入pool
                _lock = true;
                var cache = CheckIdlePool(path);
                _lock = false;
                if (cache != null)
                {
                    _pool.Add(cache);
                    cache.GetBaseView().transform.SetAsLastSibling();
                    callback?.Invoke(cache.GetBaseView());
                    return;
                }
            }

            //没有的话，则实例化预制体到pool
            LoadViewPrefab(path, euiLayer, (baseView) =>
            {
                var prefabVp = new PrefabVo(path, baseView);
                _pool.Add(prefabVp);
                callback?.Invoke(baseView);
            });
        }

        /// <summary>
        /// 检查idlePool是否有缓存
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private PrefabVo CheckIdlePool(string path)
        {
            var cache = _idlePool.Remove((value) =>
            {
                return value.GetPath() == path;
            });
            return cache;
        }

        /// <summary>
        /// 回收view
        /// </summary>
        /// <param name="baseView"></param>
        public void EnterPool(BaseView baseView)
        {
            //将pool中的vo 移入idlePool
            _pool.FindValue((value) =>
            {
                if (value.GetBaseView() == baseView)
                {
                    _idlePool.Add(value);
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// GC检查
        /// </summary>
        private void GC_Check()
        {
            if (!_lock)
            {
                _lock = true;
                while (_idlePool.Count != 0)
                {
                    var vo = _idlePool.GetFromHead();
                    _gcQueue.Add(vo);
                }
                _lock = false;
            }
        }

        /// <summary>
        /// 回收缓存
        /// </summary>
        private void GC_Release()
        {
            while (_gcQueue.Count != 0)
            {
                var vo = _gcQueue.GetFromHead();
                vo.Release();
            }
        }
        #endregion

        #region UI管理
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
        public void LoadViewPrefab(string path, EuiLayer euiLayer, UnityAction<BaseView> callback = null)
        {
            ResMgr.GetInstance().AsyncLoad<GameObject>(path,(uiGo)=>{
                if (uiGo == null) throw new Exception($"加载UI失败：{path}");
                InitUI(uiGo, euiLayer);
                uiGo.SetActive(false);
                BaseView baseView = uiGo.GetComponent<BaseView>();
                callback?.Invoke(baseView);
            });         
        }

        /// <summary>
        /// 释放view
        /// </summary>
        /// <param name="view"></param>
        /// <typeparam name="T"></typeparam>
        public void UnloadView<T>(T view) where T : BaseView
        {
            Object.Destroy(view.gameObject);
            Debug.LogWarning("<size=15><color=#9400D3>回收："  + view +$"({view.GetInstanceID()})"+ "</color></size>");
        }

        /// <summary>
        /// 关闭某个Layer曾所有View
        /// </summary>
        /// <param name="euiLayer"></param>
        public void CloseAllViewByLayer(EuiLayer euiLayer)
        {
            var viewQArray = new QArray<BaseView>();
            var layerTrans = GetFatherLayer(euiLayer);
            for (int i = layerTrans.childCount-1; i >=0; i--)   //从最顶层View开始关闭
            {
                var child = layerTrans.GetChild(i);
                if (child.TryGetComponent<BaseView>(out BaseView baseView))
                {
                    viewQArray.Add(baseView);
                }
            }
            for (int i = 0; i < viewQArray.Count; i++)
            {
                viewQArray[i].OnHide();
            }
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
        #endregion
    }

    public class PrefabVo
    {
        private string _path;
        private GameObject _go;
        private BaseView _baseView;

        public PrefabVo(string path,BaseView vo)
        {
            _path = path;
            _baseView = vo;
             _go = _baseView.gameObject;
        }

        public string GetPath()
        {
            return _path;
        }

        public BaseView GetBaseView()
        {
            return _baseView;
        }

        public void Release()
        {
            _baseView.OnRelease();
            UIManager.GetInstance().UnloadView(_baseView);
        }
    }
}