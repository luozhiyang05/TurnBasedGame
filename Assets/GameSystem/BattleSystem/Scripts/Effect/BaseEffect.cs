using System;
using System.Collections.Generic;

namespace Assets.GameSystem.BattleSystem.Scripts.Effect
{
    [Serializable]
    public abstract class BaseEffect
    {
        public int id;
        public string effName;
        public string effDesc;
        public string iconName;
        public bool isDieEff;
        public int maxRoundCnt; //最大回合数
        protected int _remainRoundCnt; //剩余回合数

        protected AbsUnit self;
        protected List<AbsUnit> targetList;

        public void InitBaseData(int id, string effName, string effDesc,string iconName, bool isDieEff)
        {
            this.id = id;
            this.effName = effName;
            this.effDesc = effDesc;
            this.iconName = iconName;
            this.isDieEff = isDieEff;
        }

        public void StartRoundSettle()
        {
            if (_remainRoundCnt - 1 >= 0)
            {
                _remainRoundCnt--; //回合开始时，回合数-1
            }
            OnStartRoundSettle(); //执行回合开始效果
        }

        /// <summary>
        /// 回合开始时效果逻辑
        /// </summary>
        protected abstract void OnStartRoundSettle();
        /// <summary>
        /// 回合结束时效果逻辑
        /// </summary>
        protected abstract void OnEndRoundSettle();
        /// <summary>
        /// 效果结束时逻辑
        /// </summary>
        protected abstract void OnExitEffectSettle();
        /// <summary>
        /// 死亡时效果逻辑
        /// </summary>
        protected abstract void OnDieEffectSettle();

        public void DieEffectSettle()
        {
            OnDieEffectSettle(); // 死亡效果逻辑
        }

        public void EndRoundSettle()
        {
            OnEndRoundSettle(); //执行回合结束效果
        }

        public bool IsEnd()
        {
            if (_remainRoundCnt == 0)
            {
                OnExitEffectSettle();
                return true; //剩余回合为0时，当前效果结束
            }

            return false;
        }
    }
}