using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Wights.Scripts
{
    [RequireComponent(typeof(Toggle))]
    public class CToggle : MonoBehaviour
    {
        private Toggle _toggle;
        public List<GameObject> inactiveObjs;
        public List<GameObject> activeObjs;
        public Text actText, inactText;
        private UnityAction _showViewCallback;
        private UnityAction _hideViewCallback;

        void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(NotifyToggle);
        }

        public void InitToggle(bool value,string text)
        {
            actText.text = text;
            inactText.text = text;
            NotifyToggle(value);
        }

        public void SetShowViewCallback(UnityAction callback)
        {
            _showViewCallback = callback;
        }

        public void SetHideViewCallback(UnityAction callback)
        {
            _hideViewCallback = callback;
        }

        private void NotifyToggle(bool value)
        {
            if (value)
            {
                if (activeObjs != null)
                {
                    foreach (var obj in activeObjs)
                    {
                        obj.SetActive(true);
                    }
                }
                if (inactiveObjs != null)
                {
                    foreach (var obj in inactiveObjs)
                    {
                        obj.SetActive(false);
                    }
                }
                _showViewCallback?.Invoke();
            }
            else
            {
                if (activeObjs != null)
                {
                    foreach (var obj in activeObjs)
                    {
                        obj.SetActive(false);
                    }
                }
                if (inactiveObjs != null)
                {
                    foreach (var obj in inactiveObjs)
                    {
                        obj.SetActive(true);
                    }
                }
                _hideViewCallback?.Invoke();
            }
        }
    }
}