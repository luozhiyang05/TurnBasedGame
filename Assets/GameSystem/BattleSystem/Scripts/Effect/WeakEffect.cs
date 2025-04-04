using System;
using System.Collections.Generic;

namespace Assets.GameSystem.BattleSystem.Scripts.Effect
{
    [Serializable]
    public class WeakEffect : BaseEffect
    {

        public void InitWeakEffData(AbsUnit selfAbs, AbsUnit target,int maxRoundCnt)
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
            targetList[0].SetWeak(false);
        }

        protected override void OnDieEffectSettle()
        {
            
        }
    }
}