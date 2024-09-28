
using System.Collections.Generic;
using Tool.Utilities;
using UnityEngine;

namespace Wights.Utilities
{
    public enum EWightType
    {
        card,
        cell
    }
    public class PoolWight : MonoBehaviour
    {
        private Dictionary<string, GameObject> _wightsDic;
        private Dictionary<string, QArray<GameObject>> _poolsDic;
        private Transform _poolTrans;
        private Transform _wightTrans;
        private bool _isInit = false;
        private void Init()
        {
            _poolTrans = transform.Find("Pool");
            _wightTrans = transform.Find("Wights");
            
            _poolsDic = new Dictionary<string, QArray<GameObject>>();
            _wightsDic = new Dictionary<string, GameObject>();

            foreach (Transform wight in _wightTrans)
            {
                _poolsDic.Add(wight.name, new QArray<GameObject>(1));
                _wightsDic.Add(wight.name, wight.gameObject);
            }
            _isInit = true;
        }

        public GameObject GetFromPool(EWightType eWightType)
        {
            if (_isInit == false)
            {
                Init();
            }
            
            if (_poolsDic.ContainsKey(eWightType.ToString()))
            {
                var pool = _poolsDic[eWightType.ToString()];
                if (pool.Count > 0)
                {
                    var wight = pool.GetFromHead();
                    wight.SetActive(true);
                    return wight;
                }
                
                var newWight = Instantiate(_wightsDic[eWightType.ToString()]);
                newWight.name = eWightType.ToString();
                newWight.SetActive(true);
                return newWight;
            }
            throw new System.Exception("没有找到对应的池子");
        }

        public void EnterPool(GameObject wight)
        {
            if (_isInit == false)
            {
                Init();
            }

            var poolName = wight.name;
            if (_poolsDic.ContainsKey(poolName))
            {
                wight.SetActive(false);
                wight.transform.SetParent(_poolTrans);
                _poolsDic[poolName].Add(wight);
            }
        }
    }
}