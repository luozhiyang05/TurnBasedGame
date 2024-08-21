using System.Collections.Generic;

namespace GameSystem.BattleSystem.Scripts.Effect
{
    public abstract class BaseEffect
    {
        public string effName;
        
        public int maxRoundCnt; //最大回合数
        public int remainRoundCnt; //剩余回合数

        protected AbsUnit self;
        protected List<AbsUnit> targetList;

        public BaseEffect(int maxRoundCnt)
        {
            this.maxRoundCnt = maxRoundCnt;
            remainRoundCnt = maxRoundCnt;
        }

        public void StartRoundSettle()
        {
            if (remainRoundCnt - 1 >= 0)
            {
                remainRoundCnt--; //回合开始时，回合数-1
            }
            OnStartRoundSettle(); //执行回合开始效果
        }

        public abstract void OnStartRoundSettle();
        public abstract void OnEndRoundSettle();

        public void EndRoundSettle()
        {
            OnEndRoundSettle(); //执行回合结束效果
        }

        public bool IsEnd()
        {
            if (remainRoundCnt == 0)
            {
                return true; //剩余回合为0时，当前效果借宿
            }

            return false;
        }
    }
}