using System;
using System.Collections.Generic;

namespace Assets.GameSystem.BattleSystem.Scripts.Effect
{
    [Serializable]
    public class DefenceEffect : BaseEffect
    {
        private int _addAmor;

        public void InitDefenceEffData(AbsUnit selfAbs, AbsUnit target,int maxRoundCnt, int amor)
        {
            self = selfAbs;
            targetList = new List<AbsUnit>() { target };
            this.maxRoundCnt = maxRoundCnt;
            _remainRoundCnt = maxRoundCnt;
            _addAmor = amor;
        }


        protected override void OnStartRoundSettle()
        {

        }

        protected override void OnEndRoundSettle()
        {

        }

        protected override void OnExitEffectSettle()
        {
            if (self.armor.Value - _addAmor < 0)
            {
                self.armor.Value = 0;
            }
            else
            {
                self.armor.Value -= _addAmor;
            }
        }
    }
}