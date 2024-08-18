using System;
using Framework;
using Tool.Mono;
using UnityEngine;

namespace GameSystem.BattleSystem.Scripts
{
    public abstract class Enemy : AbsUnit, ICanGetSystem
    {
        
        /// <summary>
        /// 回合开始时结算逻辑
        /// </summary>
        protected abstract void OnStartTurnSettle();
        public override void StartTurnSettle()
        {
            OnStartTurnSettle();

            AfterStartTurnSettle();
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
        protected abstract void SettleTurn();
        protected override void ExitTurn()
        {
            SettleTurn();
            
            SwitchTurn();
        }



        public IMgr Ins => Global.GetInstance();
    }
}