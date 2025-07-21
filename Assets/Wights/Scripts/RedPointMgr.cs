using System;
using System.Collections.Generic;
using Tool.Single;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Wights.Scripts
{
    public class RedPointMgr : Singleton<RedPointMgr>
    {
        private RedPointDef _redPointDef;
        protected override void OnInit()
        {
            _redPointDef ??= new RedPointDef();
        }

        //一般用于坏红点（只没有数据驱动，写死的红点）
        public void Register(GameObject button, string redPointPath)
        {
            var redPointData = _redPointDef.GetRedPointData(redPointPath);
            redPointData.Register(button);
        }

        //提供给数据层使用，和FireWithFun配套使用
        public void RegisterWithFun(string redPointPath, Func<int> registerFunc)
        {
            var redPointData = _redPointDef.GetRedPointData(redPointPath);
            redPointData.RegisterWithFun(registerFunc);
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

        public void FireWithFun(string redPointPath)
        {
            _redPointDef.FireWithFun(redPointPath);
        }
    }
}