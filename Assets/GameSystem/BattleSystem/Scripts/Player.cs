using Framework;
using GlobalData;
using Tool.Mono;
using UnityEngine;

namespace GameSystem.BattleSystem.Scripts
{
    public abstract class Player : AbsUnit, ICanGetSystem
    {
        
        /// <summary>
        /// 回合开始时结算逻辑
        /// </summary>
        protected abstract void OnStartRoundSettle();
        public override void StartRoundSettle()
        {
            base.StartRoundSettle();
            OnStartRoundSettle();
        }

        
        /// <summary>
        /// 行动逻辑
        /// </summary>
        protected abstract void OnAction();
        public override void Action()
        {
            OnAction();
            
            AfterAction();
        }

        
        /// <summary>
        /// 结算回合逻辑
        /// </summary>
        protected abstract void SettleRound();
        protected override void ExitRound()
        {
            SettleRound();
            
            SwitchRound();
        }

        
        
        public IMgr Ins => Global.GetInstance();
    }
}