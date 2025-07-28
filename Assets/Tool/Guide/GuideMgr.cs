using System;
using System.Collections.Generic;
using Tool.Mono;
using Tool.ResourceMgr;
using Tool.Single;
using Tool.UI;
using UnityEngine;

namespace Tool.Guide
{
    public enum EGuideType
    {
        ClickMask,
        ClickGuideGo, 
    }
    public class GuideMgr : Singleton<GuideMgr>
    {
        private string GUIDE_SYSTEM_VIEW_PATH = "";
        private GuideDef _guideDef;
        private GuideView _guideView;
        private Transform _maskPanel;
        private UIBlock _maskBlock;
        private Transform _main;
        private Dictionary<string, GuideData> _guideGoDict;
        private Queue<GameObject> _guideGoQueue;
        private Queue<string> _guideTxtQueue;
        private EGuideType eGuideType;
        protected override void OnInit()
        {
#if UNITY_EDITOR
            GUIDE_SYSTEM_VIEW_PATH = "Guide/GuideView.prefab";
#else
            //TODO:打包时请将GUIDE_SYSTEM_VIEW_PATH改为打包后的路径
            GUIDE_SYSTEM_VIEW_PATH = "";
#endif
            _guideGoQueue = new Queue<GameObject>();
            _guideTxtQueue = new Queue<string>();
            _guideGoDict = new Dictionary<string, GuideData>();
            _guideDef = new GuideDef();
            CreateGuideView();
        }

        public void CreateGuideView()
        {
            var go = ResMgr.GetInstance().LoadToolAsset(GUIDE_SYSTEM_VIEW_PATH);
            var layerTrans = UIManager.GetInstance().GetFatherLayer(EuiLayer.SystemUI);
            go = GameObject.Instantiate(go, layerTrans);
            _guideView = go.GetComponent<GuideView>();
            _main = _guideView.main;
            _maskPanel = _guideView.maskPanel;
            _maskBlock = _maskPanel.transform.GetComponent<UIBlock>();
        }


        /// <summary>
        /// 开始引导
        /// </summary>
        /// <param name="go"></param>
        /// <param name="txt"></param>
        private void StartGuide(GameObject go, string txt)
        {
            _maskPanel.gameObject.SetActive(true);
            var createGuideGo = GameObject.Instantiate(go, _main);
            _guideView.DisplayGuideTxt(txt);
            switch (eGuideType)
            {
                case EGuideType.ClickMask:
                    SetBlockClickCallback(_maskBlock, createGuideGo);
                    break;
                case EGuideType.ClickGuideGo:
                    createGuideGo.TryGetComponent<UIBlock>(out var block);
                    if (null == block) block = createGuideGo.AddComponent<UIBlock>();
                    SetBlockClickCallback(block, createGuideGo);
                    break;
            }
        }

        /// <summary>
        /// 引导点击事件
        /// </summary>
        /// <param name="block"></param>
        /// <param name="guideGo"></param>
        private void SetBlockClickCallback(UIBlock block, GameObject guideGo)
        {
            block.SetClickCallback(() =>
            {
                guideGo.SetActive(false);
                _guideView.NoDisplayGuideTxt();
                CheckGuide();
            });
        }

        /// <summary>
        /// 注册引导
        /// </summary>
        /// <param name="guideName"></param>
        /// <param name="go"></param>
        public GuideMgr RegisterGuide(string guideName, params GameObject[] gos)
        {
            var data = new GuideData();
            data.SetGuideGos(gos);
            if (_guideGoDict.ContainsKey(guideName))
            {
                _guideGoDict[guideName] = data;
            }
            else
            {
                _guideGoDict.Add(guideName, data);
            }
            return GetInstance();
        }
        public void WithGuideTexts(string guideName, params string[] texts)
        {
            if (_guideGoDict.ContainsKey(guideName))
            {
                _guideGoDict[guideName].SetTexts(texts);
            }
            else
            {
                throw new System.Exception("先添加引导Gos");
            }    
        }

        /// <summary>
        /// 创建单个引导
        /// </summary>
        /// <param name="guideName"></param>
        /// <param name="guideGo"></param>
        /// <param name="guideTxt"></param>
        public void RegisterOneGuide(string guideName, GameObject guideGo, string guideTxt)
        {
            if (_guideGoDict.ContainsKey(guideName))
            {
                _guideGoDict[guideName].AddGuideGo(guideGo);
                _guideGoDict[guideName].AddText(guideTxt);
            }
            else
            {
                var guideData = new GuideData();
                guideData.AddGuideGo(guideGo);
                guideData.AddText(guideTxt);
                _guideGoDict.Add(guideName, guideData);
            }
        }

        /// <summary>
        /// 激发指引
        /// </summary>
        /// <param name="eGuideType"></param>
        /// <param name="guideName"></param>
        /// <exception cref="System.Exception"></exception>
        public void FireGuideWithFun(EGuideType eGuideType, string guideName, Func<bool> func = null)
        {
            if (null != func && !func())
                return;

            if (_guideGoQueue.Count != 0)
                return;

            if (!_guideGoDict.ContainsKey(guideName))
                throw new System.Exception("没有这个引导：" + guideName);

            //获取引导Go的hashSet
            var guideData = _guideGoDict[guideName];
            foreach (var go in guideData.guideGos)
            {
                _guideGoQueue.Enqueue(go);
            }

            //获取引导文字的hashSet
            foreach (var txt in guideData.texts)
            {
                _guideTxtQueue.Enqueue(txt);
            }

            this.eGuideType = eGuideType;

            //检测引导队列
            CheckGuide();
        }

        /// <summary>
        /// 检查引导队列
        /// </summary>
        public void CheckGuide()
        {
            if (_guideGoQueue.Count == 0 || _guideTxtQueue.Count == 0)
            {
                _maskPanel.gameObject.SetActive(false);
                _maskBlock.ClearClickCallback();
                _guideView.NoDisplayGuideTxt();
                _guideGoQueue.Clear();
                _guideTxtQueue.Clear();
                ActionKit.GetInstance().DelayTime(1, () =>
                {
                    var cnt = _main.childCount;
                    for (int i = 0; i < cnt; i++)
                    {
                        var child = _main.GetChild(i);
                        if (!child.gameObject.activeSelf)
                        {
                            GameObject.Destroy(child.gameObject);
                        }
                    }
                });
                return;
            }
            var go = _guideGoQueue.Dequeue();
            var txt = _guideTxtQueue.Dequeue();
            StartGuide(go,txt);
        }
    }
}