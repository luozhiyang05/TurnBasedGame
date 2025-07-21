using System.Collections.Generic;
using Tool.Utilities.Bindery;
using UnityEngine;

namespace Assets.Wights.Scripts
{
    public class RedPointData
    {
        public GameObject button;
        public ValueBindery<int> redPointCnt;

        public RedPointData()
        {
            redPointCnt = new ValueBindery<int>(0);
            redPointCnt.Value = 0;
        }
        public void Register(GameObject btn)
        {
            button = btn;
            redPointCnt.OnRegisterWithValue((cnt) =>
            {
                if (null == button) return;
                var redPointGo = button.transform.Find(RedPointDef.REDPOINT_PREFAB_NAME);
                if (null == redPointGo) throw new System.Exception("红点结点go不存在");
                redPointGo.gameObject.SetActive(cnt > 0);
            });
        }
    }
    public class RedPointDef
    {   
        //红点预制体名称
        public const string REDPOINT_PREFAB_NAME = "RedPoint";

        //红点路径
        public const string ONE = "ONE";
        public const string ONE_TWO = "ONE_TWO";
        public const string ONE_TWO_TEST1 = "ONE_TWO_TEST1";
        public const string ONE_TWO_TEST2 = "ONE_TWO_TEST2";
        private Dictionary<string, RedPointData> _redPointDic;
        public RedPointDef()
        {
            _redPointDic = new Dictionary<string, RedPointData>()
            {
                {ONE,new RedPointData()},
                {ONE_TWO,new RedPointData()},
                {ONE_TWO_TEST1,new RedPointData()},
                {ONE_TWO_TEST2,new RedPointData()},
            };
        }

        public RedPointData GetRedPointData(string redPointPath)
        {
            if (_redPointDic.ContainsKey(redPointPath))
            {
                return _redPointDic[redPointPath];
            }
            throw new System.Exception("RedPointDef.GetRedPointData() redPointPath is not exist");
        }

        public void Fire(string redPointPath, int addRedPointCnt = 1)
        {
            var redPointData = GetRedPointData(redPointPath);
            var redPointCnt = redPointData.redPointCnt;
            if (redPointCnt.Value == 0 && addRedPointCnt < 0) return;
            redPointCnt.Value += addRedPointCnt;
            redPointCnt.Value = redPointCnt.Value < 0 ? 0 : redPointCnt.Value;
            while (redPointPath.LastIndexOf('_') != -1)
            {
                redPointPath = redPointPath.Substring(0, redPointPath.LastIndexOf('_'));
                redPointData = GetRedPointData(redPointPath);
                redPointData.redPointCnt.Value += addRedPointCnt;
                redPointData.redPointCnt.Value = redPointData.redPointCnt.Value < 0 ? 0 : redPointData.redPointCnt.Value;
            }
        }
    }
}