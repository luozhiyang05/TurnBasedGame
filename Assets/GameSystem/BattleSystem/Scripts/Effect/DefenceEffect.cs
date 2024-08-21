using System;
using System.Collections.Generic;

namespace GameSystem.BattleSystem.Scripts.Effect
{
    public class DefenceEffect : BaseEffect
    {
        public int addAmor;

        public DefenceEffect(int maxRoundCnt) : base(maxRoundCnt)
        {
            effName = "叠甲";
        }

        public void Init(int amor)
        {
            addAmor = amor;
        }


        public override void OnStartRoundSettle()
        {
            if (!IsEnd()) return;   //如果该效果剩余回合数为0，则消除护甲
            if (self.armor-addAmor<0)
            {
                self.armor = 0;
            }
            else
            {
                self.armor -= addAmor;
            }
        }

        public override void OnEndRoundSettle()
        {

        }
    }
}