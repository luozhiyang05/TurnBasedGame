using System;
using System.Collections.Generic;

namespace GameSystem.BattleSystem.Scripts.Effect
{
    [Serializable]
    public class DefenceEffect : BaseEffect
    {
        private int _addAmor;
        
        public void Init(AbsUnit selfAbs, AbsUnit target,int amor)
        {
            self = selfAbs;
            targetList = new List<AbsUnit>() { target };
            _addAmor = amor;
            _remainRoundCnt = maxRoundCnt;
        }


        public override void OnStartRoundSettle()
        {
           
        }

        public override void OnEndRoundSettle()
        {

        }

        public override void ExitEffectSettle()
        {
            if (self.armor-_addAmor<0)
            {
                self.armor = 0;
            }
            else
            {
                self.armor -= _addAmor;
            }
        }
    }
}