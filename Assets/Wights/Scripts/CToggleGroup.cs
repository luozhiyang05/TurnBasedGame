using System;
using System.Collections.Generic;
using GameSystem.MVCTemplate;
using Tool.ResourceMgr;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace Assets.Wights.Scripts
{
    public struct ToggleGroupData
    {
        private List<string> _texts;
        private List<string> _viewNames;
        private List<UnityAction> _openFuns;
        private BaseCtrl ctrl;
        public ToggleGroupData SetTexts(List<string> texts)
        {
            _texts = texts;
            return this;
        }
        public List<string> GetTexts()
        {
            return _texts;
        }
        public ToggleGroupData SetCtrl(BaseCtrl baseCtrl)
        {
            ctrl = baseCtrl;
            return this;
        }
        public BaseCtrl GetCtrl()
        {
            return ctrl;
        }
        public ToggleGroupData Add<T>(T t, UnityAction fun) where T : struct
        {
            if (null == _viewNames)
            {
                _viewNames = new List<string>();
            }
            if (null == _openFuns)
            {
                _openFuns = new List<UnityAction>();
            }
            _viewNames.Add(t.ToString());
            _openFuns.Add(fun);
            return this;
        }
        public UnityAction GetOpenFun(int index)
        {
            if (index >= _openFuns.Count)
            {
                return null;
            }
            return _openFuns[index];
        }
        
        public string GetViewName(int index)
        {
            if (index >= _viewNames.Count)
            {
                return null;
            }
            return _viewNames[index];
        }
    }
    public class CToggleGroup : ToggleGroup
    {
        public GameObject viewPort;
        private List<CToggle> _toggles;
        private List<string> _texts;
        private BaseCtrl _baseCtrl;
        private ToggleGroupData _data;
        private GameObject _cToggleCell;
        private string _cToggleCellName = default;
        private Func<int,bool> _cellVisableCallback;
        private bool _init;
        public bool init;   //是否初始化
        protected override void Awake()
        {
            base.Awake();
            _toggles = new List<CToggle>();
            _texts = new List<string>();
            //默认cell
            _cToggleCell = ResMgr.GetInstance().LoadAsset("", "ToggleCell", false);
        }

        
        public void InitCToggleCell(string systemName,string assetName)
        {
           _cToggleCell = ResMgr.GetInstance().LoadAsset(systemName, assetName);
        }
        public void SetCToggleName(string name)
        {
            _cToggleCellName = name;
        }
        public void InitToggleGroup(ToggleGroupData data)
        {
            _texts = data.GetTexts();
            _baseCtrl = data.GetCtrl();
            _data = data;
            CreateCToggleCells();
            init = true;
        }
        public void SelectIndex(int index)
        {
            CheckVisable();
            //设置toggle状态
            for (int i = 0; i < m_Toggles.Count; i++)
            {
                m_Toggles[i].isOn = i == index;
            }

            //在第一次设置isOn时，可能会因为结点的isOn状态跟这里重合导致没能正确显示样式，因此手动调整样式一次
            if (!_init)
            {
                _init = true;
                for (int i = 0; i < m_Toggles.Count; i++)
                {
                    _toggles[i].InitToggle(i == index, _texts[i]);
                }
            }
        }
        public void SetVisableCallback(Func<int, bool> callback)
        {
            _cellVisableCallback = callback;
        }
        public void CheckVisable()
        {
            if (null != _cellVisableCallback)
            {
                for (int i = 0; i < _toggles.Count; i++)
                {
                    _toggles[i].gameObject.SetActive(_cellVisableCallback(i));
                }
            }
        }
        private void CreateCToggleCells()
        {
            for (int i = 0; i < _texts.Count; i++)
            {
                var cell = Instantiate(_cToggleCell);
                cell.name = _cToggleCellName == default ? "ToggleCell" : _cToggleCellName;
                cell.transform.SetParent(viewPort.transform);
                cell.GetComponent<Toggle>().group = this;

                var viewName = _data.GetViewName(i);
                var cToggle = cell.GetComponent<CToggle>();
                cToggle.SetShowViewCallback(_data.GetOpenFun(i));
                cToggle.SetHideViewCallback(() => _baseCtrl.CloseView(viewName));
                _toggles.Add(cToggle);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(viewPort.transform as RectTransform);
        }
        
    }
}