using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameSystem.BattleSystem.Scripts.Effect
{
    [Serializable]
    public class ResurrectionEffect : BaseEffect
    {
        public void ResurrectionEffData(AbsUnit selfAbs, AbsUnit target,int maxRoundCnt)
        {
            self = selfAbs;
            targetList = new List<AbsUnit>() { target };
            this.maxRoundCnt = maxRoundCnt;
            _remainRoundCnt = maxRoundCnt;
        }


        protected override void OnStartRoundSettle()
        {

        }

        protected override void OnEndRoundSettle()
        {

        }

        protected override void OnExitEffectSettle()
        {

        }

        protected override void OnDieEffectSettle()
        {
            Debug.Log("死亡复活");
            var absUnit = targetList[0];
            absUnit.nowHp.Value = absUnit.maxHp.Value;
        }
    }
}