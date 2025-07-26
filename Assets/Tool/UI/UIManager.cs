using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameSystem.MVCTemplate;
using Tool.Mono;
using Tool.ResourceMgr;
using Tool.Single;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
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
            get => _resolution;
            set => _resolution = value;
        }
        private Vector2 _resolution = new Vector2(1920, 1080);

        private Transform _uiLayerTrans;
        private Transform _systemUI,_tipsUI, _gameUI, _menuUI;
        private int _systemUISort = 4000;
        private int _tipsUISort = 3000;
        private int _gameUISort = 2000;
        private int _menuUISort = 1000;
        private const float GC_TIME = 5f;  //GC回收间隔
        private bool _lock = false; //GC锁
        private QArray<PrefabVo> _idlePool = new QArray<PrefabVo>(10);
        private QArray<PrefabVo> _pool = new QArray<PrefabVo>(10);
        
        protected override void OnInit()
        {            
            #region UICanvas初始化
            ActionKit.GetInstance().AddTimer(GC_Release,GC_TIME,"GC_Release",true);

            //创建UILayer
            var canvasObj = new GameObject("UILayer");
            canvasObj.layer = LayerMask.NameToLayer("UI");
            _uiLayerTrans = canvasObj.transform;
     
            //创建事件系统
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

            //销毁保护
            Object.DontDestroyOnLoad(canvasObj);
            Object.DontDestroyOnLoad(eventSystem);

            //生成UI层
            Transform CreateUILayer(EuiLayer layer)
            {
                var layerTrnas = new GameObject(layer.ToString()).transform;
                layerTrnas.SetParent(_uiLayerTrans);
                return layerTrnas;
            }
            _menuUI = CreateUILayer(EuiLayer.MenuUI);
            _gameUI = CreateUILayer(EuiLayer.GameUI);
            _tipsUI = CreateUILayer(EuiLayer.TipsUI);
            _systemUI = CreateUILayer(EuiLayer.SystemUI);
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
                if (null != cache)
                {
                    _pool.Add(cache);
                    cache.GetBaseView().transform.SetAsLastSibling();
                    cache.GetBaseView().gameObject.SetActive(true);
                    callback?.Invoke(cache.GetBaseView());
                }
                else
                {
                    //如果pool中没有prefabVO，则表明没有打开过，需要去加载
                    var prefabVo = _pool.FindValue((value) => value.GetBaseView().SystemPath == path);
                    if (null == prefabVo)
                    {
                        //没有的话，则实例化预制体到pool
                        LoadViewPrefab(path, euiLayer, (baseView) =>
                        {
                            var prefabVp = new PrefabVo(path, baseView);
                            _pool.Add(prefabVp);
                            callback?.Invoke(baseView);
                        });
                    }
                    else
                    {
                        //重复打开的视图直接执行回调
                        var baseView = prefabVo.GetBaseView();
                        callback?.Invoke(baseView);
                    }
                }
            }
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
        public void EnterIdlePool(BaseView baseView)
        {
            //将pool中的vo 移入idlePool
            var prefavVo = _pool.FindValue((value) =>
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
            //从pool中移除vo
            if (null != prefavVo)
            {
                _pool.Remove(prefavVo);
            }
        }

        /// <summary>
        /// 回收View
        /// </summary>
        private void GC_Release()
        {
            if (!_lock)
            {
                _lock = true;
                
                for (int i = 0; i < _idlePool.Count; i++)
                {
                    var vo = _idlePool[i];
                    vo.Release();
                    _idlePool.RemoveAt(i);
                    i--;
                }

                _lock = false;
            }
        }
        #endregion

        #region UI管理

        // public void SetBasePanelSort

        /// <summary>
        /// 开启遮罩
        /// </summary>
        /// <param name="baseView"></param>
        public void OpenMaskPanel(BasePanel basePanel)
        {
            var maskPanel = basePanel.transform.Find("MaskPanel");
            if (null == maskPanel) throw new System.Exception("黑幕不存在");
            var UIBlock = maskPanel.gameObject.GetComponent<UIBlock>();
            maskPanel.gameObject.SetActive(true);
            if (basePanel.UseClickMaskPanel)
            {
                UIBlock.SetClickCallback(() => basePanel.OnClickMaskPanel());
            }
        }

        /// <summary>
        /// 关闭遮罩
        /// </summary>
        public void CloseMaskPanel(BasePanel basePanel)
        {
            var maskPanel = basePanel.transform.Find("MaskPanel");
            if (null == maskPanel) throw new System.Exception("黑幕不存在");
            var UIBlock = maskPanel.gameObject.GetComponent<UIBlock>();
            UIBlock.ClearClickCallback();
            maskPanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// 加载view预制体,获取baseView
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="euiLayer"></param>
        /// <returns></returns>
        public void LoadViewPrefab(string path, EuiLayer euiLayer, UnityAction<BaseView> callback = null)
        {
            ResMgr.GetInstance().LoadView(path, (uiGo) =>
            {
                if (uiGo == null) throw new Exception($"加载UI失败：{path}");
                InitUI(uiGo, euiLayer);
                BaseView baseView = uiGo.GetComponent<BaseView>();
                callback?.Invoke(baseView); // 存入预制体池子
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
            var canvas = viewGo.GetComponent<Canvas>();
            var canvasScaler = viewGo.GetComponent<CanvasScaler>();

            //设置画布
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
            switch (targetLayer)
            {
                case EuiLayer.SystemUI:
                    canvas.sortingOrder = _systemUISort + canvas.sortingOrder;
                    break;
                case EuiLayer.TipsUI:
                    canvas.sortingOrder = _tipsUISort + canvas.sortingOrder;
                    break;
                case EuiLayer.GameUI:
                    canvas.sortingOrder = _gameUISort + canvas.sortingOrder;
                    break;
                case EuiLayer.MenuUI:
                    canvas.sortingOrder = _menuUISort + canvas.sortingOrder;
                    break;
            }
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = Resolution;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
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