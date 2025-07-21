using System.Collections.Generic;
using Tool.Single;
using UnityEngine;

namespace Assets.Wights.Scripts
{
    public class RedPointMgr : Singleton<RedPointMgr>
    {
        private RedPointDef _redPointDef;
        protected override void OnInit()
        {
            _redPointDef ??= new RedPointDef();
        }

        public void Register(GameObject button, string redPointPath)
        {
            var redPointData = _redPointDef.GetRedPointData(redPointPath);
            redPointData.Register(button);
        }

        public void UnRegister(string redPointPath)
        {
            var redPointData = _redPointDef.GetRedPointData(redPointPath);
            redPointData.button = null;
        }

        public void Fire(string redPointPath, int addRedPointCnt = 1)
        {
            _redPointDef.Fire(redPointPath, addRedPointCnt);
        }
    }
}